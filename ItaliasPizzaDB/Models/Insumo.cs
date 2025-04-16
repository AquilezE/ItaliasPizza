using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Insumo
    {
        [Key]
        public int IdInsumo { get; set; }
        public string Nombre { get; set; }
        public bool Status {get; set; }

        //TODO: Falta ponerle cantidad 


        public int IdCategoriaInsumo { get; set; }
        [ForeignKey("IdCategoriaInsumo")]
        public virtual CategoriaInsumo CategoriaInsumo { get; set; }


        public int IdUnidadDeMedida { get; set; }
        [ForeignKey("IdUnidadDeMedida")]
        public virtual UnidadDeMedida UnidadDeMedida { get; set; }

        public virtual ICollection<InsumoParaReceta> InsumosParaReceta { get; set; }
        public virtual ICollection<ProveedorInsumo> ProveedorInsumos {  get; set; }

    }
}
