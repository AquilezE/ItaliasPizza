using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataTransferObjects
{
    public class PedidoProveedorDTO
    {
        public int IdPedidoProveedor { get; set; }
        public DateTime FechaPedido { get; set; }
        public string ProveedorNombre { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
