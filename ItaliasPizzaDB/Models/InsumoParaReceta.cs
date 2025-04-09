using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class InsumoParaReceta
    {
        [Key]
        public int IdInsumoParaReceta { get; set; }

        public float Cantidad { get; set; }


        public int IdReceta { get; set; }
        [ForeignKey("IdReceta")]
        public virtual Receta Receta{ get; set; }

        public int IdInsumo {  get; set; }
        [ForeignKey("IdInsumo")]
        public virtual Insumo Insumo{ get; set; }

    }
}
