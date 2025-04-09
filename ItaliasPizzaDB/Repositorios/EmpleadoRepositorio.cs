using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.Repositorios
{
    public class EmpleadoRepositorio
    {

        //TODO: NEEDS TESTING
        public static async Task<Empleado> ObtenerEmpleadoPorCuentaAcceso(string nombreUsuario)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return await Task.Run(() =>
                {
               
                        return context.Empleados
                            .Include(e => e.CuentaAcceso)  
                            .Include(e => e.Cargo)         
                            .FirstOrDefaultAsync(e => e.CuentaAcceso.NombreUsuario == nombreUsuario);
                
                });
            }
        }

    }
}
