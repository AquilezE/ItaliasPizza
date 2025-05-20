using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.DataTransferObjects;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class PedidoProveedorDAO
    {
        public static List<PedidoProveedorDTO> ObtenerPedidosProveedorReporte(DateTime? desde, DateTime? hasta)
        {
            using (var ctx = new ItaliasPizzaDbContext())
            {
                var pDesde = new SqlParameter("@Desde", desde);
                var pHasta = new SqlParameter("@Hasta", hasta);

                return ctx.Database
                    .SqlQuery<PedidoProveedorDTO>(
                        "EXEC dbo.sp03_GetPedidosProveedorReport @Desde, @Hasta",
                        pDesde, pHasta)
                    .ToList();
            }
        }
    }




}
