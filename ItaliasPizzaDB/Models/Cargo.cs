using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }
        public string NombreCargo { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
