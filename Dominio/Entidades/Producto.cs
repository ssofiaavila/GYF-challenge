using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Enums;

namespace Dominio.Entidades
{
    public class Producto
    {
        public string Id { get; set; }
        public string ExternalId { get; set; } 

        public double Precio { get; set; }
        public string FechaCarga { get; set; }

        public CategoriaProducto Categoria { get; set; }
    }
}
