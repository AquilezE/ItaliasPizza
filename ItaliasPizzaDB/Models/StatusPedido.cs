using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Models
{
    public class StatusPedido
    {
        [Key]
        public int IdStatusPedido { get; set; }
        public string Status { get; set; }


        public virtual ICollection<Pedido> Pedidos { get; set; }

    }
}
