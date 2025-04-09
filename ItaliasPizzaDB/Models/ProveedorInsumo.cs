using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class ProveedorInsumo
    {
        [Key, Column(Order = 0)]
        public int IdProveedor { get; set; }

        [Key, Column(Order = 1)]
        public int IdInsumo { get; set; }

        [ForeignKey("IdProveedor")]
        public virtual Proveedor Proveedor { get; set; }

        [ForeignKey("IdInsumo")]
        public virtual Insumo Insumo { get; set; }
    }
}
