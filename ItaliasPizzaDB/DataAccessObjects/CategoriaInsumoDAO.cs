using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class CategoriaInsumoDAO
    {

        public static List<CategoriaInsumo> ObtenerCategoriasInsumo()
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.CategoriasInsumo.ToList();
            }
        }

        public static CategoriaInsumo ObtenerCategoriaInsumoPorId(int id)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.CategoriasInsumo.Find(id);
            }
        }

        public static void AgregarCategoriaInsumo(CategoriaInsumo categoria)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                context.CategoriasInsumo.Add(categoria);
                context.SaveChanges();
            }
        }

        public static void EliminarCategoriaInsumo(int id)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var categoria = context.CategoriasInsumo.Find(id);
                if (categoria != null)
                {
                    context.CategoriasInsumo.Remove(categoria);
                    context.SaveChanges();
                }
            }
        }

    }
}
