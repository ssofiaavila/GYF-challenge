using Datos.Database;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Servicios
{
    public class ProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Producto> CrearProducto(ProductoRequest dto)
        {
            var nuevoProducto = new Producto
            {
                Categoria = dto.Categoria,
                FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), 
                ExternalId = Guid.NewGuid().ToString(),
                Precio = dto.Precio
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();
            return nuevoProducto;
        }

        public async Task<Producto> ObtenerProductoPorExternalId(String externalId)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task<List<Producto>> ObtenerTodosLosProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> ActualizarProducto( ProductoRequest dto)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.ExternalId == dto.ExternalId);

            if (producto == null)
                return null;

            producto.Precio = dto.Precio;
            producto.Categoria = dto.Categoria;

            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> EliminarProducto(String externalId)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.ExternalId == externalId);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Producto>> ObtenerProductosPorPresupuesto(int presupuesto)
        {
            if (presupuesto < 1 || presupuesto > 1_000_000)
                throw new ArgumentOutOfRangeException(nameof(presupuesto), "El presupuesto debe estar entre 1 y 1.000.000.");

            // Obtener todos los productos
            var todosLosProductos = await _context.Productos
                .OrderByDescending(p => p.Precio)
                .ToListAsync();

            // Inicializar variables para la mejor combinación
            List<Producto> mejorCombinacion = null;
            double menorDiferencia = double.MaxValue;

            // Verificar que hay al menos dos categorías diferentes
            if (todosLosProductos.Count < 2)
            {
                return new List<Producto>(); // No hay suficientes productos para cumplir con los requisitos
            }

            // Iterar a través de todos los pares posibles de productos de distintas categorías
            for (int i = 0; i < todosLosProductos.Count; i++)
            {
                for (int j = i + 1; j < todosLosProductos.Count; j++)
                {
                    var producto1 = todosLosProductos[i];
                    var producto2 = todosLosProductos[j];

                    // Asegurar que los productos son de categorías diferentes
                    if (producto1.Categoria != producto2.Categoria)
                    {
                        var sumaPrecios = producto1.Precio + producto2.Precio;

                        if (sumaPrecios <= presupuesto)
                        {
                            var diferencia = presupuesto - sumaPrecios;

                            if (diferencia < menorDiferencia)
                            {
                                menorDiferencia = diferencia;
                                mejorCombinacion = new List<Producto> { producto1, producto2 };
                            }
                        }
                    }
                }
            }

            // Si no se encontró una combinación válida, retornar lista vacía
            return mejorCombinacion ?? new List<Producto>();
        }

    }
}
