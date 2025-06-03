using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace DatabaseTests
{
    public class MermaDAOTest
    {
        [Fact]
        public void RegistrarMerma_RegistraCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoParaMerma",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Cantidad= 20f,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;
                }

                float cantidadMerma = 3.5f;
                bool resultado = MermaDAO.RegistrarMerma(idInsumo, cantidadMerma);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var mermaRegistrada = context.Mermas
                        .FirstOrDefault(m => m.IdInsumo == idInsumo && m.Cantidad == cantidadMerma);

                    Assert.NotNull(mermaRegistrada);
                    Assert.Equal(idInsumo, mermaRegistrada.IdInsumo);
                    Assert.Equal(cantidadMerma, mermaRegistrada.Cantidad, 2);
                    Assert.True((DateTime.Now - mermaRegistrada.Fecha).TotalMinutes < 1);
                }
            }
        }

        [Fact]
        public void ObtenerResumenDeMermas_DeberiaRetornarTotalesPorDia()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Kg" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Vegetales" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo = new Insumo
                    {
                        Nombre = "Tomate",
                        Precio = 20,
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    context.Mermas.AddRange(new List<Merma>
                    {
                        new Merma { Cantidad = 2, Fecha = DateTime.Today, IdInsumo = insumo.IdInsumo },
                        new Merma { Cantidad = 1, Fecha = DateTime.Today, IdInsumo = insumo.IdInsumo }
                    });

                    context.SaveChanges();
                }

                var resumen = MermaDAO.ObtenerResumenDeMermas();

                Assert.NotEmpty(resumen);
                var hoy = resumen.FirstOrDefault(r => r.Fecha == DateTime.Today);
                Assert.NotNull(hoy);
                Assert.Equal(60, hoy.TotalPerdido); // 2*20 + 1*20 = 60
            }
        }

        [Fact]
        public void ObtenerDetallePorFecha_DeberiaRetornarDetallesCorrectos()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Litros" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Líquidos" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo = new Insumo
                    {
                        Nombre = "Aceite",
                        Precio = 50,
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    context.Mermas.Add(new Merma
                    {
                        Cantidad = 1.5f,
                        Fecha = DateTime.Today,
                        IdInsumo = insumo.IdInsumo
                    });
                    context.SaveChanges();
                }

                var detalles = MermaDAO.ObtenerDetallePorFecha(DateTime.Today);

                Assert.Single(detalles);
                var detalle = detalles.First();
                Assert.Equal("Aceite", detalle.NombreInsumo);
                Assert.Equal(1.5f, detalle.Cantidad);
                Assert.Equal(50, detalle.PrecioUnitario);
                Assert.Equal("Litros", detalle.Unidad);
            }
        }



    }
}
