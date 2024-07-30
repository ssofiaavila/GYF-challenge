using Datos.Database;
using Dominio.Entidades;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public static class InitializeDBObjects
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any() && context.Productos.Any())
            {
                return; 
            }

            var usuarios = new[]
            {
                new Usuario
                {
                    Username = "superadmin",
                    Password = BCrypt.Net.BCrypt.HashPassword("123"),
                    Nombre = "Super",
                    Apellido = "Admin",
                    ExternalId = Guid.NewGuid().ToString()
                },
                new Usuario
                {
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("123"),
                    Nombre = "Admin",
                    Apellido = "Común",
                    ExternalId = Guid.NewGuid().ToString()
                },
           
            };
            context.Usuarios.AddRange(usuarios);

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
            context.Productos.AddRange(productos);

            context.SaveChanges();
        }
    }
}
