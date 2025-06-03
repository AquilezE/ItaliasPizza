using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public DateTime Fecha { get; set; }
        public float Total { get; set; }

        public int IdEmpleado { get; set; }
        public int IdStatusPedido { get; set; }

        public string RazonNoEntregado { get; set; } = null;

        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey("IdStatusPedido")]
        public virtual StatusPedido StatusPedido { get; set; }

        public virtual ICollection<DetallePedido> Detalles { get; set; }

    }
}
