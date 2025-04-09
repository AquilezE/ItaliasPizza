using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Transaccion
    {
        [Key]
        public int IdTransaccion { get; set; }
        public DateTime Fecha { get; set; }
        public float Cantidad { get; set; }
        public string Descripcion { get; set; }

        public int IdTipoTransaccion { get; set; }

        [ForeignKey("IdTipoTransaccion")]
        public virtual TipoTransaccion TipoTransaccion { get; set; }

        public int IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }
    }
}
