using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace ItaliasPizzaDB.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Direccion> Direcciones { get; set; }
        public virtual ICollection<PedidoParaLlevar> PedidosParaLlevar { get; set; }
    }
}
