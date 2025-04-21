using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class UnidadDeMedidaDAO
    {

        public static List<UnidadDeMedida> ObtenerUnidadesDeMedida()
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.UnidadesDeMedida.ToList();
            }
        }

        public static UnidadDeMedida ObtenerUnidadDeMedidaPorId(int id)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.UnidadesDeMedida.Find(id);
            }
        }

        public static void AgregarUnidadDeMedida(UnidadDeMedida unidad)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                context.UnidadesDeMedida.Add(unidad);
                context.SaveChanges();
            }
        }

        public static void EliminarUnidadDeMedida(int id)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var unidad = context.UnidadesDeMedida.Find(id);
                if (unidad != null)
                {
                    context.UnidadesDeMedida.Remove(unidad);
                    context.SaveChanges();
                }
            }
        }

    }
}
