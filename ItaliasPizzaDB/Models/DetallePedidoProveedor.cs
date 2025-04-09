using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class DetallePedidoProveedor
    {
        [Key]
        public int IdDetallePedidoProveedor { get; set; }

        public float Cantidad { get; set; }

        public float PrecioUnitario { get; set; }

        public int IdPedidoProveedor { get; set; }
        [ForeignKey("IdPedidoProveedor")]
        public virtual PedidoProveedor PedidoProveedor { get; set; }

        public int IdInsumo {  get; set; }
        [ForeignKey("IdInsumo")]
        public virtual Insumo Insumo{ get; set; }


    }
}
