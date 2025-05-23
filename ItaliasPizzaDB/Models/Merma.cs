using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Merma
    {
        [Key]
        public int IdMerma { get; set; }

        public int IdInsumo { get; set; }

        [ForeignKey("IdInsumo")]
        public virtual Insumo Insumo { get; set; }

        public float Cantidad { get; set; }

        public DateTime Fecha { get; set; }
    }
}
