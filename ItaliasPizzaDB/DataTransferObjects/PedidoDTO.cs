using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataTransferObjects
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public string EmpleadoNombre { get; set; }
        public string Status { get; set; }
        public string CodigoPostal { get; set; }
        public string TelefonoCliente { get; set; }
        public int Mesa { get; set; }
        public decimal Total { get; set; }
    }

}
