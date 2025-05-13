using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ItaliasPizzaDB.DataAccessObjects
{
    public class RecetaDAO
    {
        public static void GuardarReceta(Receta nuevaReceta)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                // Agrega la receta al contexto
                context.Recetas.Add(nuevaReceta);

                // Guarda todos los cambios (esto incluye los InsumoParaReceta)
                context.SaveChanges();
            }
        }

        public static void EliminarReceta(int idReceta)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var receta = context.Recetas
                    .Include("InsumosParaReceta")
                    .FirstOrDefault(r => r.IdReceta == idReceta);

                if (receta != null)
                {
                    // Eliminar insumos relacionados primero
                    context.InsumosParaReceta.RemoveRange(receta.InsumosParaReceta);
                    context.Recetas.Remove(receta);
                    context.SaveChanges();
                }
            }
        }

        public static void ActualizarRecetaDeProducto(int idProducto, Receta recetaNueva)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var producto = context.Productos
                    .Include("Receta.InsumosParaReceta")
                    .FirstOrDefault(p => p.IdProducto == idProducto);

                if (producto?.Receta == null) return;

                // Elimina los insumos anteriores
                context.InsumosParaReceta.RemoveRange(producto.Receta.InsumosParaReceta);

                // Asigna los nuevos
                foreach (var insumo in recetaNueva.InsumosParaReceta)
                {
                    insumo.IdReceta = producto.IdReceta.Value;
                    context.InsumosParaReceta.Add(insumo);
                }

                // Actualiza instrucciones
                producto.Receta.Instrucciones = recetaNueva.Instrucciones;

                context.SaveChanges();
            }
        }
        public static Receta ObtenerRecetaPorIdProducto(int idProducto)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var producto = context.Productos
                    .Include("Receta.InsumosParaReceta.Insumo") // EF6 usa strings para navegación profunda
                    .FirstOrDefault(p => p.IdProducto == idProducto);

                return producto?.Receta;
            }
        }

    }

}
