using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;
using Xunit;
using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using ItaliasPizzaDB.DataAccessObjects;

namespace DatabaseTests
{
    public class ProveedorDAOTests
    {

        [Fact]
        public void ObtenerProveedorPorId_DebeRetornarProveedor()
        {

            using (var scope = new TransactionScope())
            {
                int idProveedor1;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor1 = new Proveedor
                    {
                        Nombre = "Pepito",
                        Telefono = "2281401656",
                        Direccion = "Direccion 1"
                    };

                    context.Proveedores.Add(proveedor1);
                    context.SaveChanges();

                    idProveedor1 = proveedor1.IdProveedor;
                } 

                var proveedorObtenido = ProveedorDAO.ObtenerProveedorPorId(idProveedor1);

                Assert.Equal("Pepito", proveedorObtenido.Nombre);
                Assert.Equal("2281401656", proveedorObtenido.Telefono);
                Assert.Equal("Direccion 1", proveedorObtenido.Direccion);

            }
        }

        [Fact]
        public void ObtenerProveedores_DebeRetornarListaDeProveedores()
        {
            using (var scope = new TransactionScope())
            {

                int idProveedor1;
                int idProveedor2;

                using (var context = new ItaliasPizzaDbContext())
                {

                    var proveedor1 = new Proveedor
                    {
                        Nombre = "Pepito",
                        Telefono = "2281401656",
                        Direccion = "Direccion 1"
                    };

                    var proveedor2 = new Proveedor
                    {
                        Nombre = "Juanito",
                        Telefono = "2281401657",
                        Direccion = "Direccion 2"
                    };

                    context.Proveedores.Add(proveedor1);
                    context.Proveedores.Add(proveedor2);
                    context.SaveChanges();

                    idProveedor1 = proveedor1.IdProveedor;
                    idProveedor2 = proveedor2.IdProveedor;
                }

                var proveedoresObtenidos = ProveedorDAO.ObtenerProveedores();

                Assert.True(proveedoresObtenidos.Count == 2);
                Assert.Contains(proveedoresObtenidos, p => p.IdProveedor == idProveedor1);
                Assert.Contains(proveedoresObtenidos, p => p.IdProveedor == idProveedor2);

                //TODO: hacer un equals para proveedores
            }
        }

        [Fact]
        public void ObtenerProveedores_DebeRetornarListaDeProveedoresConFiltros()
        {
            using (var scope = new TransactionScope())
            {

                int idProveedor1;
                int idProveedor2;
                int idProveedor3;

                using (var context = new ItaliasPizzaDbContext())
                {

                    var proveedor1 = new Proveedor
                    {
                        Nombre = "Pepito",
                        Telefono = "2281401656",
                        Direccion = "Direccion 1"
                    };

                    var proveedor2 = new Proveedor
                    {
                        Nombre = "Juanito",
                        Telefono = "4421401656",
                        Direccion = "Direccion 2"
                    };

                    var proveedor3 = new Proveedor
                    {
                        Nombre = "JJJ",
                        Telefono = "123456",
                        Direccion = "Direccion 3"
                    };

                    context.Proveedores.Add(proveedor1);
                    context.Proveedores.Add(proveedor2);
                    context.Proveedores.Add(proveedor3);
                    context.SaveChanges();

                    idProveedor1 = proveedor1.IdProveedor;
                    idProveedor2 = proveedor2.IdProveedor;
                    idProveedor3 = proveedor3.IdProveedor;
                }

                var proveedoresObtenidos = ProveedorDAO.ObtenerProveedores("ito", "1656");

                Assert.True(proveedoresObtenidos.Count == 2);

            }
        }

