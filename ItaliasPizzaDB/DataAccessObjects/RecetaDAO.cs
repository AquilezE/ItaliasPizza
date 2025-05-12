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

        public static Receta ObtenerRecetaPorIdProducto(int idProducto)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var producto = context.Productos
                    .Include("Receta")
                    .Include("Receta.InsumosParaReceta")
                    .Include("Receta.InsumosParaReceta.Insumo")
                    .FirstOrDefault(p => p.IdProducto == idProducto);

                return producto?.Receta;
            }
        }
    }

}
