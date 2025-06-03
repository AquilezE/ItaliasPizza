using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace DatabaseTests
{
    public class PedidoProveedorDAOTests
    {
        [Fact]
        public void GuardarPedido_DeberiaGuardarCorrectamenteConDetalles()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear unidad de medida y categoría
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Pieza" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Embalaje" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    // Crear insumo
                    var insumo = new Insumo
                    {
                        Nombre = "Caja",
                        Precio = 10,
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    // Crear proveedor
                    var proveedor = new Proveedor { Nombre = "Proveedor Prueba" };
                    context.Proveedores.Add(proveedor);
                    context.SaveChanges();

                    // Crear detalle del pedido
                    var detalle = new DetallePedidoProveedor
                    {
                        IdInsumo = insumo.IdInsumo,
                        Cantidad = 5,
                        PrecioUnitario = 10
                    };

                    // Guardar el pedido
                    PedidoProveedorDAO.GuardarPedido(proveedor.IdProveedor, new List<DetallePedidoProveedor> { detalle });

                    // Validar que se haya guardado correctamente
                    var pedidoGuardado = context.PedidosProveedores
                        .Include("Proveedors") // ← usa el nombre real del modelo
                        .FirstOrDefault();

                    Assert.NotNull(pedidoGuardado);
                    Assert.Equal(50, pedidoGuardado.Total); // 5 * 10
                    Assert.Single(pedidoGuardado.Proveedors);
                    Assert.Equal(detalle.Cantidad, pedidoGuardado.Proveedors.First().Cantidad);
                }
            }
        }

        [Fact]
        public void ObtenerPedidosPorRango_DeberiaRetornarPedidosDentroDelRango()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                DateTime hoy = DateTime.Today;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear proveedor
                    var proveedor = new Proveedor { Nombre = "Proveedor Temporal" };
                    context.Proveedores.Add(proveedor);
                    context.SaveChanges();

                    // Crear pedido dentro del rango
                    var pedido = new PedidoProveedor
                    {
                        FechaPedido = hoy,
                        IdProveedor = proveedor.IdProveedor,
                        Total = 100,
                        Status = 1
                    };
                    context.PedidosProveedores.Add(pedido);
                    context.SaveChanges();
                }

                // Ejecutar método a probar
                var pedidos = PedidoProveedorDAO.ObtenerPedidosPorRango(hoy.AddDays(-1), hoy.AddDays(1));

                // Verificar resultados
                Assert.NotNull(pedidos);
                Assert.NotEmpty(pedidos);
                Assert.Contains(pedidos, p => p.FechaPedido.Date == hoy);
            }
        }




    }
}
