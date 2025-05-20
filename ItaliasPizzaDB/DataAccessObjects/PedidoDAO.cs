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

        public static int RegistrarPedidoParaLlevar(int idEmpleado, int idCliente, int idDireccion, List<(int idProducto, int cantidad, float subtotal)> detalles)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        float total = detalles.Sum(d => d.subtotal);

                        var nuevoPedido = new PedidoParaLlevar
                        {
                            Fecha = DateTime.Now,
                            Total = total,
                            IdEmpleado = idEmpleado,
                            IdStatusPedido = (int)StatusPedidoEnum.Realizado,
                            IdCliente = idCliente,
                            IdDireccion = idDireccion
                        };

                        context.Pedidos.Add(nuevoPedido);
                        context.SaveChanges();

                        foreach (var detalle in detalles)
                        {
                            var nuevoDetalle = new DetallePedido
                            {
                                IdPedido = nuevoPedido.IdPedido,
                                IdProducto = detalle.idProducto,
                                Cantidad = detalle.cantidad,
                                Subtotal = detalle.subtotal
                            };

                            context.DetallesPedido.Add(nuevoDetalle);
                        }

                        context.SaveChanges();
                        transaction.Commit();

                        return nuevoPedido.IdPedido;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static int RegistrarPedidoParaLocal(int idEmpleado, int mesa, List<(int idProducto, int cantidad, float subtotal)> detalles)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        float total = detalles.Sum(d => d.subtotal);

                        var nuevoPedido = new PedidoParaLocal
                        {
                            Fecha = DateTime.Now,
                            Total = total,
                            IdEmpleado = idEmpleado,
                            IdStatusPedido = (int)StatusPedidoEnum.Realizado,
                            Mesa = mesa
                        };

                        context.Pedidos.Add(nuevoPedido);
                        context.SaveChanges();

                        foreach (var detalle in detalles)
                        {
                            var nuevoDetalle = new DetallePedido
                            {
                                IdPedido = nuevoPedido.IdPedido,
                                IdProducto = detalle.idProducto,
                                Cantidad = detalle.cantidad,
                                Subtotal = detalle.subtotal
                            };

                            context.DetallesPedido.Add(nuevoDetalle);
                        }

                        context.SaveChanges();
                        transaction.Commit();

                        return nuevoPedido.IdPedido;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public static List<Producto> ObtenerProductosActivos()
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Productos
                    .Where(p => p.Status == true)
                    .ToList();
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
