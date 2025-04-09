using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class CuentaAcceso
    {
        [Key, ForeignKey("Empleado")]  // The PK is also the FK
        public int IdEmpleado { get; set; }

        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }

        public virtual Empleado Empleado { get; set; }
    }
}
