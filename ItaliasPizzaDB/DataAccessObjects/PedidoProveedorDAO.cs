using ItaliasPizzaDB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.DataTransferObjects;

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

        public static List<PedidoProveedor> ObtenerPedidosPorRango(DateTime desde, DateTime hasta)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.PedidosProveedores
                    .Where(p => p.FechaPedido >= desde && p.FechaPedido <= hasta)
                    .Include(p => p.Proveedor)
                    .ToList();
            }
        }

        public static List<PedidoProveedorDTO> ObtenerPedidosProveedorReporte(DateTime? desde, DateTime? hasta)
        {
            using (var ctx = new ItaliasPizzaDbContext())
            {
                var pDesde = new SqlParameter("@Desde", desde);
                var pHasta = new SqlParameter("@Hasta", hasta);

                return ctx.Database
                    .SqlQuery<PedidoProveedorDTO>(
                        "EXEC dbo.sp03_GetPedidosProveedorReport @Desde, @Hasta",
                        pDesde, pHasta)
                    .ToList();
            }
        }
    }
}
