using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class CategoriaProductoDAO
    {
        public static List<CategoriaProducto> ObtenerCategoriasProducto()
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.CategoriasProducto.ToList();
            }
        }
    }
}
