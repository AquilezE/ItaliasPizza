using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.Repositorios
{
    public class ProveedorRepositorio
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

        public static bool CrearProveedor(Proveedor proveedor)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                    context.Proveedores.Add(proveedor);
                    return context.SaveChanges() > 0;
   
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

    }
}
