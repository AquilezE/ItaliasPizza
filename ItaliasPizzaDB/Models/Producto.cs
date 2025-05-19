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
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Restricciones { get; set; }
        public string Descripcion { get; set; }
        public string ImagenRuta { get; set; }
        public float Precio { get; set; }
        public bool Status { get; set; }
        public int MaxPerOrder { get; set; }

        public int? IdReceta { get; set; }
        [ForeignKey("IdReceta")]
        public virtual Receta Receta { get; set; }

        public int IdCategoriaProducto { get; set; }
        [ForeignKey("IdCategoriaProducto")]
        public virtual CategoriaProducto CategoriaProducto { get; set; }

        public virtual ICollection<DetallePedido> DetallePedido { get; set;}
    }
}
