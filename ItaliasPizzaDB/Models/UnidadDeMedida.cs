using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class UnidadDeMedida
    {
        [Key]
        public int IdUnidadDeMedida { get; set; }

        public string UnidadDeMedidaNombre { get; set; }

        public virtual ICollection<Insumo> Insumos { get; set; }

    }
}
