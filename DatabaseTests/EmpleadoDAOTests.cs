using System;
using System.Transactions;
using Xunit;
using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB.DataAccessObjects;
using System.Linq;

namespace DatabaseTests
{
    public class EmpleadoDAOTests
    {
        [Fact]
        public void RegistrarEmpleadoConCuenta_DebeRegistrarEmpleadoYCuenta()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleadoExistente = context.Empleados.FirstOrDefault(e => e.Telefono == "1234567890");
                    if (empleadoExistente != null)
                    {
                        var cuentaExistente = context.CuentasAcceso.FirstOrDefault(c => c.IdEmpleado == empleadoExistente.IdEmpleado);
                        if (cuentaExistente != null)
                        {
                            context.CuentasAcceso.Remove(cuentaExistente);
                            context.SaveChanges();
                        }

                        context.Empleados.Remove(empleadoExistente);
                        context.SaveChanges();
                    }
                }

                var empleado = new Empleado
                {
                    Nombre = "Luis",
                    Apellidos = "Perez Lopez",
                    Telefono = "1234567890",
                    IdCargo = 1
                };

                var cuenta = new CuentaAcceso
                {
                    NombreUsuario = "luisperez",
                    Contraseña = "password123"
                };

                var resultado = EmpleadoDAO.RegistrarEmpleadoConCuenta(empleado, cuenta);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleadoRegistrado = context.Empleados.Find(empleado.IdEmpleado);
                    var cuentaRegistrada = context.CuentasAcceso.Find(empleado.IdEmpleado);

                    Assert.NotNull(empleadoRegistrado);
                    Assert.NotNull(cuentaRegistrada);
                    Assert.Equal(empleado.IdEmpleado, cuentaRegistrada.IdEmpleado);
                    Assert.Equal("luisperez", cuentaRegistrada.NombreUsuario);
                    Assert.True(empleadoRegistrado.Status);
                }
            }
        }


        [Fact]
        public void RegistrarEmpleadoConCuenta_NoDebeRegistrarSiTelefonoYaExiste()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleadoExistente = new Empleado
                    {
                        Nombre = "Carlos",
                        Apellidos = "Ramirez Gomez",
                        Telefono = "9999999999",
                        IdCargo = 1
                    };

                    context.Empleados.Add(empleadoExistente);
                    context.SaveChanges();
                }

                var nuevoEmpleado = new Empleado
                {
                    Nombre = "Mario",
                    Apellidos = "Lopez Sanchez",
                    Telefono = "9999999999", 
                    IdCargo = 1
                };

                var nuevaCuenta = new CuentaAcceso
                {
                    NombreUsuario = "mariolopez",
                    Contraseña = "otroPassword"
                };

                var resultado = EmpleadoDAO.RegistrarEmpleadoConCuenta(nuevoEmpleado, nuevaCuenta);

                Assert.False(resultado);
            }
        }

        [Fact]
        public void RegistrarEmpleadoConCuenta_NoDebeRegistrarSiUsuarioYaExiste()
        {
            using (var scope = new TransactionScope())
            {
                int idEmpleadoExistente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleadoExistente = new Empleado
                    {
                        Nombre = "Ana",
                        Apellidos = "Hernandez Vega",
                        Telefono = "8888888888",
                        IdCargo = 1
                    };

                    context.Empleados.Add(empleadoExistente);
                    context.SaveChanges();

                    idEmpleadoExistente = empleadoExistente.IdEmpleado;

                    var cuentaExistente = new CuentaAcceso
                    {
                        IdEmpleado = idEmpleadoExistente,
                        NombreUsuario = "anaH",
                        Contraseña = "claveAna"
                    };

                    context.CuentasAcceso.Add(cuentaExistente);
                    context.SaveChanges();
                }

                var nuevoEmpleado = new Empleado
                {
                    Nombre = "Luisa",
                    Apellidos = "Morales Diaz",
                    Telefono = "7777777777",
                    IdCargo = 1
                };

                var nuevaCuenta = new CuentaAcceso
                {
                    NombreUsuario = "anaH",
                    Contraseña = "contraseñaNueva"
                };

                var resultado = EmpleadoDAO.RegistrarEmpleadoConCuenta(nuevoEmpleado, nuevaCuenta);

                Assert.False(resultado);
            }
        }


    }
}