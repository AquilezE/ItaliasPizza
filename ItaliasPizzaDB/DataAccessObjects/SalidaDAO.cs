using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class SalidaDAO
    {

        public static async Task<bool> RegistrarTransaccion(float cantidad, string descripcion, int idEmpleado, int idTipoTransaccion)
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
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar la transacción: " + ex.Message);

                    Exception inner = ex.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine("Inner exception: " + inner.Message);
                        inner = inner.InnerException;
                    }

                    return false;
                }
            }
        }

        public static int RegistrarGastoEnCorteDeCaja(float montoGasto)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                try
                {
                    if (montoGasto <= 0)
                        return 2;

                    var corteMasReciente = context.CortesDeCaja
                        .OrderByDescending(c => c.FechaApertura)
                        .FirstOrDefault();

                    if (corteMasReciente == null)
                        return 1; 

                    corteMasReciente.Gasto += montoGasto;

                    context.SaveChanges();

                    return 0;
                }
                catch (Exception ex)
                {
                    return 3; 
                }
            }
        }
    }
}
