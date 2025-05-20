using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataTransferObjects
{
    public class DetallePedidoDTO
    {
        public int IdProducto { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public float Subtotal { get; set; }
        public int? IdReceta { get; set; }    // <— new

    }
}
