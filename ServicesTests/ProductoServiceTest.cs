using Aplicacion.DTOs;
using Aplicacion.Servicios;
using Datos.Database;
using Dominio.Entidades;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;



namespace ServicesTests
{
    public class ProductoServiceTest
    {
        private readonly ProductoService _productoService;
        private readonly AppDbContext _dbContext;

        public ProductoServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);
            _productoService = new ProductoService(_dbContext);
        }

        [Fact]
        public async Task CrearProducto_DeberiaAgregarProducto()
        {
            var dto = new ProductoRequest
            {
                Categoria = CategoriaProducto.ProductoUno,
                Precio = 1500.00
            };

            var result = await _productoService.CrearProducto(dto);

            Assert.NotNull(result);
            Assert.Equal(CategoriaProducto.ProductoUno, result.Categoria);
            Assert.Equal(1500.00, result.Precio);
            Assert.NotEqual(default, result.FechaCarga);
            Assert.NotNull(result.ExternalId);

            var productoEnDb = await _dbContext.Productos.FindAsync(result.Id);
            Assert.NotNull(productoEnDb);
        }

        [Fact]
        public async Task ObtenerProductoPorExternalId_DeberiaRetornarProducto()
        {
            var producto = new Producto
            {
                ExternalId = "test-id",
                Categoria = CategoriaProducto.ProductoDos,
                Precio = 500.00,
                FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };
            _dbContext.Productos.Add(producto);
            await _dbContext.SaveChangesAsync();

            var result = await _productoService.ObtenerProductoPorExternalId("test-id");

            Assert.NotNull(result);
            Assert.Equal("test-id", result.ExternalId);
        }

        [Fact]
        public async Task ObtenerProductoPorExternalId_DeberiaRetornarNull_ProductoNoExiste()
        {
            var result = await _productoService.ObtenerProductoPorExternalId("non-existent-id");
            Assert.Null(result);
        }

        [Fact]
        public async Task ActualizarProducto_DeberiaActualizarProducto()
        {
            var producto = new Producto
            {
                ExternalId = "test-id-update",
                Categoria = CategoriaProducto.ProductoUno,
                Precio = 800.00,
                FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };
            _dbContext.Productos.Add(producto);
            await _dbContext.SaveChangesAsync();

            var dto = new ProductoRequest
            {
                ExternalId = "test-id-update", 
                Categoria = CategoriaProducto.ProductoUno,
                Precio = 850.00
            };

            var result = await _productoService.ActualizarProducto(dto);

            Assert.NotNull(result);
            Assert.Equal(850.00, result.Precio);

            var productoEnDb = await _dbContext.Productos.FindAsync(producto.Id);
            Assert.NotNull(productoEnDb);
            Assert.Equal(850.00, productoEnDb.Precio);
        }

        [Fact]
        public async Task EliminarProducto_DeberiaEliminarProducto()
        {
            var producto = new Producto
            {
                ExternalId = "test-id-delete",
                Categoria = CategoriaProducto.ProductoUno,
                Precio = 1200.00,
                FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            };
            _dbContext.Productos.Add(producto);
            await _dbContext.SaveChangesAsync();

            var result = await _productoService.EliminarProducto("test-id-delete");

            Assert.True(result);
            var deletedProduct = await _dbContext.Productos.FirstOrDefaultAsync(p => p.ExternalId == "test-id-delete");
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task ObtenerProductosPorPresupuesto_DeberiaRetornarProductosCorrectos()
        {
            var productos = new List<Producto>
            {
            new Producto
                {
                    Id = "1",
                    ExternalId = Guid.NewGuid().ToString(),
                    Categoria = CategoriaProducto.ProductoDos,
                    Precio = 10.00,
                    FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                },
                new Producto
                {
                    Id = "2",
                    ExternalId = Guid.NewGuid().ToString(),
                    Categoria = CategoriaProducto.ProductoUno,
                    Precio = 60.00,
                    FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                },
                new Producto
                {
                    Id = "3",
                    ExternalId = Guid.NewGuid().ToString(),
                    Categoria = CategoriaProducto.ProductoDos,
                    Precio = 5.00,
                    FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                },
                new Producto
                {
                    Id = "4",
                    ExternalId = Guid.NewGuid().ToString(),
                    Categoria = CategoriaProducto.ProductoUno,
                    Precio = 5.00,
                    FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                },
                new Producto
                {
                    Id = "5",
                    ExternalId = Guid.NewGuid().ToString(),
                    Categoria = CategoriaProducto.ProductoDos,
                    Precio = 15.00,
                    FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                }
            };

            _dbContext.Productos.AddRange(productos);
            await _dbContext.SaveChangesAsync();

            var result = await _productoService.ObtenerProductosPorPresupuesto(70);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var producto1 = result.FirstOrDefault(p => p.Categoria == CategoriaProducto.ProductoUno);
            var producto2 = result.FirstOrDefault(p => p.Categoria == CategoriaProducto.ProductoDos);

            Assert.NotNull(producto1);
            Assert.NotNull(producto2);
            Assert.Equal("2", producto1.Id);
            Assert.Equal("1", producto2.Id);
            Assert.Equal(70, producto1.Precio + producto2.Precio);
        }


        [Fact]
        public async Task ObtenerProductosPorPresupuesto_DeberiaRetornarVacio_SinProductos()
        {
            var productos = new List<Producto>
            {
                new Producto { ExternalId = Guid.NewGuid().ToString(), Categoria = CategoriaProducto.ProductoUno, Precio = 3000.00, FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") },
                new Producto { ExternalId = Guid.NewGuid().ToString(), Categoria = CategoriaProducto.ProductoDos, Precio = 4000.00, FechaCarga = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") }
            };


            _dbContext.Productos.AddRange(productos);
            await _dbContext.SaveChangesAsync();

            var result = await _productoService.ObtenerProductosPorPresupuesto(1500);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerProductosPorPresupuesto_DeberiaLanzarExcepcion_ParaPresupuestoInvalido()
        {
            var presupuestoInvalido = 0;
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _productoService.ObtenerProductosPorPresupuesto(presupuestoInvalido));
        }
    }
}