        [Fact]
        public void ObtenerProveedores_DebeRetornarListaDeProveedoresConFiltroTelefono()
        {
            using (var scope = new TransactionScope())
            {

                int idProveedor1;
                int idProveedor2;
                int idProveedor3;

                using (var context = new ItaliasPizzaDbContext())
                {

                    var proveedor1 = new Proveedor
                    {
                        Nombre = "Pepito",
                        Telefono = "2281401656",
                        Direccion = "Direccion 1"
                    };

                    var proveedor2 = new Proveedor
                    {
                        Nombre = "Juanito",
                        Telefono = "4421401656",
                        Direccion = "Direccion 2"
                    };

                    var proveedor3 = new Proveedor
                    {
                        Nombre = "JJJ",
                        Telefono = "123456",
                        Direccion = "Direccion 3"
                    };

                    context.Proveedores.Add(proveedor1);
                    context.Proveedores.Add(proveedor2);
                    context.Proveedores.Add(proveedor3);
                    context.SaveChanges();

                    idProveedor1 = proveedor1.IdProveedor;
                    idProveedor2 = proveedor2.IdProveedor;
                    idProveedor3 = proveedor3.IdProveedor;
                }

                var proveedoresObtenidos = ProveedorDAO.ObtenerProveedores("", "1656");

                Assert.True(proveedoresObtenidos.Count == 2);

            }
        }

        [Fact]
        public void ObtenerProveedores_DebeRetornarListaDeProveedoresConFiltroNombre()
        {
            using (var scope = new TransactionScope())
            {

                int idProveedor1;
                int idProveedor2;
                int idProveedor3;

                using (var context = new ItaliasPizzaDbContext())
                {

                    var proveedor1 = new Proveedor
                    {
                        Nombre = "Pepito",
                        Telefono = "2281401656",
                        Direccion = "Direccion 1"
                    };

                    var proveedor2 = new Proveedor
                    {
                        Nombre = "Juanito",
                        Telefono = "4421401656",
                        Direccion = "Direccion 2"
                    };

                    var proveedor3 = new Proveedor
                    {
                        Nombre = "JJJ",
                        Telefono = "123456",
                        Direccion = "Direccion 3"
                    };

                    context.Proveedores.Add(proveedor1);
                    context.Proveedores.Add(proveedor2);
                    context.Proveedores.Add(proveedor3);
                    context.SaveChanges();

                    idProveedor1 = proveedor1.IdProveedor;
                    idProveedor2 = proveedor2.IdProveedor;
                    idProveedor3 = proveedor3.IdProveedor;
                }

                var proveedoresObtenidos = ProveedorDAO.ObtenerProveedores("ito", "");

                Assert.True(proveedoresObtenidos.Count == 2);

            }
        }

        [Fact]
        public void CrearProveedor_CrearProveedorCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                Proveedor nuevoProveedor = new Proveedor
                {
                    Nombre = "ProveedorTest",
                    Telefono = "1234567890",
                    Direccion = "Calle Ficticia 123"
                };

                bool resultado = ProveedorDAO.CrearProveedor(nuevoProveedor);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedorGuardado = context.Proveedores
                        .FirstOrDefault(p => p.Nombre == "ProveedorTest" && p.Telefono == "1234567890");

