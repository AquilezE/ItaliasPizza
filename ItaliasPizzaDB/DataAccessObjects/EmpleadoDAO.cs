using System.Data.Entity;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class EmpleadoDAO
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
