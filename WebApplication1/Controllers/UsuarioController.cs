using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Datos.Database;
using Dominio.Entidades;
using Microsoft.AspNetCore.Hosting;
using Aplicacion.Servicios;
using NuGet.Protocol.Plugins;
using Aplicacion.DTOs;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWebHostEnvironment _env;

        private readonly AppDbContext _context;
        private readonly UsuarioService _usuarioService;


        public UsuarioController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UsuarioService usuarioService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _usuarioService = usuarioService;
            _env = webHostEnvironment;
        }


        // CREA AL USUARIO

        [HttpPost("registro")]
        public async Task<IActionResult> PostUser(UsuarioDTO usuario)
        {
            string result = await _usuarioService.CrearUsuario(usuario);

            if (result == "Usuario creado exitosamente.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("inicio-sesion")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var token = await _usuarioService.Authenticate(request.Username, request.Password);

            if (token == null)
                return Unauthorized("Usuario o contraseña inválidos");

            return Ok(new { Token = token });
        }

    }
}
