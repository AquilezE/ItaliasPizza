using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    namespace ItaliasPizzaDB.DataAccessObjects
    {

        public class ProductoDAO
        {


            public bool AgregarProducto(Producto producto, bool tieneReceta, Receta receta, string imagenRutaLocal)
            {

                using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
                {
                    try
                    {
                        // 1. Guardar receta si se requiere
                        if (tieneReceta && receta != null)
                        {
                            context.Recetas.Add(receta);
                            context.SaveChanges();
                            producto.IdReceta = receta.IdReceta;
                        }

                        // 2. Copiar imagen y guardar nombre
                        if (!string.IsNullOrWhiteSpace(imagenRutaLocal) && File.Exists(imagenRutaLocal))
                        {
                            string nombreImagen = $"{Guid.NewGuid()}_{Path.GetFileName(imagenRutaLocal)}";
                            string carpetaDestino = "ImagenesProductos";
                            Directory.CreateDirectory(carpetaDestino);

                            string rutaDestino = Path.Combine(carpetaDestino, nombreImagen);
                            File.Copy(imagenRutaLocal, rutaDestino, overwrite: true);

                            producto.ImagenRuta = nombreImagen;
                        }

                        // 3. Guardar producto
                        context.Productos.Add(producto);
                        context.SaveChanges();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al guardar producto: " + ex.Message);
                        return false;
                    }
                }
            }

            public static List<Producto> ObtenerProductosConCategoria()
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    return context.Productos
                        .Include("CategoriaProducto")
                        .ToList();
                }
            }

            public static Producto ObtenerProductoCompleto(int idProducto)
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    return context.Productos
                        .Include("Receta")
                        .Include("Receta.InsumosParaReceta")
                        .Include("Receta.InsumosParaReceta.Insumo")
                        .Include("Receta.InsumosParaReceta.Insumo.UnidadDeMedida")
                        .FirstOrDefault(p => p.IdProducto == idProducto);
                }
            }

            public bool ActualizarProducto(Producto productoModificado, string nuevaRutaImagen = null)
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    try
                    {
                        // 1. Recuperar el producto original del contexto
                        var productoOriginal = context.Productos.FirstOrDefault(p => p.IdProducto == productoModificado.IdProducto);
                        if (productoOriginal == null)
                            return false;

                        // 2. Actualizar campos
                        productoOriginal.Nombre = productoModificado.Nombre;
                        productoOriginal.Codigo = productoModificado.Codigo;
                        productoOriginal.Precio = productoModificado.Precio;
                        productoOriginal.Restricciones = productoModificado.Restricciones;
                        productoOriginal.Descripcion = productoModificado.Descripcion;
                        productoOriginal.IdCategoriaProducto = productoModificado.IdCategoriaProducto;
                        productoOriginal.IdReceta = productoModificado.IdReceta;

                        // 3. Imagen nueva (si aplica)
                        if (!string.IsNullOrWhiteSpace(nuevaRutaImagen) && File.Exists(nuevaRutaImagen))
                        {
                            string nombreImagen = $"{Guid.NewGuid()}_{Path.GetFileName(nuevaRutaImagen)}";
                            string carpetaDestino = "ImagenesProductos";
                            Directory.CreateDirectory(carpetaDestino);
                            string rutaDestino = Path.Combine(carpetaDestino, nombreImagen);
                            File.Copy(nuevaRutaImagen, rutaDestino, true);

                            productoOriginal.ImagenRuta = nombreImagen;
                        }

                        // 4. Guardar cambios
                        context.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al actualizar producto: " + ex.Message);
                        return false;
                    }
                }
            }


        }


    }

}
