using Aplicacion.Servicios;
using Datos.Database;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;
using Aplicacion.DTOs;
namespace ServicesTests
{
    public class UsuarioServiceTest
    {
        private readonly UsuarioService _usuarioService;
        private readonly AppDbContext _dbContext;

        public UsuarioServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "JWT:Secret", "your-256-bit-secret" }
                })
                .Build();

            _usuarioService = new UsuarioService(_dbContext, configuration);
        }

        [Fact]
        public async Task CrearUsuario_DeberiaCrearUsuarioExitosamente()
        {
            var usuario = new UsuarioDTO
            {
                Username = "testuser",
                Password = "testpassword",
                Nombre = "Test",
                Apellido = "User"
            };

            var result = await _usuarioService.CrearUsuario(usuario);

            Assert.Equal("Usuario creado exitosamente.", result);

            var usuarioEnDb = await _dbContext.Usuarios.SingleOrDefaultAsync(u => u.Username == "testuser");
            Assert.NotNull(usuarioEnDb);
            Assert.True(BCrypt.Net.BCrypt.Verify("testpassword", usuarioEnDb.Password));
        }

        [Fact]
        public async Task CrearUsuario_DeberiaRetornarMensajeUsuarioExistente()
        {
            var usuario = new UsuarioDTO
            {
                Username = "existinguser",
                Password = "password",
                Nombre = "Existing",
                Apellido = "User"
            };

            await _usuarioService.CrearUsuario(usuario);

            var result = await _usuarioService.CrearUsuario(usuario);

            Assert.Equal("El usuario ya existe.", result);
        }

        [Fact]
        public async Task Authenticate_DeberiaRetornarTokenCuandoUsuarioEsValido()
        {
            var usuario = new UsuarioDTO
            {
                Username = "existinguser",
                Password = "password",
                Nombre = "Existing",
                Apellido = "User"
            };

            await _usuarioService.CrearUsuario(usuario);

            var token = await _usuarioService.Authenticate("existinguser", "password");

            Assert.NotNull(token);
        }

        [Fact]
        public async Task Authenticate_DeberiaRetornarNullCuandoUsuarioEsInvalido()
        {
            var token = await _usuarioService.Authenticate("invaliduser", "invalidpassword");

            Assert.Null(token);
        }
    }
}
