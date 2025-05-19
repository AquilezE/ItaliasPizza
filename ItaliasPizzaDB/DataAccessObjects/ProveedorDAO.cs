using System;
using System.Collections.Generic;
using System.Linq;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class ProveedorDAO
    {
        public static Proveedor ObtenerProveedorPorId(int idProveedor)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                    return context.Proveedores
                        .FirstOrDefault(p => p.IdProveedor == idProveedor);
            }
        }

        public static List<Proveedor> ObtenerProveedores()
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                    return context.Proveedores.ToList();
            }
        }

        public static List<Proveedor> ObtenerProveedores(string nombre, string telefono)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                    var proveedores = context.Proveedores.AsQueryable();

                    if (!string.IsNullOrEmpty(nombre))
                    {
                        proveedores = proveedores.Where(p => p.Nombre.Contains(nombre));
                    }

                    if (!string.IsNullOrEmpty(telefono))
                    {
                        proveedores = proveedores.Where(p => p.Telefono.Contains(telefono));
                    }

                    return proveedores.ToList();
            }
        }

        public static Proveedor CrearProveedor(Proveedor proveedor)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                context.Proveedores.Add(proveedor);
                context.SaveChanges();
                return proveedor;
            }
        }

        public static bool ActualizarProveedor(Proveedor proveedor)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                    Proveedor proveedorActual = context.Proveedores.FirstOrDefault(p => p.IdProveedor == proveedor.IdProveedor);
                    if (proveedorActual == null)
                    {
                        return false;
                    }

                    proveedorActual.Nombre = proveedor.Nombre;  
                    proveedorActual.Telefono = proveedor.Telefono;
                    proveedorActual.Direccion = proveedor.Direccion;

                    return context.SaveChanges() > 0;

            }
        }

        public static bool AgregarInsumoAProveedor(ProveedorInsumo proveedorInsumo)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                bool yaExiste = context.ProveedoresInsumos
                    .Any(pi => pi.IdProveedor == proveedorInsumo.IdProveedor &&
                               pi.IdInsumo == proveedorInsumo.IdInsumo);

                if (yaExiste)
                {
                    return false;
                }

                context.ProveedoresInsumos.Add(proveedorInsumo);
                return context.SaveChanges() > 0;
            }
        }

        public static int ValidarProveedorPorNombre(String nombreProveedor)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Proveedores
                    .Any(p => p.Nombre == nombreProveedor) ? 1 : 0;

            }
        }

        public static int ValidarProveedorPorTelefono(String telefonoProveedor)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Proveedores
                    .Any(p => p.Telefono == telefonoProveedor) ? 1 : 0;

            }
        }
        public static int ValidarProveedorPorNombreDiferente(string nombreProveedor, int idProveedor)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.Proveedores
                    .Any(p => p.Nombre == nombreProveedor && p.IdProveedor != idProveedor) ? 1 : 0;
            }
        }

        public static int ValidarProveedorPorTelefonoDiferente(string telefonoProveedor, int idProveedor)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.Proveedores
                    .Any(p => p.Telefono == telefonoProveedor && p.IdProveedor != idProveedor) ? 1 : 0;
            }
        }
        public static bool EliminarInsumoDeProveedor(ProveedorInsumo proveedorInsumo)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                var proveedorInsumoDb = context.ProveedoresInsumos
                    .FirstOrDefault(pi => pi.IdProveedor == proveedorInsumo.IdProveedor && pi.IdInsumo == proveedorInsumo.IdInsumo);

                if (proveedorInsumoDb == null)
                    return false;

                context.ProveedoresInsumos.Remove(proveedorInsumoDb);
                return context.SaveChanges() > 0;
            }
        }

        public static List<Insumo> ObtenerInsumosDeProveedor(int idProveedor)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.ProveedoresInsumos
                    .Where(pi => pi.IdProveedor == idProveedor)
                    .Select(pi => pi.Insumo)
                    .ToList();
            }
        }

    }
}
