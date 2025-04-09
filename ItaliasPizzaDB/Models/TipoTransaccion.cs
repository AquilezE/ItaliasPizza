using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class TipoTransaccion
    {
        [Key]
        public int IdTipoTransaccion { get; set; }
        public string TipoTransaccionesNombre { get; set; }

        public virtual ICollection<Transaccion> Transacciones { get; set; }

    }
}
