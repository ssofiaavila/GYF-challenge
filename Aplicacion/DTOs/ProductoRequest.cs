using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class ProductoRequest
    {
        public String ExternalId {  get; set; }
    
      
            public CategoriaProducto Categoria { get; set; }

            public double Precio { get; set; }
        
    }
}
