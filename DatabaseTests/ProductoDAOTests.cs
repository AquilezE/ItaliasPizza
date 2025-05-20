 using System;
    using System.IO;
    using System.Linq;
    using System.Transactions;
    using Xunit;
    using ItaliasPizzaDB;
    using ItaliasPizzaDB.DataAccessObjects;
    using ItaliasPizzaDB.Models;
    using ItaliasPizzaDB.DataAccessObjects.ItaliasPizzaDB.DataAccessObjects;
using System.Collections.Generic;

    namespace DatabaseTests
    {
        public class ProductoDAOTests
        {
            /*[Fact]
            public void AgregarProducto_SinReceta_ConImagen_DeberiaGuardarCorrectamente()
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
        }*/
    }



}


