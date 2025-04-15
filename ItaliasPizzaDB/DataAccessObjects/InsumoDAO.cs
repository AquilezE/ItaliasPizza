using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class InsumoDAO
    {

        public static List<Insumo> ObtenerInsumos()
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.Insumos.ToList();
            }
        }

        public static List<Insumo> ObtenerInsumos(int idCategoria, int idUnidadDeMedida, bool isActive)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                IQueryable<Insumo> query = context.Insumos.Where(i => i.Status == isActive);

                if (idCategoria != -1)
                {
                    query = query.Where(i => i.CategoriaInsumo.IdCategoriaInsumo == idCategoria);
                }
                if (idUnidadDeMedida != -1)
                {
                    query = query.Where(i => i.UnidadDeMedida.IdUnidadDeMedida == idUnidadDeMedida);
                }
                return query.ToList();
            }
        }
    }
}
