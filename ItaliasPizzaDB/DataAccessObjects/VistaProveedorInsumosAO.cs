using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class VistaInsumosPorProveedorDAO
    {
        public static List<VistaProveedorInsumos> ObtenerInsumosDeProveedor(int idProveedor)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.VistaInsumosPorProveedor
                    .Where(i => i.IdProveedor == idProveedor)
                    .ToList();
            }
        }
    }
}
