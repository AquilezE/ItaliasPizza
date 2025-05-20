    using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB.DataTransferObjects;

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
                var query = context.Insumos
                    .Include(i => i.CategoriaInsumo)
                    .Include(i => i.UnidadDeMedida)
                    .Where(i => i.Status == isActive);

                if (idCategoria != -1)
                    query = query.Where(i => i.IdCategoriaInsumo == idCategoria);

                if (idUnidadDeMedida != -1)
                    query = query.Where(i => i.IdUnidadDeMedida == idUnidadDeMedida);

                return query.ToList();
            }
        }

        public static bool CrearInsumo(Insumo insumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                context.Insumos.Add(insumo);
                return context.SaveChanges() > 0;

            }
        }

        public static bool ActualizarInsumo(Insumo insumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                Insumo insumoActual = context.Insumos.FirstOrDefault(p => p.IdInsumo == insumo.IdInsumo);
                if (insumoActual == null)
                {
                    return false;
                }

                insumoActual.Nombre = insumo.Nombre;
                insumoActual.Status = insumo.Status;
                insumoActual.IdCategoriaInsumo = insumo.IdCategoriaInsumo;

                return context.SaveChanges() > 0;
            }
        }

        public static int ValidarInsumoPorNombreActivo(String nombreInsumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Insumos
             .Any(p => p.Nombre == nombreInsumo && p.Status == true) ? 1 : 0;
            }
        }

        public static int ValidarInsumoPorNombreDesactivado(String nombreInsumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Insumos
                    .Any(p => p.Nombre == nombreInsumo && p.Status == false) ? 1 : 0;

            }
        }

        public static int ValidarInsumoNoRegistradoEnReceta(Insumo insumo)
        {
            int registrado = 0;

            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                bool insumoRegistrado = context.InsumosParaReceta
                    .Any(p => p.IdInsumo == insumo.IdInsumo);

                registrado = insumoRegistrado ? 1 : 0;
            }
            return registrado;
        }

        public static bool EliminarInsumo(Insumo insumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                Insumo insumoActual = context.Insumos.FirstOrDefault(p => p.IdInsumo == insumo.IdInsumo);
                if (insumoActual == null)
                {
                    return false;
                }
                insumoActual.Status = false;

                return context.SaveChanges() > 0;
            }
        }

        public static bool ActivarInsumo(string nombreInsumo)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                var insumoActual = context.Insumos
                    .FirstOrDefault(p => p.Nombre == nombreInsumo && p.Status == false);

                if (insumoActual == null)
                {
                    return false;
                }

                insumoActual.Status = true;

                return context.SaveChanges() > 0;
            }
        }

        public static List<InsumoDTO> ObtenerInventarioReporte()
        {
            using (var ctx = new ItaliasPizzaDbContext())
            {
                return ctx.Database
                    .SqlQuery<InsumoDTO>("EXEC dbo.sp02_GetInventarioReport")
                    .ToList();
            }
        }

    }
}
