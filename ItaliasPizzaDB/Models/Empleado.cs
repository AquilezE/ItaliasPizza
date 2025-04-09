using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public bool Status { get; set; }

        public int IdCargo { get; set; }
        [ForeignKey("IdCargo")]
        public virtual Cargo Cargo { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Transaccion> Transacciones { get; set; }
        public virtual ICollection<CorteDeCaja> CortesDeCaja { get; set; }
        public virtual CuentaAcceso CuentaAcceso { get; set; }

    }
}
