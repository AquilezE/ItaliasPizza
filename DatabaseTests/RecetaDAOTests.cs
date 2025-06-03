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

    public class RecetaDAOTests
    {
        [Fact]
        public void GuardarReceta_DeberiaPersistirRecetaConInsumos()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Gramos" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.SaveChanges();

                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Categoría Test" };
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo = new Insumo
                    {
                        Nombre = "Harina",
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    var receta = new Receta
                    {
                        Instrucciones = "Mezclar y hornear",
                        InsumosParaReceta = new List<InsumoParaReceta>
                        {
                            new InsumoParaReceta { Insumo = insumo, Cantidad = 200 }
                        }
                    };

                    RecetaDAO.GuardarReceta(receta);

                    Assert.True(receta.IdReceta > 0);
                    var guardada = context.Recetas.Include("InsumosParaReceta").FirstOrDefault(r => r.IdReceta == receta.IdReceta);
                    Assert.NotNull(guardada);
                    Assert.NotEmpty(guardada.InsumosParaReceta);
                }
            }
        }

        [Fact]
        public void EliminarReceta_DeberiaEliminarRecetaYRelacionados()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idReceta;
                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Litros" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Líquido" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo = new Insumo
                    {
                        Nombre = "Agua",
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    var receta = new Receta
                    {
                        Instrucciones = "Hervir",
                        InsumosParaReceta = new List<InsumoParaReceta>
                        {
                            new InsumoParaReceta { Insumo = insumo, Cantidad = 500 }
                        }
                    };
                    context.Recetas.Add(receta);
                    context.SaveChanges();
                    idReceta = receta.IdReceta;
                }

                RecetaDAO.EliminarReceta(idReceta);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var recetaEliminada = context.Recetas.FirstOrDefault(r => r.IdReceta == idReceta);
                    Assert.Null(recetaEliminada);
                }
            }
        }

        [Fact]
        public void ActualizarRecetaDeProducto_DeberiaActualizarInsumosYInstrucciones()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Kg" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Carnes" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo1 = new Insumo { Nombre = "Carne molida", UnidadDeMedida = unidad, IdCategoriaInsumo = categoria.IdCategoriaInsumo };
                    var insumo2 = new Insumo { Nombre = "Tocino", UnidadDeMedida = unidad, IdCategoriaInsumo = categoria.IdCategoriaInsumo };
                    context.Insumos.Add(insumo1);
                    context.Insumos.Add(insumo2);
                    context.SaveChanges();

                    var receta = new Receta
                    {
                        Instrucciones = "Freír carne",
                        InsumosParaReceta = new List<InsumoParaReceta>
                {
                    new InsumoParaReceta { Insumo = insumo1, Cantidad = 300 }
                }
                    };
                    context.Recetas.Add(receta);
                    context.SaveChanges();

                    var categoriaProducto = new CategoriaProducto { CategoriaProductoNombre = "Especial" };
                    context.CategoriasProducto.Add(categoriaProducto);
                    context.SaveChanges();

                    var producto = new Producto
                    {
                        Nombre = "Pizza BBQ",
                        Precio = 120,
                        IdCategoriaProducto = categoriaProducto.IdCategoriaProducto,
                        IdReceta = receta.IdReceta
                    };
                    context.Productos.Add(producto);
                    context.SaveChanges();
                    idProducto = producto.IdProducto;
                }

                var recetaNueva = new Receta
                {
                    Instrucciones = "Hornear con tocino",
                    InsumosParaReceta = new List<InsumoParaReceta>
            {
                new InsumoParaReceta { IdInsumo = 2, Cantidad = 150 } // Asegúrate que ese insumo exista
            }
                };

                RecetaDAO.ActualizarRecetaDeProducto(idProducto, recetaNueva);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var productoActualizado = context.Productos.Include("Receta.InsumosParaReceta").First(p => p.IdProducto == idProducto);
                    Assert.Equal("Hornear con tocino", productoActualizado.Receta.Instrucciones);
                    Assert.Single(productoActualizado.Receta.InsumosParaReceta);
                }
            }
        }

        [Fact]
        public void ObtenerRecetaPorIdProducto_DeberiaRetornarRecetaCompleta()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "g" };
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Salsas" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    var insumo = new Insumo { Nombre = "Salsa BBQ", UnidadDeMedida = unidad, IdCategoriaInsumo = categoria.IdCategoriaInsumo };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    var receta = new Receta
                    {
                        Instrucciones = "Agregar salsa al final",
                        InsumosParaReceta = new List<InsumoParaReceta>
                {
                    new InsumoParaReceta { Insumo = insumo, Cantidad = 80 }
                }
                    };
                    context.Recetas.Add(receta);
                    context.SaveChanges();

                    var categoriaProducto = new CategoriaProducto { CategoriaProductoNombre = "BBQ" };
                    context.CategoriasProducto.Add(categoriaProducto);
                    context.SaveChanges();

                    var producto = new Producto
                    {
                        Nombre = "Pizza Salsa BBQ",
                        Precio = 95,
                        IdCategoriaProducto = categoriaProducto.IdCategoriaProducto,
                        IdReceta = receta.IdReceta
                    };
                    context.Productos.Add(producto);
                    context.SaveChanges();
                    idProducto = producto.IdProducto;
                }

                var recetaObtenida = RecetaDAO.ObtenerRecetaPorIdProducto(idProducto);

                Assert.NotNull(recetaObtenida);
                Assert.NotEmpty(recetaObtenida.InsumosParaReceta);
                Assert.NotNull(recetaObtenida.InsumosParaReceta.First().Insumo);
            }
        }




    }
}
