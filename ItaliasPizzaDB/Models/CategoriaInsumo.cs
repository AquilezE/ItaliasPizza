using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class CategoriaInsumo
    {
        [Key]
        public int IdCategoriaInsumo { get; set; }
        public string CategoriaInsumoNombre { get; set; }

        public virtual ICollection<Insumo> Insumos { get; set; }


    }
}
