using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.Repositorios
{
    public class CuentaAccesoRepositorio
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
