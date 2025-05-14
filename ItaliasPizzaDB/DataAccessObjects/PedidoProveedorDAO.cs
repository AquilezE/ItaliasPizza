using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public static class PedidoProveedorDAO
    {
        public static void GuardarPedido(int idProveedor, List<DetallePedidoProveedor> detalles)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var pedido = new PedidoProveedor
                {
                    FechaPedido = DateTime.Now,
                    IdProveedor = idProveedor,
                    Status = 1,
                    Total = detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                    Proveedors = detalles
                };

                context.PedidosProveedores.Add(pedido);
                context.SaveChanges();
            }
        }
    }

}
