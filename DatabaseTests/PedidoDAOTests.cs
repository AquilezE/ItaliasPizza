using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.DataAccessObjects.ItaliasPizzaDB.DataAccessObjects;
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
                        IdEmpleado = 2,
                        IdStatusPedido = 1
                    };

                    context.Pedidos.Add(pedido1);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                }

                var pedidoObtenido = PedidoDAO.ObtenerPedidoPorId(idPedido1);

                Assert.Equal(100.0f, pedidoObtenido.Total);
                Assert.Equal(fechaCreada.Date, pedidoObtenido.Fecha.Date);
                Assert.Equal(2, pedidoObtenido.IdEmpleado);
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
                        IdEmpleado = 3,
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
                        IdEmpleado = 3,
                        IdStatusPedido = (int) StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 3,
                        IdStatusPedido = (int)StatusPedidoEnum.Preparando
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 3,
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
                        IdEmpleado = 4,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 4,
                        IdStatusPedido = (int)StatusPedidoEnum.Preparando
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 4,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido4 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 400.0f,
                        IdEmpleado = 4,
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
                        IdEmpleado = 6,
                        IdStatusPedido = 3
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 6,
                        IdStatusPedido = 4
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 6,
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
                        IdEmpleado = 5,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 5,
                        IdStatusPedido = (int)StatusPedidoEnum.EnCamino
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 5,
                        IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega
                    };

                    var pedido4 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 400.0f,
                        IdEmpleado = 5,
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
                        IdEmpleado = 2,
                        IdStatusPedido = (int)StatusPedidoEnum.Realizado
                    };

                    var pedido2 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 200.0f,
                        IdEmpleado = 2,
                        IdStatusPedido = (int)StatusPedidoEnum.Cancelado
                    };

                    var pedido3 = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Total = 300.0f,
                        IdEmpleado = 2,
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
                        IdEmpleado = 2,
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
        public void CambiarEstadoPedido_RetornaDosSiSeCambioEstadoYEstaCancelado()
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
                        IdEmpleado = 2,
                        IdStatusPedido = (int)StatusPedidoEnum.Cancelado
                    };

                    context.Pedidos.Add(pedido1);
                    context.SaveChanges();

                    idPedido1 = pedido1.IdPedido;
                }

                var resultado = PedidoDAO.CambiarEstadoPedido(idPedido1, (int)StatusPedidoEnum.Preparando);

                Assert.Equal(2, resultado);
            }
        }


        [Fact]
        public void RegistrarPedidoParaLlevar_RetornaIdPedidoValido()
        {
            using (var scope = new TransactionScope())
            {
                int idClienteReal;
                int idDireccionReal;
                int idProducto1;
                int idProducto2;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // 1. Crear categoría
                    var categoria = new CategoriaProducto { CategoriaProductoNombre = "Pizzas" };
                    context.Set<CategoriaProducto>().Add(categoria);
                    context.SaveChanges();

                    // 2. Crear recetas
                    var receta1 = new Receta();
                    var receta2 = new Receta();
                    context.Recetas.Add(receta1);
                    context.Recetas.Add(receta2);
                    context.SaveChanges();

                    // 3. Crear productos (sin IDs fijos)
                    var producto1 = new Producto
                    {
                        Precio = 50.0f,
                        Status = true,
                        MaxPerOrder = 10,
                        IdReceta = receta1.IdReceta,
                        IdCategoriaProducto = categoria.IdCategoriaProducto
                    };

                    var producto2 = new Producto
                    {
                        Precio = 25.0f,
                        Status = true,
                        MaxPerOrder = 5,
                        IdReceta = receta2.IdReceta,
                        IdCategoriaProducto = categoria.IdCategoriaProducto
                    };

                    context.Productos.Add(producto1);
                    context.Productos.Add(producto2);
                    context.SaveChanges();

                    idProducto1 = producto1.IdProducto;
                    idProducto2 = producto2.IdProducto;

                    // 4. Crear cliente y dirección
                    var cliente = new Cliente { Nombre = "Cliente de prueba" };
                    var direccion = new Direccion { Calle = "Calle falsa 123" };
                    context.Clientes.Add(cliente);
                    context.Direcciones.Add(direccion);
                    context.SaveChanges();

                    idClienteReal = cliente.IdCliente;
                    idDireccionReal = direccion.IdDireccion;
                }

                int idEmpleado = 2;
                var detalles = new List<(int idProducto, int cantidad, float subtotal)>
        {
            (idProducto1, 2, 100.0f),  // Usar ID real del producto 1
            (idProducto2, 1, 25.0f)     // Usar ID real del producto 2
        };

                int idPedido = PedidoDAO.RegistrarPedidoParaLlevar(
                    idEmpleado,
                    idClienteReal,
                    idDireccionReal,
                    detalles);

                // Verificaciones...
                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido = context.Pedidos
                        .OfType<PedidoParaLlevar>()
                        .Include(p => p.Detalles)
                        .FirstOrDefault(p => p.IdPedido == idPedido);

                    Assert.NotNull(pedido);
                    Assert.Equal(125.0f, pedido.Total);

                    var detallesDb = context.DetallesPedido
                        .Where(d => d.IdPedido == idPedido)
                        .Include(d => d.Producto)
                        .ToList();

                    Assert.Equal(2, detallesDb.Count);
                    Assert.Contains(detallesDb, d => d.IdProducto == idProducto1);
                    Assert.Contains(detallesDb, d => d.IdProducto == idProducto2);
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
                    // 1. Crear categoría de producto primero
                    var categoria = new CategoriaProducto
                    {
                        CategoriaProductoNombre = "Pizzas"
                    };
                    context.Set<CategoriaProducto>().Add(categoria);
                    context.SaveChanges();

                    var cargo = new Cargo { NombreCargo = "Mesero" };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();

                    var receta1 = new Receta();
                    var receta2 = new Receta();
                    context.Recetas.Add(receta1);
                    context.Recetas.Add(receta2);
                    context.SaveChanges();

                    // 2. Crear productos asignando la categoría
                    var producto1 = new Producto
                    {
                        Precio = 50.0f,
                        Status = true,
                        MaxPerOrder = 10,
                        IdReceta = receta1.IdReceta,
                        IdCategoriaProducto = categoria.IdCategoriaProducto // Asignar categoría
                    };

                    var producto2 = new Producto
                    {
                        Precio = 25.0f,
                        Status = true,
                        MaxPerOrder = 5,
                        IdReceta = receta2.IdReceta,
                        IdCategoriaProducto = categoria.IdCategoriaProducto // Asignar categoría
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
                int idProducto1, idProducto2, idProducto3;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // 1. Crear categoría
                    var categoria = new CategoriaProducto
                    {
                        CategoriaProductoNombre = "Test"
                    };
                    context.Set<CategoriaProducto>().Add(categoria);

                    // 2. Crear recetas
                    var recetas = new List<Receta>
            {
                new Receta { Instrucciones = "Receta 1" },
                new Receta { Instrucciones = "Receta 2" },
                new Receta { Instrucciones = "Receta 3" }
            };
                    context.Set<Receta>().AddRange(recetas);
                    context.SaveChanges();

                    // 3. Crear productos
                    var productos = new List<Producto>
            {
                new Producto
                {
                    Nombre = "Producto 1",
                    Codigo = "P1",
                    Precio = 100.0f,
                    Status = true,
                    MaxPerOrder = 5,
                    IdCategoriaProducto = categoria.IdCategoriaProducto,
                    IdReceta = recetas[0].IdReceta
                },
                new Producto
                {
                    Nombre = "Producto 2",
                    Codigo = "P2",
                    Precio = 150.0f,
                    Status = false,
                    MaxPerOrder = 3,
                    IdCategoriaProducto = categoria.IdCategoriaProducto,
                    IdReceta = recetas[1].IdReceta
                },
                new Producto
                {
                    Nombre = "Producto 3",
                    Codigo = "P3",
                    Precio = 200.0f,
                    Status = true,
                    MaxPerOrder = 10,
                    IdCategoriaProducto = categoria.IdCategoriaProducto,
                    IdReceta = recetas[2].IdReceta
                }
            };
                    context.Set<Producto>().AddRange(productos);
                    context.SaveChanges();

                    idProducto1 = productos[0].IdProducto;
                    idProducto2 = productos[1].IdProducto;
                    idProducto3 = productos[2].IdProducto;
                }

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
                    // 1. Crear categoría
                    var categoria = new CategoriaProducto
                    {
                        CategoriaProductoNombre = "Test"
                    };
                    context.Set<CategoriaProducto>().Add(categoria);

                    // 2. Crear recetas
                    var recetas = new List<Receta>
                    {
                        new Receta { Instrucciones = "Receta 1" },
                        new Receta { Instrucciones = "Receta 2" }
                    };
                    context.Set<Receta>().AddRange(recetas);
                    context.SaveChanges();

                    // 3. Crear productos inactivos
                    var productos = new List<Producto>
                    {
                        new Producto
                        {
                            Nombre = "Producto 1",
                            Codigo = "P1",
                            Precio = 100.0f,
                            Status = false,
                            MaxPerOrder = 5,
                            IdCategoriaProducto = categoria.IdCategoriaProducto,
                            IdReceta = recetas[0].IdReceta
                        },
                        new Producto
                        {
                            Nombre = "Producto 2",
                            Codigo = "P2",
                            Precio = 150.0f,
                            Status = false,
                            MaxPerOrder = 3,
                            IdCategoriaProducto = categoria.IdCategoriaProducto,
                            IdReceta = recetas[1].IdReceta
                        }
                    };
                    context.Set<Producto>().AddRange(productos);
                    context.SaveChanges();
                }

                var productosObtenidos = PedidoDAO.ObtenerProductosActivos();

                Assert.Empty(productosObtenidos);
            }
        }


    }
}