                    Assert.NotNull(proveedorGuardado);
                    Assert.Equal("ProveedorTest", proveedorGuardado.Nombre);
                    Assert.Equal("1234567890", proveedorGuardado.Telefono);
                    Assert.Equal("Calle Ficticia 123", proveedorGuardado.Direccion);
                }
            }
        }

        [Fact]
        public void AgregarInsumoAProveedor_AgregarInsumoAProveedor()
        {
            using (var scope = new TransactionScope())
            {
                int idProveedor;
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = "ProveedorTest",
                        Telefono = "5551234567",
                        Direccion = "Calle X"
                    };

                    var insumo = new Insumo
                    {
                        Nombre = "InsumoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Proveedores.Add(proveedor);
                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    idProveedor = proveedor.IdProveedor;
                    idInsumo = insumo.IdInsumo;
                }

                var proveedorInsumo = new ProveedorInsumo
                {
                    IdProveedor = idProveedor,
                    IdInsumo = idInsumo
                };

                bool resultado = ProveedorDAO.AgregarInsumoAProveedor(proveedorInsumo);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var relacion = context.ProveedoresInsumos
                        .FirstOrDefault(pi => pi.IdProveedor == idProveedor && pi.IdInsumo == idInsumo);

                    Assert.NotNull(relacion);
                }
            }
        }

        [Fact]
        public void ValidarProveedorPorNombre_RetornarUnoSiExiste()
        {
            using (var scope = new TransactionScope())
            {
                string nombreProveedor = "ProveedorPrueba";

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = nombreProveedor,
                        Telefono = "1234567890",
                        Direccion = "Dirección de prueba"
                    };

                    context.Proveedores.Add(proveedor);
                    context.SaveChanges();
                }

                int resultado = ProveedorDAO.ValidarProveedorPorNombre(nombreProveedor);

                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void ValidarProveedorPorNombre_RetornarCeroSiNoExiste()
        {
            using (var scope = new TransactionScope())
            {
                string nombreProveedor = "ProveedorInexistente";

                int resultado = ProveedorDAO.ValidarProveedorPorNombre(nombreProveedor);

                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ValidarProveedorPorTelefono_RetornarUnoSiExiste()
        {
            using (var scope = new TransactionScope())
            {
                string numeroProveedor = "1234567890";

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = "Nestle",
                        Telefono = numeroProveedor,
                        Direccion = "Dirección de prueba"
                    };

                    context.Proveedores.Add(proveedor);
                    context.SaveChanges();
                }

                int resultado = ProveedorDAO.ValidarProveedorPorTelefono(numeroProveedor);

                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void ValidarProveedorPorTelefono_RetornarCeroSiNoExiste()
        {
            using (var scope = new TransactionScope())
            {
                string numeroProveedor = "1234567890";

                int resultado = ProveedorDAO.ValidarProveedorPorTelefono(numeroProveedor);

                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ActualizarProveedor_ActualizarDatosCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idProveedor;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = "ProveedorOriginal",
                        Telefono = "1234567890",
                        Direccion = "Dirección Original"
                    };

                    context.Proveedores.Add(proveedor);
                    context.SaveChanges();
                    idProveedor = proveedor.IdProveedor;
                }

                var proveedorActualizado = new Proveedor
                {
                    IdProveedor = idProveedor,
                    Nombre = "ProveedorActualizado",
                    Telefono = "0987654321",
                    Direccion = "Nueva Dirección"
                };

                bool resultado = ProveedorDAO.ActualizarProveedor(proveedorActualizado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedorDb = context.Proveedores.FirstOrDefault(p => p.IdProveedor == idProveedor);

                    Assert.NotNull(proveedorDb);
                    Assert.Equal("ProveedorActualizado", proveedorDb.Nombre);
                    Assert.Equal("0987654321", proveedorDb.Telefono);
                    Assert.Equal("Nueva Dirección", proveedorDb.Direccion);
                }
            }
        }

        [Fact]
        public void EliminarInsumoDeProveedor_DeberiaEliminarCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idProveedor;
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedor = new Proveedor
                    {
                        Nombre = "ProveedorTest",
                        Telefono = "123456789",
                        Direccion = "Dirección Test"
                    };
                    context.Proveedores.Add(proveedor);

                    var insumo = new Insumo
                    {
                        Nombre = "InsumoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };
                    context.Insumos.Add(insumo);

                    context.SaveChanges();

                    idProveedor = proveedor.IdProveedor;
                    idInsumo = insumo.IdInsumo;

                    var proveedorInsumo = new ProveedorInsumo
                    {
                        IdProveedor = idProveedor,
                        IdInsumo = idInsumo
                    };
                    context.ProveedoresInsumos.Add(proveedorInsumo);
                    context.SaveChanges();

                }

                using (var context = new ItaliasPizzaDbContext())
                {
                    var proveedorInsumoDb = context.ProveedoresInsumos.FirstOrDefault(pi => pi.IdInsumo == idInsumo && pi.IdProveedor == idProveedor);
                    Assert.NotNull(proveedorInsumoDb);

                    bool resultado = ProveedorDAO.EliminarInsumoDeProveedor(proveedorInsumoDb);

                    Assert.True(resultado);

                    var eliminado = context.ProveedoresInsumos.FirstOrDefault(pi => pi.IdInsumo == idInsumo && pi.IdProveedor == idProveedor);
                    Assert.Null(eliminado);
                }
            }
        }

    }
}
