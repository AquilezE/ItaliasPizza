using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Receta
    {
        [Key]
        public int IdReceta {  get; set; }

        public string Instrucciones { get; set; }

        public virtual ICollection<InsumoParaReceta> InsumosParaReceta {  get; set; }        
        //TODO: Que pedo con receta???
    }
}
