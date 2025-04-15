using System.Linq;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class CuentaAccesoDAO
    {
        //TODO: NEEDS TESTING
        public static async Task<bool> VerificarCuenta(string nombreUsuario, string contrasenia)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return await Task.Run(() =>
                {
                    return context.CuentasAcceso
                        .Any(c => c.NombreUsuario == nombreUsuario && c.Contraseña == contrasenia);
                });
            }
        }
    }
}
