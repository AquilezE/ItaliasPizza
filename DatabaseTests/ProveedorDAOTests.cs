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
    }
}
