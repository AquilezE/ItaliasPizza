using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public float Precio { get; set; }
        public bool Status { get; set; }
        public int MaxPerOrder { get; set; }

        public int IdReceta { get; set; }
        [ForeignKey("IdReceta")]
        public virtual Receta Receta { get; set; }

        public virtual ICollection<DetallePedido> DetallePedido { get; set;}
    }
}
