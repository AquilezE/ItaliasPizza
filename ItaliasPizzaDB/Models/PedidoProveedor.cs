using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class PedidoProveedor
    {
        [Key]
        public int IdPedidoProveedor { get; set; }
        public DateTime FechaPedido { get; set; }
        public float Total {  get; set; }
        public byte Status { get; set; }
        //TODO: Checa si los que son bytes son bytes o bools, no se alch

        public int IdProveedor { get; set; }
        [ForeignKey("IdProveedor")]
        public virtual Proveedor Proveedor { get; set; }

        public virtual ICollection<DetallePedidoProveedor> Proveedors { get; set; }

    }
}
