using ItaliasPizzaDB.Models;
using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using ItaliasPizzaDB.DataAccessObjects;
using System.Runtime.Remoting.Contexts;
using System.Runtime.CompilerServices;

namespace DatabaseTests
{
    public class ClienteDAOTests
    {
        [Fact]
        public void ObtenerClientePorId_DebeRetornarClienteCorrecto()
        {
            using (var scope = new TransactionScope())
            {
                int idCliente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cliente = new Cliente
                    {
                        Nombre = "Juan",
                        Apellidos = "Pérez",
                        Telefono = "1234567890",
                        Status = true
                    };

                    context.Clientes.Add(cliente);
                    context.SaveChanges();

                    idCliente = cliente.IdCliente;
                }

                var clienteObtenido = ClienteDAO.ObtenerClientePorId(idCliente);

                Assert.NotNull(clienteObtenido);
                Assert.Equal("Juan", clienteObtenido.Nombre);
                Assert.Equal("Pérez", clienteObtenido.Apellidos);
                Assert.Equal("1234567890", clienteObtenido.Telefono);
                Assert.True(clienteObtenido.Status);
            }
        }

        [Fact]
        public void CrearCliente_DebeAgregarClienteCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                var nuevoCliente = new Cliente
                {
                    Nombre = "Laura",
                    Apellidos = "González",
                    Telefono = "5551234567",
                    Status = true
                };

                Cliente clienteCreado = ClienteDAO.CrearCliente(nuevoCliente);

                Assert.NotNull(clienteCreado);
                Assert.True(clienteCreado.IdCliente > 0);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var clienteEnBD = context.Clientes
                        .FirstOrDefault(c => c.IdCliente == clienteCreado.IdCliente);

