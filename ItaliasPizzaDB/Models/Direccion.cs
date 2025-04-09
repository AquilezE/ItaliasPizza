using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public bool Status { get; set; }
        public string Referencia { get; set; }

        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<PedidoParaLlevar> PedidosParaLlevar { get; set; }
    }
}
