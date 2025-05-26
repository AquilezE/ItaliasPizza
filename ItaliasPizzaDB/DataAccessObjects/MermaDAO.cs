using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class MermaVisualPorDia
    {
        public DateTime Fecha { get; set; }
        public float TotalPerdido { get; set; }
    }

    public class MermaDAO
    {
        public static List<MermaVisualPorDia> ObtenerResumenDeMermas()
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var resumen = context.Mermas
                    .Include(m => m.Insumo)
                    .GroupBy(m => DbFunctions.TruncateTime(m.Fecha))
                    .Select(g => new MermaVisualPorDia
                    {
                        Fecha = g.Key.Value,
                        TotalPerdido = g.Sum(m => m.Cantidad * m.Insumo.Precio)
                    })
                    .OrderByDescending(r => r.Fecha)
                    .ToList();

                return resumen;
            }
        }

        public static List<MermaDetalleDTO> ObtenerDetallePorFecha(DateTime fecha)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var detalles = context.Mermas
                    .Where(m => DbFunctions.TruncateTime(m.Fecha) == fecha.Date)
                    .Include(m => m.Insumo)
                    .Select(m => new MermaDetalleDTO
                    {
                        NombreInsumo = m.Insumo.Nombre,
                        Cantidad = m.Cantidad, 
                        PrecioUnitario = m.Insumo.Precio,
                        Unidad = m.Insumo.UnidadDeMedida.UnidadDeMedidaNombre
                    })
                    .ToList();

                return detalles;
            }
        }

        public static bool RegistrarMerma(int idInsumo, float cantidad)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var insumo = context.Insumos.FirstOrDefault(i => i.IdInsumo == idInsumo);
                if (insumo == null)
                    throw new Exception("Insumo no encontrado");

                var nuevaMerma = new Merma
                {
                    IdInsumo = idInsumo,
                    Cantidad = cantidad,
                    Fecha = DateTime.Now
                };

                context.Mermas.Add(nuevaMerma);
                context.SaveChanges();

                return true;
            }
        }
    }

}
