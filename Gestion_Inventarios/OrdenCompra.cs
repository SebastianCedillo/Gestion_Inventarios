using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Inventarios
{
    public class OrdenCompra
    {
        public int OrdenId { get; set; }
        public DateTime FechaOrden { get; set; }
        public int ProveedorId { get; set; }
    }

}