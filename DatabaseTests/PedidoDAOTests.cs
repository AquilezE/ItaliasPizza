using System;
using System.Collections.Generic;
using System.Data.Entity;
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



        [Fact]
        public void RegistrarPedidoParaLlevar_RetornaIdPedidoValido()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var receta1 = new Receta { };
                    var receta2 = new Receta { };
                    context.Recetas.Add(receta1);
                    context.Recetas.Add(receta2);
                    context.SaveChanges();

                    context.Productos.Add(new Producto
                    {
                        IdProducto = 1,
                        Precio = 50.0f,
                        Status = true,
                        MaxPerOrder = 10,
                        IdReceta = receta1.IdReceta
                    });

                    context.Productos.Add(new Producto
                    {
                        IdProducto = 2,
                        Precio = 25.0f,
                        Status = true,
                        MaxPerOrder = 5,
                        IdReceta = receta2.IdReceta
                    });

                    var cliente = new Cliente { Nombre = "Cliente de prueba" };
                    var direccion = new Direccion { Calle = "Calle falsa 123" };
                    context.Clientes.Add(cliente);
                    context.Direcciones.Add(direccion);
                    context.SaveChanges();
                }

                int idEmpleado = 1;
                int idCliente = 1;  
                int idDireccion = 1; 

                var detalles = new List<(int idProducto, int cantidad, float subtotal)>
                {
                    (1, 2, 100.0f),
                    (2, 1, 25.0f)
                };

                int idPedido = PedidoDAO.RegistrarPedidoParaLlevar(idEmpleado, idCliente, idDireccion, detalles);

                Assert.True(idPedido > 0);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido = context.Pedidos
                        .OfType<PedidoParaLlevar>()  
                        .Include(p => p.Detalles)
                        .FirstOrDefault(p => p.IdPedido == idPedido);

                    Assert.NotNull(pedido);
                    Assert.Equal(125.0f, pedido.Total);
                    Assert.Equal(idEmpleado, pedido.IdEmpleado);
                    Assert.Equal((int)StatusPedidoEnum.Realizado, pedido.IdStatusPedido);

                    Assert.Equal(idCliente, pedido.IdCliente);
                    Assert.Equal(idDireccion, pedido.IdDireccion);

                    var detallesDb = context.DetallesPedido
                        .Where(d => d.IdPedido == idPedido)
                        .Include(d => d.Producto)
                        .ToList();

                    Assert.Equal(2, detallesDb.Count);

                    var detalle1 = detallesDb.FirstOrDefault(d => d.IdProducto == 1);
                    Assert.NotNull(detalle1);
                    Assert.Equal(2, detalle1.Cantidad);
                    Assert.Equal(100.0f, detalle1.Subtotal);

                    var detalle2 = detallesDb.FirstOrDefault(d => d.IdProducto == 2);
                    Assert.NotNull(detalle2);
                    Assert.Equal(1, detalle2.Cantidad);
                    Assert.Equal(25.0f, detalle2.Subtotal);
                }
            }
        }

        [Fact]
        public void RegistrarPedidoParaLocal_RetornaIdPedidoValido()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo = new Cargo { NombreCargo = "Mesero" };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();

                    var receta1 = new Receta { };
                    var receta2 = new Receta { };
                    context.Recetas.Add(receta1);
                    context.Recetas.Add(receta2);
                    context.SaveChanges();

                    var producto1 = new Producto
                    {
                        Precio = 50.0f,
                        Status = true,
                        MaxPerOrder = 10,
                        IdReceta = receta1.IdReceta
                    };

                    var producto2 = new Producto
                    {
                        Precio = 25.0f,
                        Status = true,
                        MaxPerOrder = 5,
                        IdReceta = receta2.IdReceta
                    };

                    context.Productos.Add(producto1);
                    context.Productos.Add(producto2);
                    context.SaveChanges(); 

                    var empleado = new Empleado
                    {
                        Nombre = "Empleado de prueba",
                        IdCargo = cargo.IdCargo
                    };
                    context.Empleados.Add(empleado);
                    context.SaveChanges();

                    int idEmpleado = empleado.IdEmpleado;
                    int mesa = 3;
                    var detalles = new List<(int idProducto, int cantidad, float subtotal)>
            {
                (producto1.IdProducto, 2, 100.0f), 
                (producto2.IdProducto, 1, 25.0f)    
            };
                    int idPedido = PedidoDAO.RegistrarPedidoParaLocal(idEmpleado, mesa, detalles);

                    Assert.True(idPedido > 0);

                    var pedido = context.Pedidos
                        .OfType<PedidoParaLocal>()
                        .Include(p => p.Detalles)
                        .FirstOrDefault(p => p.IdPedido == idPedido);

                    Assert.NotNull(pedido);
                    Assert.Equal(125.0f, pedido.Total);
                    Assert.Equal(idEmpleado, pedido.IdEmpleado);
                    Assert.Equal((int)StatusPedidoEnum.Realizado, pedido.IdStatusPedido);
                    Assert.Equal(mesa, pedido.Mesa);

                    var detallesDb = context.DetallesPedido
                        .Where(d => d.IdPedido == idPedido)
                        .Include(d => d.Producto)
                        .ToList();

                    Assert.Equal(2, detallesDb.Count);

                    var detalle1 = detallesDb.FirstOrDefault(d => d.IdProducto == producto1.IdProducto);
                    Assert.NotNull(detalle1);
                    Assert.Equal(2, detalle1.Cantidad);
                    Assert.Equal(100.0f, detalle1.Subtotal);

                    var detalle2 = detallesDb.FirstOrDefault(d => d.IdProducto == producto2.IdProducto);
                    Assert.NotNull(detalle2);
                    Assert.Equal(1, detalle2.Cantidad);
                    Assert.Equal(25.0f, detalle2.Subtotal);
                }
            }
        }

        [Fact]
        public void ObtenerProductosActivos_DebeRetornarSoloProductosActivos()
        {
            using (var scope = new TransactionScope())
            {
                int idProducto1;
                int idProducto2;
                int idProducto3;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Primero crear recetas necesarias
                    var receta1 = new Receta { Instrucciones = "Receta 1" };
                    var receta2 = new Receta { Instrucciones = "Receta 2" };
                    var receta3 = new Receta { Instrucciones = "Receta 3" };

                    context.Recetas.AddRange(new[] { receta1, receta2, receta3 });
                    context.SaveChanges();

                    var producto1 = new Producto
                    {
                        Precio = 100.0f,
                        Status = true,
                        MaxPerOrder = 5,
                        IdReceta = receta1.IdReceta // Usar ID de receta existente
                    };

                    var producto2 = new Producto
                    {
                        Precio = 150.0f,
                        Status = false,
                        MaxPerOrder = 3,
                        IdReceta = receta2.IdReceta // Usar ID de receta existente
                    };

                    var producto3 = new Producto
                    {
                        Precio = 200.0f,
                        Status = true,
                        MaxPerOrder = 10,
                        IdReceta = receta3.IdReceta // Usar ID de receta existente
                    };

                    context.Productos.Add(producto1);
                    context.Productos.Add(producto2);
                    context.Productos.Add(producto3);
                    context.SaveChanges();

                    idProducto1 = producto1.IdProducto;
                    idProducto2 = producto2.IdProducto;
                    idProducto3 = producto3.IdProducto;
                }

                // Corregir: Llamar al método correcto en ProductoDAO en lugar de PedidoDAO
                var productosObtenidos = PedidoDAO.ObtenerProductosActivos();

                Assert.Equal(2, productosObtenidos.Count);
                Assert.Contains(productosObtenidos, p => p.IdProducto == idProducto1);
                Assert.DoesNotContain(productosObtenidos, p => p.IdProducto == idProducto2);
                Assert.Contains(productosObtenidos, p => p.IdProducto == idProducto3);
            }
        }

        [Fact]
        public void ObtenerProductosActivos_SinProductosActivos_DebeRetornarListaVacia()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    // Primero crear las recetas necesarias
                    var receta1 = new Receta { Instrucciones = "Receta 1" };
                    var receta2 = new Receta { Instrucciones = "Receta 2" };
                    context.Recetas.AddRange(new[] { receta1, receta2 });
                    context.SaveChanges();

                    var producto1 = new Producto
                    {
                        Precio = 100.0f,
                        Status = false,
                        MaxPerOrder = 5,
                        IdReceta = receta1.IdReceta // Usar ID de receta existente
                    };

                    var producto2 = new Producto
                    {
                        Precio = 150.0f,
                        Status = false,
                        MaxPerOrder = 3,
                        IdReceta = receta2.IdReceta // Usar ID de receta existente
                    };

                    context.Productos.Add(producto1);
                    context.Productos.Add(producto2);
                    context.SaveChanges();
                }

                // Corregir: Usar ProductoDAO en lugar de PedidoDAO
                var productosObtenidos = PedidoDAO.ObtenerProductosActivos();

                Assert.Empty(productosObtenidos);
            }
        }


    }
}
