using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public static List<PedidoProveedor> ObtenerPedidosPorRango(DateTime desde, DateTime hasta)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.PedidosProveedores
                    .Where(p => p.FechaPedido >= desde && p.FechaPedido <= hasta && p.Status != 0)
                    .Include(p => p.Proveedor)
                    .ToList();                                              
            }
        }


        public static int AgregarDetallePedidoAInsumos(int idPedidoProveedor)
        {

            using (var ctx = new ItaliasPizzaDbContext())
            {


                var pedido = ctx.PedidosProveedores
                    .Include("Proveedors.Insumo")
                    .FirstOrDefault(p => p.IdPedidoProveedor == idPedidoProveedor);



                if (pedido == null)
                    return 1;

                foreach (var detalle in pedido.Proveedors)
                    {
                        if (detalle.Insumo != null)
                        {
                            detalle.Insumo.Cantidad += detalle.Cantidad;
                        }
                    }

                    
                    pedido.Status = 0;

                    ctx.SaveChanges();
                    return 0;
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
