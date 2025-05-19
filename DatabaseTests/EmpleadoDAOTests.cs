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

        [Fact]
        public void ModificarEmpleado_DebeModificarEmpleadoYCuenta()
        {
            var telefonoOriginal = Guid.NewGuid().ToString().Substring(0, 10);
            var usuarioOriginal = Guid.NewGuid().ToString().Substring(0, 10);

            var empleadoOriginal = new Empleado
            {
                Nombre = "Juan",
                Apellidos = "Pérez",
                Telefono = telefonoOriginal,
                IdCargo = 1,
                Status = true
            };

            var cuentaOriginal = new CuentaAcceso
            {
                NombreUsuario = usuarioOriginal,
                Contraseña = "password123"
            };

            using (var context = new ItaliasPizzaDbContext())
            {
                context.Empleados.Add(empleadoOriginal);
                empleadoOriginal.CuentaAcceso = cuentaOriginal;
                context.SaveChanges();
            }

            var telefonoEditado = Guid.NewGuid().ToString().Substring(0, 10);
            var usuarioEditado = Guid.NewGuid().ToString().Substring(0, 10);

            var empleadoEditado = new Empleado
            {
                IdEmpleado = empleadoOriginal.IdEmpleado,
                Nombre = "Juan Modificado",
                Apellidos = "Pérez Actualizado",
                Telefono = telefonoEditado,
                IdCargo = 2,
                Status = true
            };

            var cuentaEditada = new CuentaAcceso
            {
                NombreUsuario = usuarioEditado,
                Contraseña = "nuevapassword"
            };

            var resultado = EmpleadoDAO.ModificarEmpleado(empleadoEditado, cuentaEditada);

            Assert.True(resultado, "El método ModificarEmpleado devolvió false cuando se esperaba true");

            using (var context = new ItaliasPizzaDbContext())
            {
                var empleadoActualizado = context.Empleados
                .First(e => e.IdEmpleado == empleadoOriginal.IdEmpleado);

                empleadoActualizado.CuentaAcceso = context.CuentasAcceso
                    .FirstOrDefault(c => c.IdEmpleado == empleadoActualizado.IdEmpleado);

                Assert.Equal("Juan Modificado", empleadoActualizado.Nombre);
                Assert.Equal("Pérez Actualizado", empleadoActualizado.Apellidos);
                Assert.Equal(telefonoEditado, empleadoActualizado.Telefono);
                Assert.Equal(2, empleadoActualizado.IdCargo);
                Assert.True(empleadoActualizado.Status);

                Assert.NotNull(empleadoActualizado.CuentaAcceso);
                Assert.Equal(usuarioEditado, empleadoActualizado.CuentaAcceso.NombreUsuario);
                Assert.Equal("nuevapassword", empleadoActualizado.CuentaAcceso.Contraseña);
            }
        }

        [Fact]
        public void ModificarEmpleado_NoDebeModificarSiTelefonoYaExiste()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleado1 = new Empleado
                    {
                        Nombre = "Empleado Uno",
                        Apellidos = "Apellido Uno",
                        Telefono = "1111111111",
                        IdCargo = 1
                    };
                    var empleado2 = new Empleado
                    {
                        Nombre = "Empleado Dos",
                        Apellidos = "Apellido Dos",
                        Telefono = "2222222222",
                        IdCargo = 1
                    };
                    context.Empleados.Add(empleado1);
                    context.Empleados.Add(empleado2);
                    context.SaveChanges();
                }

                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleado2 = context.Empleados.First(e => e.Telefono == "2222222222");

                    var empleadoEditado = new Empleado
                    {
                        IdEmpleado = empleado2.IdEmpleado,
                        Nombre = "Modificado",
                        Apellidos = "Modificado",
                        Telefono = "1111111111", 
                        IdCargo = 1
                    };

                    var resultado = EmpleadoDAO.ModificarEmpleado(empleadoEditado, null);

                    Assert.False(resultado);
                }
            }
        }

        [Fact]
        public void ModificarEmpleado_NoDebeModificarSiUsuarioYaExiste()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var empleado1 = new Empleado
                    {
                        Nombre = "Empleado Uno",
                        Apellidos = "Apellido Uno",
                        Telefono = "3333333333",
                        IdCargo = 1
                    };
                    context.Empleados.Add(empleado1);
                    context.SaveChanges();

                    var cuenta1 = new CuentaAcceso
                    {
                        IdEmpleado = empleado1.IdEmpleado,
                        NombreUsuario = "usuarioExistente",
                        Contraseña = "clave"
                    };
                    context.CuentasAcceso.Add(cuenta1);
                    context.SaveChanges();

                    var empleado2 = new Empleado
                    {
                        Nombre = "Empleado Dos",
                        Apellidos = "Apellido Dos",
                        Telefono = "4444444444",
                        IdCargo = 1
                    };
                    context.Empleados.Add(empleado2);
                    context.SaveChanges();

                    var cuenta2 = new CuentaAcceso
                    {
                        IdEmpleado = empleado2.IdEmpleado,
                        NombreUsuario = "otroUsuario",
                        Contraseña = "clave2"
                    };
                    context.CuentasAcceso.Add(cuenta2);
                    context.SaveChanges();

                    var cuentaEditada = new CuentaAcceso
                    {
                        NombreUsuario = "usuarioExistente", 
                        Contraseña = "nuevoIntento"
                    };

                    var empleadoEditado = new Empleado
                    {
                        IdEmpleado = empleado2.IdEmpleado,
                        Nombre = "Modificado",
                        Apellidos = "Modificado",
                        Telefono = "4444444444",
                        IdCargo = 1
                    };

                    var resultado = EmpleadoDAO.ModificarEmpleado(empleadoEditado, cuentaEditada);
                    Assert.False(resultado);
                }
            }
        }

        [Fact]
        public void ObtenerEmpleadoPorUsuario_DebeRetornarEmpleadoCuandoExiste()
        {
            using (var scope = new TransactionScope())
            {
                string nombreUsuario = "testuser";
                int idEmpleado;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo = new Cargo { NombreCargo = "Mesero" };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();

                    var empleado = new Empleado
                    {
                        Nombre = "Carlos",
                        Apellidos = "Sanchez",
                        Telefono = "5551234568",
                        Status = true,
                        IdCargo = cargo.IdCargo
                    };
                    context.Empleados.Add(empleado);
                    context.SaveChanges();

                    var cuentaAcceso = new CuentaAcceso
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        NombreUsuario = nombreUsuario,
                        Contraseña = "password123"
                    };
                    context.CuentasAcceso.Add(cuentaAcceso);
                    context.SaveChanges();

                    idEmpleado = empleado.IdEmpleado;
                }

                var empleadoObtenido = EmpleadoDAO.ObtenerEmpleadoPorUsuario(nombreUsuario);

                Assert.NotNull(empleadoObtenido);
                Assert.Equal(idEmpleado, empleadoObtenido.IdEmpleado);
                Assert.Equal("Carlos", empleadoObtenido.Nombre);
                Assert.Equal("Sanchez", empleadoObtenido.Apellidos);
                Assert.Equal("5551234568", empleadoObtenido.Telefono);
                Assert.True(empleadoObtenido.Status);
                Assert.NotNull(empleadoObtenido.Cargo);
                Assert.Equal("Mesero", empleadoObtenido.Cargo.NombreCargo);
                Assert.NotNull(empleadoObtenido.CuentaAcceso);
                Assert.Equal(nombreUsuario, empleadoObtenido.CuentaAcceso.NombreUsuario);
            }
        }

        [Fact]
        public void ObtenerEmpleadoPorUsuario_DebeRetornarNullCuandoNoExiste()
        {
            using (var scope = new TransactionScope())
            {

                var empleadoObtenido = EmpleadoDAO.ObtenerEmpleadoPorUsuario("usuarioinexistente");

                Assert.Null(empleadoObtenido);
            }
        }

        [Fact]
        public void ObtenerEmpleadoPorUsuario_DebeCargarRelacionesCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                string nombreUsuario = "usuarioRelaciones";

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo = new Cargo { NombreCargo = "Cocinero" };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();

                    var empleado = new Empleado
                    {
                        Nombre = "María",
                        Apellidos = "Gómez",
                        Telefono = "5557654321",
                        Status = true,
                        IdCargo = cargo.IdCargo
                    };
                    context.Empleados.Add(empleado);
                    context.SaveChanges();

                    var cuentaAcceso = new CuentaAcceso
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        NombreUsuario = nombreUsuario,
                        Contraseña = "securepass"
                    };
                    context.CuentasAcceso.Add(cuentaAcceso);
                    context.SaveChanges();
                }

                var empleadoObtenido = EmpleadoDAO.ObtenerEmpleadoPorUsuario(nombreUsuario);

                Assert.NotNull(empleadoObtenido.Cargo);
                Assert.Equal("Cocinero", empleadoObtenido.Cargo.NombreCargo);
                Assert.NotNull(empleadoObtenido.CuentaAcceso);
                Assert.Equal(nombreUsuario, empleadoObtenido.CuentaAcceso.NombreUsuario);
                Assert.Equal("securepass", empleadoObtenido.CuentaAcceso.Contraseña);
            }
        }


        [Fact]
        public void ConsultarCargos_DebeRetornarTodosLosCargosExistentes()
        {
            using (var scope = new TransactionScope())
            {
                int idCargo1, idCargo2;
                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo1 = new Cargo { NombreCargo = "Cajero" };
                    var cargo2 = new Cargo { NombreCargo = "Cocinero" };

                    context.Cargos.Add(cargo1);
                    context.Cargos.Add(cargo2);
                    context.SaveChanges();

                    idCargo1 = cargo1.IdCargo;
                    idCargo2 = cargo2.IdCargo;
                }

                var cargosObtenidos = EmpleadoDAO.ConsultarCargos();

                var cargoCajero = cargosObtenidos.FirstOrDefault(c => c.IdCargo == idCargo1);
                var cargoCocinero = cargosObtenidos.FirstOrDefault(c => c.IdCargo == idCargo2);

                Assert.NotNull(cargoCajero);
                Assert.Equal("Cajero", cargoCajero.NombreCargo);
                Assert.NotNull(cargoCocinero);
                Assert.Equal("Cocinero", cargoCocinero.NombreCargo);
            }
        }

        [Fact]
        public void ConsultarCargos_DebeRetornarCargosConSusPropiedadesCorrectas()
        {
            using (var scope = new TransactionScope())
            {
                int idCargo;
                string nombreEsperado = "Gerente";

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo = new Cargo { NombreCargo = nombreEsperado };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();
                    idCargo = cargo.IdCargo;
                }

                var cargosObtenidos = EmpleadoDAO.ConsultarCargos();
                var cargoObtenido = cargosObtenidos.First(c => c.IdCargo == idCargo);

                Assert.Equal(idCargo, cargoObtenido.IdCargo);
                Assert.Equal(nombreEsperado, cargoObtenido.NombreCargo);
            }
        }

        [Fact]
        public void ConsultarCargos_DebeManejarNombresDeCargoNulos()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var cargo = new Cargo { NombreCargo = null };
                    context.Cargos.Add(cargo);
                    context.SaveChanges();
                }

                var cargosObtenidos = EmpleadoDAO.ConsultarCargos();
                var cargoConNombreNulo = cargosObtenidos.First(c => c.NombreCargo == null);

                Assert.NotNull(cargoConNombreNulo);
                Assert.Null(cargoConNombreNulo.NombreCargo);
            }
        }
    }
}