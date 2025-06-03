using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.Models;
using System.Web;

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
                        query = query.Where(p => p.IdStatusPedido== (int)StatusPedidoEnum.ListoParaEntrega && p is PedidoParaLocal);
                        break;
                    case (int)RolesEnum.Repartidor:
                        query = query.Where(p =>
                            (p.IdStatusPedido == (int)StatusPedidoEnum.ListoParaEntrega ||
                             p.IdStatusPedido == (int)StatusPedidoEnum.EnCamino) &&
                            p is PedidoParaLlevar); break;
                    case (int)RolesEnum.Gerente:
                        query = query.Where(p => p.IdStatusPedido == (int)StatusPedidoEnum.Cancelado || p.IdStatusPedido == (int)StatusPedidoEnum.NoEntregado);
                        break;
                    default:
                        query = query.Where(p => false);
                        break;
                }

                var list = query.ToList();
                foreach (var dom in list.OfType<PedidoParaLlevar>())
                {

                    context.Entry(dom).Reference(d => d.Direccion).Load();
                    context.Entry(dom).Reference(d => d.Cliente).Load();
                }

                return list;

            }
        }

        public static Pedido ObtenerPedidoPorId(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                var pedido = context.Pedidos
                    .Include(p => p.StatusPedido)
                    .Include(p => p.Detalles)
                    .Include(p => p.Empleado)
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido is PedidoParaLlevar dom)
                {
                    context.Entry(dom).Reference(d => d.Direccion).Load();
                    context.Entry(dom).Reference(d => d.Cliente).Load();
                }
                
                return pedido;

            }
        }

        public static int CambiarEstadoPedido(int idPedido, int idStatus)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pedido = context.Pedidos
                    .FirstOrDefault(p => p.IdPedido == idPedido);


                if (pedido == null)
                {
                    Console.WriteLine($"No se encontró el pedido con ID {idPedido}");
                    return 1;
                }

                if (pedido.IdStatusPedido == idStatus)
                {
                    return 1;
                    Console.WriteLine($"Estado anterior{pedido.IdStatusPedido}");
                    Console.WriteLine($"Estado nuevo{idStatus}");
                }

                if (pedido.IdStatusPedido == 5) return 2;

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

                        RemoverInsumosInventario(nuevoPedido.IdPedido);

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

                        RemoverInsumosInventario(nuevoPedido.IdPedido);

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

        public static int ActualizarInventarioPorPedido(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pedido = context.Pedidos
                    .Include(p => p.Detalles)
                    .Include("Detalles.Producto")
                    .Include("Detalles.Producto.Receta")
                    .Include("Detalles.Producto.Receta.InsumosParaReceta")
                    .Include("Detalles.Producto.Receta.InsumosParaReceta.Insumo")
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido == null) return 1; // Pedido no encontrado

                if (pedido.IdStatusPedido != (int)StatusPedidoEnum.Realizado &&
                    pedido.IdStatusPedido != (int)StatusPedidoEnum.Preparando)
                {
                    return 2; // Estado del pedido no válido
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var insumosNecesarios = CalcularInsumosNecesarios(pedido.Detalles);

                        if (!VerificarInventarioDisponible(context, insumosNecesarios, out string insumoFaltante))
                        {
                            transaction.Rollback();
                            return 4; 
                        }

                        ActualizarInventario(context, insumosNecesarios);

                        //if (pedido.IdStatusPedido == (int)StatusPedidoEnum.Realizado)
                        //{
                        //    pedido.IdStatusPedido = (int)StatusPedidoEnum.Preparando;
                        //}

                        context.SaveChanges();
                        transaction.Commit();
                        return 0; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error al actualizar inventario: {ex.Message}");
                        return 5; 
                    }
                }
            }
        }

        private static Dictionary<int, float> CalcularInsumosNecesarios(ICollection<DetallePedido> detalles)
            {
                var insumosNecesarios = new Dictionary<int, float>();

                foreach (var detalle in detalles)
                {
                    if (detalle.Producto?.Receta == null) continue;

                    foreach (var insumoReceta in detalle.Producto.Receta.InsumosParaReceta)
                    {
                        float cantidadTotal = (float)insumoReceta.Cantidad * detalle.Cantidad;

                        if (insumosNecesarios.ContainsKey(insumoReceta.IdInsumo))
                        {
                            insumosNecesarios[insumoReceta.IdInsumo] += cantidadTotal;
                        }
                        else
                        {
                            insumosNecesarios.Add(insumoReceta.IdInsumo, cantidadTotal);
                        }
                    }
                }

                return insumosNecesarios;
            }

        private static bool VerificarInventarioDisponible(ItaliasPizzaDbContext context,
            Dictionary<int, float> insumosNecesarios, out string insumoFaltante)
            {
                insumoFaltante = string.Empty;

                foreach (var insumo in insumosNecesarios)
                {
                    var insumoDb = context.Insumos
                        .Include(i => i.UnidadDeMedida)
                        .FirstOrDefault(i => i.IdInsumo == insumo.Key);

                    if (insumoDb == null)
                    {
                        insumoFaltante = $"Insumo ID {insumo.Key} no encontrado";
                        return false;
                    }

                    if (insumoDb.Cantidad < insumo.Value)
                    {
                        insumoFaltante = $"{insumoDb.Nombre} (Necesario: {insumo.Value} {insumoDb.UnidadDeMedida.UnidadDeMedidaNombre}, Disponible: {insumoDb.Cantidad} {insumoDb.UnidadDeMedida.UnidadDeMedidaNombre})";
                        return false;
                    }
                }

                return true;
            }

        private static void ActualizarInventario(ItaliasPizzaDbContext context, Dictionary<int, float> insumosNecesarios)
            {
                foreach (var insumo in insumosNecesarios)
                {
                var insumoDb = context.Insumos.Find(insumo.Key);
                if (insumoDb == null) continue;

                float cantidadARestar = (float)insumo.Value;
                insumoDb.Cantidad = insumoDb.Cantidad - cantidadARestar;

                context.Entry(insumoDb).Property(i => i.Cantidad).IsModified = true;
            }

            context.SaveChanges();
        }

        public static int ActualizarInventarioSinReceta(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pedido = context.Pedidos
                    .Include("Detalles")
                    .Include("Detalles.Producto")
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido == null) return 1;

                if (pedido.IdStatusPedido != (int)StatusPedidoEnum.Realizado &&
                    pedido.IdStatusPedido != (int)StatusPedidoEnum.Preparando)
                {
                    return 2;
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var insumosNecesarios = new Dictionary<int, float>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            if (detalle.Producto == null) continue;

                            context.Entry(detalle.Producto)
                                .Reference(p => p.Receta)
                                .Query()
                                .Include("InsumosParaReceta")
                                .Include("InsumosParaReceta.Insumo")
                                .Load();

                            if (detalle.Producto.Receta != null)
                            {
                                foreach (var insumoReceta in detalle.Producto.Receta.InsumosParaReceta)
                                {
                                    float cantidadTotal = insumoReceta.Cantidad * detalle.Cantidad;
                                    AcumularInsumo(insumosNecesarios, insumoReceta.IdInsumo, cantidadTotal);
                                }
                            }
                            else
                            {
                                var insumo = context.Insumos
                                    .FirstOrDefault(i => i.Nombre.Equals(detalle.Producto.Nombre, StringComparison.OrdinalIgnoreCase));

                                if (insumo == null)
                                {
                                    transaction.Rollback();
                                    return 6; 
                                }
                                AcumularInsumo(insumosNecesarios, insumo.IdInsumo, detalle.Cantidad);
                            }
                        }

                        if (!VerificarInventarioDisponible(context, insumosNecesarios, out string insumoFaltante))
                        {
                            transaction.Rollback();
                            return 4;
                        }

                        ActualizarInventario(context, insumosNecesarios);

                        if (pedido.IdStatusPedido == (int)StatusPedidoEnum.Realizado)
                        {
                            pedido.IdStatusPedido = (int)StatusPedidoEnum.Preparando;
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error: {ex.Message}");
                        return 5;
                    }
                }
            }
        }

        private static void AcumularInsumo(Dictionary<int, float> insumosNecesarios, int idInsumo, float cantidad)
        {
            if (insumosNecesarios.ContainsKey(idInsumo))
                insumosNecesarios[idInsumo] += cantidad;
            else
                insumosNecesarios.Add(idInsumo, cantidad);
        }

        public static int CancelarPedido(int idPedido, string motivoCancelacion = "")
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                using (var transactionScope = context.Database.BeginTransaction())
                {
                    try
                    {
                        var pedido = context.Pedidos.Include("Detalles").Include("Detalles.Producto").FirstOrDefault(p => p.IdPedido == idPedido);

                        if (pedido == null)
                        {
                            return 1; 
                        }

                        if (pedido.IdStatusPedido == (int)StatusPedidoEnum.Cancelado ||
                            pedido.IdStatusPedido == (int)StatusPedidoEnum.Entregado ||
                            pedido.IdStatusPedido == (int)StatusPedidoEnum.NoEntregado)
                        {
                            return 2; 
                        }

                        RestaurarInsumosInventario(idPedido);

                        pedido.IdStatusPedido = (int)StatusPedidoEnum.Cancelado;
                        context.SaveChanges();
                        transactionScope.Commit();

                        return 0;
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                        Console.WriteLine($"Error al cancelar el pedido: {ex.Message}");
                        return 5;
                    }
                }
            }
        }

        public static int MarcarPedidoNoEntregado(int idPedido, string razonNoEntregado)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var pedido = context.Pedidos
                            .Include("Detalles")
                            .Include("Detalles.Producto")
                            .FirstOrDefault(p => p.IdPedido == idPedido);

                        if (pedido == null) return 1; // Pedido no encontrado

                        if (pedido.IdStatusPedido != (int)StatusPedidoEnum.EnCamino &&
                            pedido.IdStatusPedido != (int)StatusPedidoEnum.ListoParaEntrega)
                        {
                            return 2; 
                        }

                        var insumosUsados = new Dictionary<int, float>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            if (detalle.Producto == null) continue;

                            context.Entry(detalle.Producto)
                                .Reference(p => p.Receta)
                                .Query()
                                .Include("InsumosParaReceta")
                                .Include("InsumosParaReceta.Insumo")
                                .Load();

                            if (detalle.Producto.Receta != null)
                            {
                                foreach (var insumoReceta in detalle.Producto.Receta.InsumosParaReceta)
                                {
                                    float cantidadTotal = insumoReceta.Cantidad * detalle.Cantidad;
                                    AcumularInsumo(insumosUsados, insumoReceta.IdInsumo, cantidadTotal);

                                    var merma = new Merma
                                    {
                                        IdInsumo = insumoReceta.IdInsumo,
                                        Cantidad = cantidadTotal,
                                        Fecha = DateTime.Now
                                    };
                                    context.Mermas.Add(merma);
                                }
                            }
                            else
                            {
                                var insumo = context.Insumos
                                    .FirstOrDefault(i => i.Nombre.Equals(detalle.Producto.Nombre, StringComparison.OrdinalIgnoreCase));

                                if (insumo != null)
                                {
                                    float cantidadTotal = detalle.Cantidad;
                                    insumo.Cantidad += cantidadTotal;
                                    context.Entry(insumo).Property(i => i.Cantidad).IsModified = true;
                                }
                            }
                        }

                        pedido.IdStatusPedido = (int)StatusPedidoEnum.NoEntregado;
                        pedido.RazonNoEntregado = razonNoEntregado ?? "No especificado";
                        context.SaveChanges();
                        transaction.Commit();
                        return 0; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error al marcar pedido como no entregado: {ex.Message}");
                        return 5;
                    }
                }
            }
        }

        public static int RemoverInsumosInventario(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                var pedido = context.Pedidos
                    .Include(p => p.Detalles)
                    .Include("Detalles.Producto")
                    .Include("Detalles.Producto.Receta")
                    .Include("Detalles.Producto.Receta.InsumosParaReceta")
                    .Include("Detalles.Producto.Receta.InsumosParaReceta.Insumo")
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido == null) return 1;

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var insumosNecesarios = new Dictionary<int, float>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            if (detalle.Producto == null) continue;

                            context.Entry(detalle.Producto)
                                .Reference(p => p.Receta)
                                .Query()
                                .Include("InsumosParaReceta")
                                .Include("InsumosParaReceta.Insumo")
                                .Load();

                            if (detalle.Producto.Receta != null)
                            {
                                foreach (var insumoReceta in detalle.Producto.Receta.InsumosParaReceta)
                                {
                                    float cantidadTotal = insumoReceta.Cantidad * detalle.Cantidad;
                                    AcumularInsumo(insumosNecesarios, insumoReceta.IdInsumo, cantidadTotal);
                                }
                            }
                            else
                            {
                                var insumo = context.Insumos
                                    .FirstOrDefault(i => i.Nombre.Equals(detalle.Producto.Nombre, StringComparison.OrdinalIgnoreCase));

                                if (insumo != null)
                                {
                                    AcumularInsumo(insumosNecesarios, insumo.IdInsumo, detalle.Cantidad);
                                }
                            }
                        }

                        if (!VerificarInventarioDisponible(context, insumosNecesarios, out string insumoFaltante))
                        {
                            transaction.Rollback();
                            return 4; 
                        }

                        ActualizarInventario(context, insumosNecesarios);                                                                                                                               

                        context.SaveChanges();
                        transaction.Commit();
                        return 0; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error al remover insumos del inventario: {ex.Message}");
                        return 5; 
                    }
                }
            }
        }

        public static int RestaurarInsumosInventario(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var pedido = context.Pedidos
                            .Include("Detalles")
                            .Include("Detalles.Producto")
                            .FirstOrDefault(p => p.IdPedido == idPedido);

                        if (pedido == null) return 1; 

                        if (pedido.IdStatusPedido == (int)StatusPedidoEnum.Cancelado ||
                            pedido.IdStatusPedido == (int)StatusPedidoEnum.Entregado ||
                            pedido.IdStatusPedido == (int)StatusPedidoEnum.NoEntregado)
                        {
                            return 2;
                        }

                        var insumosUsados = new Dictionary<int, float>();

                        foreach (var detalle in pedido.Detalles)
                        {
                            if (detalle.Producto == null) continue;

                            context.Entry(detalle.Producto)
                                .Reference(p => p.Receta)
                                .Query()
                                .Include("InsumosParaReceta")
                                .Include("InsumosParaReceta.Insumo")
                                .Load();

                            if (detalle.Producto.Receta != null)
                            {
                                foreach (var insumoReceta in detalle.Producto.Receta.InsumosParaReceta)
                                {
                                    float cantidadTotal = insumoReceta.Cantidad * detalle.Cantidad;
                                    AcumularInsumo(insumosUsados, insumoReceta.IdInsumo, cantidadTotal);
                                }
                            }
                            else
                            {
                                var insumo = context.Insumos
                                    .FirstOrDefault(i => i.Nombre.Equals(detalle.Producto.Nombre, StringComparison.OrdinalIgnoreCase));

                                if (insumo != null)
                                {
                                    AcumularInsumo(insumosUsados, insumo.IdInsumo, detalle.Cantidad);
                                }
                            }
                        }

                        foreach (var insumo in insumosUsados)
                        {
                            var insumoDb = context.Insumos.Find(insumo.Key);
                            if (insumoDb == null) continue;

                            
                            insumoDb.Cantidad += insumo.Value;
                            context.Entry(insumoDb).Property(i => i.Cantidad).IsModified = true;
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return 0; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error al restaurar insumos al inventario: {ex.Message}");
                        return 5;
                    }
                }
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
    
        public static List<DetallePedidoDTO> ObtenerDetallesPorPedido(int idPedido)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                
                return context.DetallesPedido
                    .Where(d => d.IdPedido == idPedido)
                    .Include(d => d.Producto)
                    .Select(d => new DetallePedidoDTO
                    {
                        IdProducto = d.Producto.IdProducto,
                        ProductoNombre = d.Producto.Nombre,
                        IdReceta = d.Producto.IdReceta,    // <— new
                        Cantidad = d.Cantidad,
                        Subtotal = d.Subtotal
                    })
                    .ToList();
            }
        }
    }
}
