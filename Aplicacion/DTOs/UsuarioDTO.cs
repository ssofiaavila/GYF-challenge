using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class UsuarioDTO
    {
        public String externalId { get; set; }

        public String Username { get; set; }

        public string Password { get; set; }

        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}
