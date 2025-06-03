using System;
using System.IO;
using System.Linq;
using System.Transactions;
using Xunit;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System.Collections.Generic;


namespace DatabaseTests
    {
        public class ProductoDAOTests
        {
            [Fact]
            public void AgregarProducto_SinReceta_DeberiaGuardarCorrectamente()
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string rutaImagenTemporal = Path.GetTempFileName();
                    File.WriteAllText(rutaImagenTemporal, "fake image content");

                    int idProducto;

                    using (var context = new ItaliasPizzaDbContext())
                    {
                        var dao = new ProductoDAO();

                        var producto = new Producto
                        {
                            Nombre = "Pizza Test",
                            Precio = 100,
                            IdCategoriaProducto = 1,
                            Descripcion = "Pizza de prueba unitaria",
                            Restricciones = "No se puede poner queso a la pisa"
                        };

                        bool resultado = dao.AgregarProducto(producto, false, null, rutaImagenTemporal);

                        Assert.True(resultado);
                        Assert.NotEqual(0, producto.IdProducto); // Se debió generar ID
                        idProducto = producto.IdProducto;
                    }

                    using (var context = new ItaliasPizzaDbContext())
                    {
                        var productoGuardado = context.Productos.FirstOrDefault(p => p.IdProducto == idProducto);
                        Assert.NotNull(productoGuardado);
                        Assert.Equal("Pizza Test", productoGuardado.Nombre);
                        Assert.False(string.IsNullOrWhiteSpace(productoGuardado.ImagenRuta));
                        Assert.True(File.Exists(Path.Combine("ImagenesProductos", productoGuardado.ImagenRuta)));
                    }

                    // Limpieza de archivo temporal
                    File.Delete(rutaImagenTemporal);
                }
            }
        [Fact]
        public void AgregarProducto_ConReceta_DeberiaGuardarCorrectamente()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                string rutaImagenTemporal = Path.GetTempFileName();
                File.WriteAllText(rutaImagenTemporal, "fake image content");

                int idProducto;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoEjemplo = context.Insumos.FirstOrDefault();
                    Assert.NotNull(insumoEjemplo); // Asegúrate de tener al menos un insumo

                    var receta = new Receta
                    {
                        Instrucciones = "Receta para pizza unitaria",
                        InsumosParaReceta = new List<InsumoParaReceta>
                {
                    new InsumoParaReceta
                    {
                        IdInsumo = insumoEjemplo.IdInsumo,
                        Cantidad = 1
                    }
                }
                    };

                    var producto = new Producto
                    {
                        Nombre = "Pizza con receta",
                        Precio = 150,
                        IdCategoriaProducto = 1,
                        Descripcion = "Pizza de prueba con receta",
                        Restricciones = "Sin cebolla"
                    };

                    var dao = new ProductoDAO();
                    bool resultado = dao.AgregarProducto(producto, true, receta, rutaImagenTemporal);


                    Assert.True(resultado);
                    Assert.NotEqual(0, producto.IdProducto);
                    idProducto = producto.IdProducto;
                }

                using (var context = new ItaliasPizzaDbContext())
                {
                    var productoGuardado = context.Productos
                        .Include("Receta.InsumosParaReceta")
                        .FirstOrDefault(p => p.IdProducto == idProducto);

                    Assert.NotNull(productoGuardado);
                    Assert.Equal("Pizza con receta", productoGuardado.Nombre);
                    Assert.NotNull(productoGuardado.Receta);
                    Assert.Single(productoGuardado.Receta.InsumosParaReceta);
                }

                File.Delete(rutaImagenTemporal);
            }
        }

        [Fact]
        public void ObtenerProductosConCategoria_DeberiaRetornarListaConCategorias()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear categoría y producto de prueba
                    var categoria = new CategoriaProducto { CategoriaProductoNombre  = "Categoría Test" };
                    var producto = new Producto
                    {
                        Nombre = "Producto con categoría",
                        Precio = 50,
                        IdCategoriaProducto = 1,
                        Descripcion = "Test",
                        Restricciones = "Ninguna"
                    };

                    context.CategoriasProducto.Add(categoria);
                    context.SaveChanges();
                    producto.IdCategoriaProducto = categoria.IdCategoriaProducto;

                    context.Productos.Add(producto);
                    context.SaveChanges();
                }

                var productos = ProductoDAO.ObtenerProductosConCategoria();

                Assert.NotEmpty(productos);
                Assert.Contains(productos, p => p.CategoriaProducto != null);
            }
        }

        [Fact]
        public void ObtenerProductoCompleto_DeberiaIncluirRecetaEInsumos()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto = 0;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // 1. Crear Unidad de Medida
                    var unidad = new UnidadDeMedida { UnidadDeMedidaNombre = "Gramos" };
                    context.UnidadesDeMedida.Add(unidad);
                    context.SaveChanges();

                    // 2. Crear Categoría de Insumo (para evitar error FK)
                    var categoria = new CategoriaInsumo { CategoriaInsumoNombre = "Lácteos" };
                    context.CategoriasInsumo.Add(categoria);
                    context.SaveChanges();

                    // 3. Crear Insumo con unidad y categoría
                    var insumo = new Insumo
                    {
                        Nombre = "Queso",
                        UnidadDeMedida = unidad,
                        IdCategoriaInsumo = categoria.IdCategoriaInsumo
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();  // Necesario para generar IdInsumo

                    // 4. Crear Receta con Insumo ya existente
                    var receta = new Receta
                    {
                        Instrucciones = "Hornear 15 min",
                        InsumosParaReceta = new List<InsumoParaReceta>
                {
                    new InsumoParaReceta
                    {
                        Insumo = insumo,
                        Cantidad = 100
                    }
                }
                    };
                    context.Recetas.Add(receta);
                    context.SaveChanges();  // Genera IdReceta

                    // 5. Crear Producto con la receta asociada
                    var producto = new Producto
                    {
                        Nombre = "Producto con receta",
                        Precio = 70,
                        IdCategoriaProducto = 1, // Asegúrate de que esta categoría ya exista en tu base
                        IdReceta = receta.IdReceta,
                        Descripcion = "Incluye insumos",
                        Restricciones = "Sin restricciones"
                    };
                    context.Productos.Add(producto);
                    context.SaveChanges();

                    idProducto = producto.IdProducto;
                }

                // 6. Verificación
                var productoCompleto = ProductoDAO.ObtenerProductoCompleto(idProducto);

                Assert.NotNull(productoCompleto);
                Assert.NotNull(productoCompleto.Receta);
                Assert.NotEmpty(productoCompleto.Receta.InsumosParaReceta);
                Assert.NotNull(productoCompleto.Receta.InsumosParaReceta.First().Insumo.UnidadDeMedida);
            }
        }

        [Fact]
        public void ActualizarProducto_DeberiaModificarDatosCorrectamente()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto;
                int idCategoria;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear categoría para asegurar existencia
                    var categoria = new CategoriaProducto { CategoriaProductoNombre = "Temporal" };
                    context.CategoriasProducto.Add(categoria);
                    context.SaveChanges();
                    idCategoria = categoria.IdCategoriaProducto;

                    var producto = new Producto
                    {
                        Nombre = "Producto original",
                        Precio = 30,
                        IdCategoriaProducto = idCategoria,
                        Descripcion = "Antes",
                        Restricciones = "Ninguna"
                    };

                    context.Productos.Add(producto);
                    context.SaveChanges();
                    idProducto = producto.IdProducto;
                }

                var productoModificado = new Producto
                {
                    IdProducto = idProducto,
                    Nombre = "Producto actualizado",
                    Precio = 99,
                    Descripcion = "Descripción nueva",
                    Restricciones = "Nueva restricción",
                    IdCategoriaProducto = idCategoria
                };

                var dao = new ProductoDAO();
                bool resultado = dao.ActualizarProducto(productoModificado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var productoActual = context.Productos.FirstOrDefault(p => p.IdProducto == idProducto);
                    Assert.Equal("Producto actualizado", productoActual.Nombre);
                    Assert.Equal("Descripción nueva", productoActual.Descripcion);
                    Assert.Equal("Nueva restricción", productoActual.Restricciones);
                }
            }
        }

        [Fact]
        public void ActualizarProducto_DeberiaQuitarRecetaCorrectamente()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto;
                int idReceta;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear receta de prueba
                    var receta = new Receta { Instrucciones = "Paso a paso original" };
                    context.Recetas.Add(receta);
                    context.SaveChanges();
                    idReceta = receta.IdReceta;

                    // Crear categoría para evitar errores de FK
                    var categoria = new CategoriaProducto { CategoriaProductoNombre = "Temporal" };
                    context.CategoriasProducto.Add(categoria);
                    context.SaveChanges();

                    // Crear producto con receta
                    var producto = new Producto
                    {
                        Nombre = "Producto con receta",
                        Precio = 50,
                        IdCategoriaProducto = categoria.IdCategoriaProducto,
                        IdReceta = idReceta,
                        Descripcion = "Con receta",
                        Restricciones = "Ninguna"
                    };

                    context.Productos.Add(producto);
                    context.SaveChanges();
                    idProducto = producto.IdProducto;
                }

                // Modificar producto para quitarle la receta
                var productoModificado = new Producto
                {
                    IdProducto = idProducto,
                    Nombre = "Producto sin receta",
                    Precio = 50,
                    IdCategoriaProducto = 1, // No cambia
                    IdReceta = null,
                    Descripcion = "Receta eliminada",
                    Restricciones = "Actualizado"
                };

                var dao = new ProductoDAO();
                bool resultado = dao.ActualizarProducto(productoModificado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var productoActual = context.Productos.First(p => p.IdProducto == idProducto);
                    Assert.Null(productoActual.IdReceta);
                }
            }
        }

        [Fact]
        public void ActualizarProducto_DeberiaAsignarRecetaCorrectamente()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idProducto;
                int idReceta;

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Crear receta nueva
                    var receta = new Receta { Instrucciones = "Nueva receta asignada" };
                    context.Recetas.Add(receta);
                    context.SaveChanges();
                    idReceta = receta.IdReceta;

                    // Crear categoría
                    var categoria = new CategoriaProducto { CategoriaProductoNombre = "Temporal 2" };
                    context.CategoriasProducto.Add(categoria);
                    context.SaveChanges();

                    // Crear producto sin receta
                    var producto = new Producto
                    {
                        Nombre = "Producto sin receta",
                        Precio = 70,
                        IdCategoriaProducto = categoria.IdCategoriaProducto,
                        IdReceta = null,
                        Descripcion = "Originalmente sin receta",
                        Restricciones = "Ninguna"
                    };

                    context.Productos.Add(producto);
                    context.SaveChanges();
                    idProducto = producto.IdProducto;
                }

                // Modificar para asignar receta
                var productoModificado = new Producto
                {
                    IdProducto = idProducto,
                    Nombre = "Producto con receta nueva",
                    Precio = 70,
                    IdCategoriaProducto = 1, // Mantiene
                    IdReceta = idReceta,
                    Descripcion = "Ahora con receta",
                    Restricciones = "Nuevas"
                };

                var dao = new ProductoDAO();
                bool resultado = dao.ActualizarProducto(productoModificado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var productoActual = context.Productos.First(p => p.IdProducto == idProducto);
                    Assert.Equal(idReceta, productoActual.IdReceta);
                }
            }
        }




    }



}


