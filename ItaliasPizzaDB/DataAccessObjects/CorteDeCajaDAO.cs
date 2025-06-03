using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace ItaliasPizzaDB.DataAccessObjects
{
    public class CorteDeCajaDAO
    {
        public static CorteDeCaja ObtenerUltimoCorteDeCaja()
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                return context.CortesDeCaja
                    .OrderByDescending(c => c.FechaApertura) 
                    .FirstOrDefault(); 
            }
        }

        public static int RegistrarNuevoCorteDeCaja(int idEmpleado, float cambioInicial, DateTime fecha)
        {
            using (ItaliasPizzaDbContext context = new ItaliasPizzaDbContext())
            {
                try
                {
                    DateTime fechaNoCerrado = new DateTime(1900, 1, 1);

                    var nuevoCorte = new CorteDeCaja
                    {
                        FechaApertura = fecha,
                        Cambio = cambioInicial,
                        VentaDelDia = 0, 
                        Gasto = 0,       
                        IdEmpleado = idEmpleado,
                        FechaCierre = fechaNoCerrado,
                    };

                    context.CortesDeCaja.Add(nuevoCorte);
                    context.SaveChanges();

                    return 0; 
                }
                catch (Exception ex)
                {
                    return -1; 
                }
            }
        }

        public static int RegistrarFechaCierre(DateTime fechaApertura)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                try
                {
                    var corte = context.CortesDeCaja
                        .FirstOrDefault(c => DbFunctions.TruncateTime(c.FechaApertura) == fechaApertura.Date);

                    if (corte == null)
                        return -1; 

                    DateTime fechaNoCerrado = new DateTime(1900, 1, 1);

                    if (corte.FechaCierre > fechaNoCerrado)
                    {
                        return 1; 
                    }

                    corte.FechaCierre = DateTime.Now;
                    context.SaveChanges();

                    return 0; 
                }
                catch (Exception ex)
                {
                    return -1; 
                }
            }
        }

        public static int AgregarVentaAlCorteDelDia(float montoPedido)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                try
                {
                    // Obtener el corte de caja abierto (sin fecha de cierre)
                    DateTime fechaNoCerrado = new DateTime(1900, 1, 1);
                    var corte = context.CortesDeCaja
                        .FirstOrDefault(c => c.FechaCierre == fechaNoCerrado);

                    if (corte == null)
                        return -1; // No hay corte de caja abierto

                    if (montoPedido <= 0)
                        return -2; // Monto inválido

                    corte.VentaDelDia += montoPedido;
                    context.SaveChanges();

                    return 0; // Éxito
                }
                catch (Exception ex)
                {
                    // Considera registrar el error (logging)
                    return -1; // Error general
                }
            }
        }

        public static int RestarSalidaAlCorteDelDia(float montoSalida)
        {
            using (var context = new ItaliasPizzaDbContext())
            {
                try
                {
                    DateTime fechaNoCerrado = new DateTime(1900, 1, 1);
                    var corte = context.CortesDeCaja
                        .FirstOrDefault(c => c.FechaCierre == fechaNoCerrado);

                    if (corte == null)
                        return -1; 

                    if (montoSalida <= 0)
                        return -2; 

                    corte.VentaDelDia -= montoSalida;
                    context.SaveChanges();

                    return 0; 
                }
                catch (Exception ex)
                {
                     return -1; // Error general
                }
            }
        }
    }
}

    
