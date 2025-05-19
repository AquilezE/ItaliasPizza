using ItaliasPizzaDB.Models;
using System.Data.Entity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ItaliasPizzaDB.DataAccessObjects
{
    public class ClienteDAO
    {

        public static Cliente ObtenerClientePorId(int idCliente)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Clientes
                    .FirstOrDefault(p => p.IdCliente == idCliente);
            }
        }

        public static Cliente CrearCliente(Cliente cliente)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                context.Clientes.Add(cliente);
                context.SaveChanges();
                return cliente;
            }
        }

        public static bool AgregarDireccion(Direccion direccion)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                context.Direcciones.Add(direccion);
                return context.SaveChanges() > 0;

            }
        }

        public static int ValidarClientePorTelefono(String telefonoCliente)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Clientes
                    .Any(p => p.Telefono == telefonoCliente) ? 1 : 0;

            }
        }

        public static bool ActualizarCliente(Cliente cliente)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                Cliente clienteActual = context.Clientes.FirstOrDefault(p => p.IdCliente == cliente.IdCliente);
                if (clienteActual == null)
                {
                    return false;
                }

                clienteActual.Nombre = cliente.Nombre;
                clienteActual.Telefono = cliente.Telefono;
                clienteActual.Apellidos = cliente.Apellidos;

                return context.SaveChanges() > 0;

            }
        }

        public static bool EliminarCliente(int idCliente)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                Cliente clienteActual = context.Clientes.FirstOrDefault(p => p.IdCliente == idCliente);
                if (clienteActual == null)
                {
                    return false;
                }

                clienteActual.Status = false;

                return context.SaveChanges() > 0;

            }
        }

        public static Direccion ObtenerDireccionDeClientePorId(int idCliente)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Direcciones
                    .FirstOrDefault(p => p.IdCliente == idCliente);
            }
        }

        public static bool ActualizarDireccionDeClientePorId(Direccion direccion)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {

                Direccion direccionActual = context.Direcciones.FirstOrDefault(p => p.IdCliente == direccion.IdCliente);
                
                if (direccionActual == null)
                {
                    return false;
                }

                direccionActual.Numero = direccion.Numero;
                direccionActual.Calle = direccion.Calle;
                direccionActual.CodigoPostal = direccion.CodigoPostal;
                direccionActual.Colonia = direccion.Colonia;
                direccionActual.Ciudad = direccion.Ciudad;
                direccionActual.Estado = direccion.Estado;
                direccionActual.Referencia = direccion.Referencia;

                return context.SaveChanges() > 0;

            }
        }

        public static bool ValidarDireccionRepetida(Direccion direccion)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.Direcciones.Any(d =>
                    d.IdCliente == direccion.IdCliente &&
                    d.Calle == direccion.Calle &&
                    d.Numero == direccion.Numero &&
                    d.Colonia == direccion.Colonia &&
                    d.CodigoPostal == direccion.CodigoPostal &&
                    d.Ciudad == direccion.Ciudad &&
                    d.Estado == direccion.Estado &&
                    d.Referencia == direccion.Referencia
                );
            }
        }

        public static Cliente ObtenerClienteConDireccionesPorTelefono(string telefono)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                return context.Clientes
                    .Include(c => c.Direcciones)
                    .FirstOrDefault(c => c.Telefono == telefono && c.Status);
            }
        }

    }
}
