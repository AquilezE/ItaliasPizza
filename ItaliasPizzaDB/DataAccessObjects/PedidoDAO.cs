using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class PedidoDAO
    {
        public static List<Pedido> ObtenerPedidos()
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Pedidos.ToList();
            }
        }

        public static List<Pedido> ObtenerPedidos(int cargoId)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var query = context.Pedidos
                    .Include(p => p.StatusPedido)
                    .Include(p => p.Detalles)
                    .Include(p => p.Empleado)
                    .AsQueryable();

                switch (cargoId)
                {
                    case (int)RolesEnum.Cajero: 
                        query = query.Where(p => p.IdStatusPedido == (int)StatusPedidoEnum.Realizado);
                        break;
                    case (int)RolesEnum.Cocinero: 
                        query = query.Where(p => p.IdStatusPedido == (int)StatusPedidoEnum.Realizado || p.IdStatusPedido == (int)StatusPedidoEnum.Preparando);
                        break;
                    case (int)RolesEnum.Mesero: 
                        query = query.Where(p => p.IdStatusPedido== (int)StatusPedidoEnum.ListoParaEntrega);
                        break;
                    case (int)RolesEnum.Repartidor:
                        query = query.Where(p => p.IdStatusPedido== (int)StatusPedidoEnum.ListoParaEntrega || p.IdStatusPedido == (int)StatusPedidoEnum.EnCamino);
                        break;
                    case (int)RolesEnum.Gerente:
                        query = query.Where(p => p.IdStatusPedido == (int)StatusPedidoEnum.Cancelado || p.IdStatusPedido == (int)StatusPedidoEnum.NoEntregado);
                        break;
                    default:
                        query = query.Where(p => false);
                        break;
                }

                return query.ToList();
            }
        }

        public static Pedido ObtenerPedidoPorId(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Pedidos
                    .FirstOrDefault(p => p.IdPedido == idPedido);
            }
        }

        public static int CambiarEstadoPedido(int idPedido, int idStatus)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pedido = context.Pedidos
                    .FirstOrDefault(p => p.IdPedido == idPedido);


                if (pedido == null) return 1;

                if (pedido.IdStatusPedido == idStatus) return 1;

                if (pedido.IdStatusPedido == 5) return 1;

                pedido.IdStatusPedido = idStatus;
                context.SaveChanges();

                return 0;
            }
        }

        public static List<PedidoDTO> ObtenerPedidosReporte(DateTime? desde, DateTime? hasta)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pDesde = new SqlParameter("@Desde", desde);
                var pHasta = new SqlParameter("@Hasta", hasta);

                return context.Database
                    .SqlQuery<PedidoDTO>(
                        "EXEC dbo.sp01_GetPedidosReportes @Desde, @Hasta",
                        pDesde, pHasta)
                    .ToList();
            }
        }
    }
}
