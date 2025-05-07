using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
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
        }


    }

}
