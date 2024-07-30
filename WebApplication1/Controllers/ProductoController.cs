using Aplicacion.Servicios;
using Datos.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController: ControllerBase
{
        private readonly AppDbContext _context;
        private readonly ProductoService _productoService;

        public ProductoController(AppDbContext context, ProductoService productoService)
        {
            _context = context;
            _productoService = productoService;
        }

        [HttpPost("crear-producto")]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoRequest dto)
        {
            if (dto == null)
            {
                return BadRequest("Los datos del producto no pueden ser nulos");
            }

            var nuevoProducto = await _productoService.CrearProducto(dto);
            return CreatedAtAction(nameof(CrearProducto), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        [HttpGet("obtener-producto/{id}")]
        public async Task<IActionResult> ObtenerProductoPorId(String externalId)
        {
            var producto = await _productoService.ObtenerProductoPorExternalId(externalId);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpGet("obtener-todos")]
        public async Task<IActionResult> ObtenerTodosLosProductos()
        {
            var productos = await _productoService.ObtenerTodosLosProductos();
            return Ok(productos);
        }

        [HttpPut("actualizar-producto/{id}")]
        public async Task<IActionResult> ActualizarProducto([FromBody] ProductoRequest dto)
        {
            var productoActualizado = await _productoService.ActualizarProducto(dto);
            if (productoActualizado == null)
                return NotFound();

            return Ok(productoActualizado);
        }

        [HttpDelete("eliminar-producto/{id}")]
        public async Task<IActionResult> EliminarProducto(String externalId)
        {
            var resultado = await _productoService.EliminarProducto(externalId);
            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpGet("filtrar-productos-por-presupuesto")]
        public async Task<IActionResult> FiltrarProductosPorPresupuesto(int presupuesto)
        {
            try
            {
                var productos = await _productoService.ObtenerProductosPorPresupuesto(presupuesto);

                if (productos == null || productos.Count < 2)
                {
                    return NotFound("No se encontraron suficientes productos que cumplan con los criterios.");
                }

                return Ok(productos);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
