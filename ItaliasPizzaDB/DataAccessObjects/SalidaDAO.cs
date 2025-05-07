using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    internal class SalidaDAO
    {

        public static async Task<bool> RegistrarTransaccion(float cantidad, string descripcion, int idTipoTransaccion, int idEmpleado)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                try
                {
                    
                    var nuevaTransaccion = new Transaccion
                    {
                        Fecha = DateTime.Now, 
                        Cantidad = cantidad,
                        Descripcion = descripcion,
                        IdTipoTransaccion = idTipoTransaccion,
                        IdEmpleado = idEmpleado
                    };


                    
                    context.Transacciones.Add(nuevaTransaccion);

                    
                    await context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
