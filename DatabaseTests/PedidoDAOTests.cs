using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using Xunit;

namespace DatabaseTests
{
    [Collection("Database collection")]
    public class PedidoDAOTests
    {

        [Fact]
        public void ObtenerPedidoPorId_DebeRetornarPedido()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                DateTime fechaCreada = DateTime.Now;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = fechaCreada,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = 1
                    };

                    context.Pedidos.Add(pedido1);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                }

                var pedidoObtenido = PedidoDAO.ObtenerPedidoPorId(idPedido1);

                Assert.Equal(100.0f, pedidoObtenido.Total);
                Assert.Equal(fechaCreada.Date, pedidoObtenido.Fecha.Date);
                Assert.Equal(1, pedidoObtenido.IdEmpleado);
                Assert.Equal(1, pedidoObtenido.IdStatusPedido);
            }
        }

        [Fact]
        public void ObtenerPedidos_DebeRetornarListaDePedidos()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido2;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = 1
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 2,
                        IdStatusPedido = 2
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido2 = pedido2.IdPedido;
                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos();

                Assert.Equal(2, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido1);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido2);
            }
        }
        [Fact]
        public void ObtenerPedidosCajero_DebeObtenerPedidosRealizado()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido3;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int) StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Preparando
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int) StatusPedidoEnum.Realizado
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.Pedidos.Add(pedido3);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido3 = pedido3.IdPedido;
                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos((int)RolesEnum.Cajero);

                Assert.Equal(2, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido1);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido3);
            }
        }
        [Fact]
        public void ObtenerPedidosCocinero_DebeObtenerPedidosRealizadoYPreparando()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido2;
                int idPedido3;
                int idPedido4;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Preparando
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido4 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 400.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.Pedidos.Add(pedido3);
                    context.Pedidos.Add(pedido4);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido2 = pedido2.IdPedido;
                    idPedido3 = pedido3.IdPedido;
                    idPedido4 = pedido4.IdPedido;
                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos((int)RolesEnum.Cocinero);

                Assert.Equal(3, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido1);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido2);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido4);
                Assert.DoesNotContain(pedidosObtenidos, p => p.IdPedido == idPedido3);
            }
        }
        [Fact]
        public void ObtenerPedidosMesero_DebeObtenerPedidosListoParaEntregar()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido2;
                int idPedido3;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = 3
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = 4
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = 3
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.Pedidos.Add(pedido3);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido2 = pedido2.IdPedido;
                    idPedido3 = pedido3.IdPedido;
                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos((int)RolesEnum.Mesero);

                Assert.Equal(2, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido1);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido3);
                Assert.DoesNotContain(pedidosObtenidos, p => p.IdPedido == idPedido2);
            }
        }
        [Fact]
        public void ObtenerPedidosRepartidor_DebeObtenerPedidosEnCaminoYListos()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido2;
                int idPedido3;
                int idPedido4;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.EnCamino
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido4 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 400.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.Pedidos.Add(pedido3);
                    context.Pedidos.Add(pedido4);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido2 = pedido2.IdPedido;
                    idPedido3 = pedido3.IdPedido;
                    idPedido4 = pedido4.IdPedido;
                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos((int)RolesEnum.Repartidor);

                Assert.Equal(3, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido1);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido2);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido3);
                Assert.DoesNotContain(pedidosObtenidos, p => p.IdPedido == idPedido4);
            }
        }
        [Fact]
        public void ObtenerPedidosGerente_DebeObtenerPedidosCanceladosYNoEntregados()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;
                int idPedido2;
                int idPedido3;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Cancelado
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.NoEntregado
                    };

                    context.Pedidos.Add(pedido1);
                    context.Pedidos.Add(pedido2);
                    context.Pedidos.Add(pedido3);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                    idPedido2 = pedido2.IdPedido;
                    idPedido3 = pedido3.IdPedido;

                }

                var pedidosObtenidos = PedidoDAO.ObtenerPedidos((int)RolesEnum.Gerente);

                Assert.Equal(2, pedidosObtenidos.Count);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido2);
                Assert.Contains(pedidosObtenidos, p => p.IdPedido == idPedido3);
                Assert.DoesNotContain(pedidosObtenidos, p => p.IdPedido == idPedido1);

            }
        }

        [Fact]
        public void CambiarEstadoPedido_RetornaCeroSiSeCambioEstado()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    context.Pedidos.Add(pedido1);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                }

                var resultado = PedidoDAO.CambiarEstadoPedido(idPedido1, (int)StatusPedidoEnum.Preparando);

                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void CambiarEstadoPedido_RetornaUnoSiSeCambioEstadoYEstaCancelado()
        {
            using (var scope = new TransactionScope())
            {
                int idPedido1;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido1 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 100.0f,
                        IdEmpleado = 1,
                        IdStatusPedido = (int)StatusPedidoEnum.Cancelado
                    };

                    context.Pedidos.Add(pedido1);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                }

                var resultado = PedidoDAO.CambiarEstadoPedido(idPedido1, (int)StatusPedidoEnum.Preparando);

                Assert.Equal(1, resultado);
            }
        }

    }
}
