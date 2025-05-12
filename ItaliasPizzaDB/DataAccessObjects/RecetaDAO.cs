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





    }

}
