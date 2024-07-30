using Datos.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Dominio.Entidades;
using BCrypt.Net;
using Aplicacion.DTOs;

namespace Aplicacion.Servicios
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private readonly string _jwtSecret;


        public UsuarioService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSecret = configuration["JWT:Secret"];
        }


        public async Task<string> CrearUsuario(UsuarioDTO usuario)
        {
            if (_context.Usuarios == null)
            {
                return "Entity set 'AppDbContext.Usuarios' is null.";
            }
            else
            {
                if (!_context.Usuarios.Any(e => e.Username == usuario.Username))
                {
                    // Encriptar la contraseña antes de guardar
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

                    var nuevoUsuario = new Usuario
                    {
                        Username = usuario.Username,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Password= usuario.Password,
                        ExternalId = Guid.NewGuid().ToString(),
                    };
                    _context.Usuarios.Add(nuevoUsuario);

                    await _context.SaveChangesAsync();
                    return "Usuario creado exitosamente.";
                }
                else
                {
                    return "El usuario ya existe.";
                }
            }
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _context.Usuarios.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null || !VerifyPassword(password, user.Password))
                return null;

            return GenerateJwtToken(user);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Implementa la lógica de verificación de contraseña aquí
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }





    }
}
