using System;
using System.Data.Entity;
using System.Linq;
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

        public static bool RegistrarEmpleadoConCuenta(Empleado nuevoEmpleado, CuentaAcceso nuevaCuenta)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    bool registroExitoso = false;
                    try
                    {

                        if (context.Empleados.Any(e => e.Telefono == nuevoEmpleado.Telefono))
                            return registroExitoso;


                        if (context.CuentasAcceso.Any(c => c.NombreUsuario == nuevaCuenta.NombreUsuario))
                            return registroExitoso;


                        nuevoEmpleado.Status = true;
                        context.Empleados.Add(nuevoEmpleado);
                        context.SaveChanges();


                        nuevaCuenta.IdEmpleado = nuevoEmpleado.IdEmpleado;
                        context.CuentasAcceso.Add(nuevaCuenta);
                        context.SaveChanges();

                        transaction.Commit();

                        registroExitoso = true;

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }

                    return registroExitoso;
                }
            }
        }

        public static bool ModificarEmpleado(Empleado empleadoEditado, CuentaAcceso cuentaEditada)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                bool modificacionExitoso = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var empleadoExistente = context.Empleados
                        .Include(e => e.CuentaAcceso)
                        .FirstOrDefault(e => e.IdEmpleado == empleadoEditado.IdEmpleado);
                        if (context.Empleados.Any(e => e.Telefono == empleadoEditado.Telefono))
                        {
                            return modificacionExitoso; 
                        }

                        if (cuentaEditada != null &&
                            context.CuentasAcceso.Any(c => c.NombreUsuario == cuentaEditada.NombreUsuario))
                        {
                            return modificacionExitoso; 
                        }

                        if (cuentaEditada != null && empleadoExistente.CuentaAcceso != null)
                        {
                            context.Entry(empleadoExistente.CuentaAcceso).CurrentValues.SetValues(cuentaEditada);
                        }

                        context.Entry(empleadoExistente).CurrentValues.SetValues(empleadoEditado);



                        context.SaveChanges();
                        transaction.Commit();
                        modificacionExitoso = true; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                    }

                    return modificacionExitoso;
                }
            }
        }

    }
}
