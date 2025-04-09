using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        //TODO: Deberiamos darle una direccion al proovedor???
        public string Direccion { get; set;}

        public virtual ICollection<PedidoProveedor> Pedidos { get; set; }
        public virtual ICollection<ProveedorInsumo> ProveedorInsumos { get; set; }

    }
}
