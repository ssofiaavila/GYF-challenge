using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public String ExternalId {  get; set; }

        public String Username {  get; set; }

        public string Password { get; set; }

        public string? Nombre { get; set; }
        public string? Apellido { get; set; }

    }
}
