using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class PedidoParaLlevar: Pedido
    {
        public int? IdDireccion { get; set; }
        public int? IdCliente { get; set; }

        [ForeignKey("IdDireccion")]
        public virtual Direccion Direccion { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}
