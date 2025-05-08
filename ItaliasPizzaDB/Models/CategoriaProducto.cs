using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class CategoriaProducto {
        
        [Key]
        public int IdCategoriaProducto { get; set; }
        public string CategoriaProductoNombre { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }


    }
}
