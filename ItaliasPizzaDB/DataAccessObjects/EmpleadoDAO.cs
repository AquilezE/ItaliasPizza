using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB;
using System.Collections.Generic;

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
            using (var context = new ItaliasPizzaDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var empleadoExistente = context.Empleados
                    .FirstOrDefault(e => e.IdEmpleado == empleadoEditado.IdEmpleado);

                        if (empleadoExistente == null)
                            return false;

                        // Luego cargar manualmente la cuenta de acceso si existe
                        empleadoExistente.CuentaAcceso = context.CuentasAcceso
                            .FirstOrDefault(c => c.IdEmpleado == empleadoExistente.IdEmpleado);

                        if (context.Empleados.Any(e => e.Telefono == empleadoEditado.Telefono &&
                                                    e.IdEmpleado != empleadoEditado.IdEmpleado))
                        {
                            Console.WriteLine("Teléfono ya existe en otro empleado");
                            return false;
                        }

                        // Actualizar propiedades del empleado
                        empleadoExistente.Nombre = empleadoEditado.Nombre;
                        empleadoExistente.Apellidos = empleadoEditado.Apellidos;
                        empleadoExistente.Telefono = empleadoEditado.Telefono;
                        empleadoExistente.IdCargo = empleadoEditado.IdCargo;
                        empleadoExistente.Status = empleadoEditado.Status;

                        if (cuentaEditada != null)
                        {
                            if (context.CuentasAcceso.Any(c => c.NombreUsuario == cuentaEditada.NombreUsuario &&
                                                             c.IdEmpleado != empleadoEditado.IdEmpleado))
                            {
                                Console.WriteLine("Nombre de usuario ya existe en otra cuenta");
                                return false;
                            }

                            if (empleadoExistente.CuentaAcceso == null)
                            {
                                empleadoExistente.CuentaAcceso = new CuentaAcceso
                                {
                                    IdEmpleado = empleadoEditado.IdEmpleado,
                                    NombreUsuario = cuentaEditada.NombreUsuario,
                                    Contraseña = cuentaEditada.Contraseña
                                };
                            }
                            else
                            {
                                empleadoExistente.CuentaAcceso.NombreUsuario = cuentaEditada.NombreUsuario;
                                empleadoExistente.CuentaAcceso.Contraseña = cuentaEditada.Contraseña;
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error al modificar empleado: {ex}");
                        return false;
                    }
                }
            }
        }

        public static Empleado ObtenerEmpleadoPorUsuario(string nombreUsuario)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Empleados
                    .Include(e => e.Cargo)
                    .Include(e => e.CuentaAcceso)
                    .FirstOrDefault(e => e.CuentaAcceso.NombreUsuario == nombreUsuario);
            }
        }

        public static List<Cargo> ConsultarCargos()
        {
            
                using (var context = new ItaliasPizzaDbContext())
                {
                    return context.Cargos.ToList();
                }
            
        }
    }
}