                    Assert.NotNull(clienteEnBD);
                    Assert.Equal("Laura", clienteEnBD.Nombre);
                    Assert.Equal("González", clienteEnBD.Apellidos);
                    Assert.Equal("5551234567", clienteEnBD.Telefono);
                    Assert.True(clienteEnBD.Status);
                }
            }
        }

        [Fact]
        public void AgregarDireccion_DebeAgregarDireccionCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idCliente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cliente = new Cliente
                    {
                        Nombre = "Juan",
                        Apellidos = "Pérez",
                        Telefono = "1234567890",
                        Status = true
                    };

                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;
                }

                var nuevaDireccion = new Direccion
                {
                    IdCliente = idCliente,
                    Calle = "Av. Reforma",
                    Numero = 123,
                    CodigoPostal = "12345",
                    Colonia = "Centro",
                    Ciudad = "Ciudad de México",
                    Estado = "CDMX",
                    Status = true,
                    Referencia = "Cerca del parque"
                };

                bool resultado = ClienteDAO.AgregarDireccion(nuevaDireccion);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var direccionEnBD = context.Direcciones
                        .FirstOrDefault(d => d.Calle == "Av. Reforma" && d.Numero == 123 && d.Colonia == "Centro");

                    Assert.NotNull(direccionEnBD);
                    Assert.Equal("12345", direccionEnBD.CodigoPostal);
                    Assert.Equal("Ciudad de México", direccionEnBD.Ciudad);
                    Assert.Equal("CDMX", direccionEnBD.Estado);
                    Assert.True(direccionEnBD.Status);
                    Assert.Equal("Cerca del parque", direccionEnBD.Referencia);
                }
            }
        }

        [Fact]
        public void ValidarClientePorTelefono_TelefonoExistente_DebeRetornarUno()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "Carlos",
                    Apellidos = "Ramírez",
                    Telefono = "5566778899",
                    Status = true
                };

                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                }
                int resultado = ClienteDAO.ValidarClientePorTelefono("5566778899");

                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void ValidarClientePorTelefono_TelefonoInexistente_DebeRetornarCero()
        {
            using (var scope = new TransactionScope())
            {
                int resultado = ClienteDAO.ValidarClientePorTelefono("0000000000");

                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ActualizarCliente_DebeActualizarClienteCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                var clienteOriginal = new Cliente
                {
                    Nombre = "Ana",
                    Apellidos = "López",
                    Telefono = "5511122233",
                    Status = true
                };

                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(clienteOriginal);
                    context.SaveChanges();
                }

                int idCliente;
                using (var context = new ItaliasPizzaDbContext())
                {
                    idCliente = context.Clientes
                        .First(c => c.Nombre == "Ana" && c.Apellidos == "López").IdCliente;
                }

                var clienteActualizado = new Cliente
                {
                    IdCliente = idCliente,
                    Nombre = "Ana María",
                    Apellidos = "López Hernández",
                    Telefono = "5599887766"
                };

                bool resultado = ClienteDAO.ActualizarCliente(clienteActualizado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var clienteEnBD = context.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);

                    Assert.NotNull(clienteEnBD);
                    Assert.Equal("Ana María", clienteEnBD.Nombre);
                    Assert.Equal("López Hernández", clienteEnBD.Apellidos);
                    Assert.Equal("5599887766", clienteEnBD.Telefono);
                }
            }
        }

        [Fact]
        public void EliminarCliente_ClienteExistente_DebeDesactivarCliente()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "Juan",
                    Apellidos = "Pérez",
                    Telefono = "5544332211",
                    Status = true
                };

                int idCliente;
                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;
                }

                bool resultado = ClienteDAO.EliminarCliente(idCliente);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var clienteEnBD = context.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
                    Assert.NotNull(clienteEnBD);
                    Assert.False(clienteEnBD.Status);
                }
            }
        }

        [Fact]
        public void ObtenerDireccionDeClientePorId_DebeRetornarDireccion()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "María",
                    Apellidos = "Santos",
                    Telefono = "5512345678",
                    Status = true
                };

                int idCliente;
                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;

                    var direccion = new Direccion
                    {
                        Calle = "Insurgentes",
                        Numero = 456,
                        CodigoPostal = "67890",
                        Colonia = "Roma",
                        Ciudad = "CDMX",
                        Estado = "CDMX",
                        Status = true,
                        Referencia = "Frente al metro",
                        IdCliente = idCliente
                    };

                    context.Direcciones.Add(direccion);
                    context.SaveChanges();
                }

                var direccionObtenida = ClienteDAO.ObtenerDireccionDeClientePorId(idCliente);

                Assert.NotNull(direccionObtenida);
                Assert.Equal("Insurgentes", direccionObtenida.Calle);
                Assert.Equal("67890", direccionObtenida.CodigoPostal);
                Assert.Equal("CDMX", direccionObtenida.Estado);
            }
        }

        [Fact]
        public void ActualizarDireccionDeClientePorId_DebeActualizarCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "Pedro",
                    Apellidos = "Ramírez",
                    Telefono = "5588776655",
                    Status = true
                };

                int idCliente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;

                    var direccion = new Direccion
                    {
                        IdCliente = idCliente,
                        Calle = "Av. Central",
                        Numero = 123,
                        CodigoPostal = "11111",
                        Colonia = "Centro",
                        Ciudad = "CDMX",
                        Estado = "CDMX",
                        Referencia = "Cerca del parque",
                        Status = true
                    };

                    context.Direcciones.Add(direccion);
                    context.SaveChanges();
                }

                var direccionActualizada = new Direccion
                {
                    IdCliente = idCliente,
                    Calle = "Av. Reforma",
                    Numero = 456,
                    CodigoPostal = "22222",
                    Colonia = "Juárez",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    Referencia = "Frente a la plaza"
                };

                bool resultado = ClienteDAO.ActualizarDireccionDeClientePorId(direccionActualizada);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var direccionEnBD = context.Direcciones.FirstOrDefault(d => d.IdCliente == idCliente);

                    Assert.NotNull(direccionEnBD);
                    Assert.Equal("Av. Reforma", direccionEnBD.Calle);
                    Assert.Equal(456, direccionEnBD.Numero);
                    Assert.Equal("22222", direccionEnBD.CodigoPostal);
                    Assert.Equal("Juárez", direccionEnBD.Colonia);
                    Assert.Equal("Frente a la plaza", direccionEnBD.Referencia);
                }
            }
        }

        [Fact]
        public void ValidarDireccionRepetida_DireccionYaExiste_DeberiaRetornarTrue()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "Carlos",
                    Apellidos = "Lopez",
                    Telefono = "5511223344",
                    Status = true
                };

                int idCliente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;

                    var direccion = new Direccion
                    {
                        IdCliente = idCliente,
                        Calle = "Insurgentes",
                        Numero = 123,
                        CodigoPostal = "01000",
                        Colonia = "Roma Norte",
                        Ciudad = "CDMX",
                        Estado = "CDMX",
                        Referencia = "Cerca del metrobus",
                        Status = true
                    };

                    context.Direcciones.Add(direccion);
                    context.SaveChanges();
                }

                var direccionRepetida = new Direccion
                {
                    IdCliente = idCliente,
                    Calle = "Insurgentes",
                    Numero = 123,
                    CodigoPostal = "01000",
                    Colonia = "Roma Norte",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    Referencia = "Cerca del metrobus"
                };

                bool resultado = ClienteDAO.ValidarDireccionRepetida(direccionRepetida);
                Assert.True(resultado);
            }
        }


        [Fact]
        public void ValidarDireccionRepetida_DireccionNoExiste_DeberiaRetornarFalse()
        {
            using (var scope = new TransactionScope())
            {
                var cliente = new Cliente
                {
                    Nombre = "Andrea",
                    Apellidos = "Perez",
                    Telefono = "5599887766",
                    Status = true
                };

                int idCliente;

                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    idCliente = cliente.IdCliente;

                    var direccion = new Direccion
                    {
                        IdCliente = idCliente,
                        Calle = "Reforma",
                        Numero = 200,
                        CodigoPostal = "06600",
                        Colonia = "Juárez",
                        Ciudad = "CDMX",
                        Estado = "CDMX",
                        Referencia = "Frente a Torre Mayor",
                        Status = true
                    };

                    context.Direcciones.Add(direccion);
                    context.SaveChanges();
                }

                var direccionNueva = new Direccion
                {
                    IdCliente = idCliente,
                    Calle = "Chapultepec",
                    Numero = 500,
                    CodigoPostal = "06100",
                    Colonia = "Condesa",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    Referencia = "Cerca del Castillo"
                };

                bool resultado = ClienteDAO.ValidarDireccionRepetida(direccionNueva);

                Assert.False(resultado);
            }
        }

        [Fact]
        public void ObtenerClienteConDireccionesPorTelefono_DebeRetornarClienteConDirecciones()
        {
            using (var scope = new TransactionScope())
            {
                string telefono = "1234567890";
                int idClienteCreado;
                int idDireccion1, idDireccion2;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var cliente = new Cliente
                    {
                        Nombre = "Juan Pérez",
                        Telefono = telefono,
                        Status = true,
                        Direcciones = new List<Direccion>
                {
                    new Direccion { Calle = "Av. Principal", Colonia = "Centro", Numero = 123 },
                    new Direccion { Calle = "Calle Secundaria", Colonia = "Sur", Numero = 456 }
                }
                    };

                    context.Clientes.Add(cliente);
                    context.SaveChanges();

                    idClienteCreado = cliente.IdCliente;
                    idDireccion1 = cliente.Direcciones.ElementAt(0).IdDireccion;
                    idDireccion2 = cliente.Direcciones.ElementAt(1).IdDireccion;
                }

                var clienteObtenido = ClienteDAO.ObtenerClienteConDireccionesPorTelefono(telefono);

                Assert.NotNull(clienteObtenido);
                Assert.Equal(idClienteCreado, clienteObtenido.IdCliente);
                Assert.Equal(2, clienteObtenido.Direcciones.Count);
                Assert.Contains(clienteObtenido.Direcciones, d => d.IdDireccion == idDireccion1);
                Assert.Contains(clienteObtenido.Direcciones, d => d.IdDireccion == idDireccion2);
            }
        }


    }
}
