using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using Xunit;

namespace DatabaseTests
{
    public class CorteDeCajaDAOTests
    {
        [Fact]
        public void ObtenerUltimoCorteDeCaja_DebeRetornarElMasReciente()
        {
            using (var scope = new TransactionScope())
            {
                DateTime fecha1 = DateTime.Now.AddHours(-2);
                DateTime fecha2 = DateTime.Now.AddHours(-1);
                DateTime fechaPorDefecto = new DateTime(1753, 1, 1); // Mínimo válido para SQL datetime

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corte1 = new CorteDeCaja
                    {
                        FechaApertura = fecha1,
                        Cambio = 1000,
                        IdEmpleado = 1,
                        FechaCierre = fechaPorDefecto
                    };

                    var corte2 = new CorteDeCaja
                    {
                        FechaApertura = fecha2,
                        Cambio = 1500,
                        IdEmpleado = 2,
                        FechaCierre = fechaPorDefecto
                    };

                    context.CortesDeCaja.Add(corte1);
                    context.CortesDeCaja.Add(corte2);
                    context.SaveChanges();
                }

                var ultimoCorte = CorteDeCajaDAO.ObtenerUltimoCorteDeCaja();

                Assert.NotNull(ultimoCorte);
                Assert.Equal(1500, ultimoCorte.Cambio);
                Assert.Equal(2, ultimoCorte.IdEmpleado);
                Assert.Equal(fecha2.Date, ultimoCorte.FechaApertura.Date);
            }
        }

        [Fact]
        public void RegistrarNuevoCorteDeCaja_DebeRegistrarCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                DateTime fecha = DateTime.Now.Date;
                int idEmpleado = 1;
                float cambioInicial = 1000.0f;

                int resultado = CorteDeCajaDAO.RegistrarNuevoCorteDeCaja(idEmpleado, cambioInicial, fecha);

                Assert.Equal(0, resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corteRegistrado = context.CortesDeCaja
                        .OrderByDescending(c => c.FechaApertura)
                        .FirstOrDefault();

                    Assert.NotNull(corteRegistrado);
                    Assert.Equal(idEmpleado, corteRegistrado.IdEmpleado);
                    Assert.Equal(cambioInicial, corteRegistrado.Cambio);
                    Assert.Equal(fecha.Date, corteRegistrado.FechaApertura.Date);
                    Assert.Equal(0, corteRegistrado.VentaDelDia);
                    Assert.Equal(0, corteRegistrado.Gasto);
                }
            }
        }

        [Fact]
        public void RegistrarNuevoCorteDeCaja_ConError_DebeRetornarMenosUno()
        {
            using (var scope = new TransactionScope())
            {
                int resultado = CorteDeCajaDAO.RegistrarNuevoCorteDeCaja(-1, 1000.0f, DateTime.Now);

                Assert.Equal(-1, resultado);
            }
        }

        [Fact]
        public void RegistrarFechaCierre_DebeActualizarCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                DateTime fechaApertura = DateTime.Now.Date;
                DateTime fechaNoCerrado = new DateTime(1900, 1, 1);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corte = new CorteDeCaja
                    {
                        FechaApertura = fechaApertura,
                        Cambio = 1000,
                        IdEmpleado = 1,
                        FechaCierre = fechaNoCerrado,
                        VentaDelDia = 0,
                        Gasto = 0
                    };

                    context.CortesDeCaja.Add(corte);
                    context.SaveChanges();
                }

                int resultado = CorteDeCajaDAO.RegistrarFechaCierre(fechaApertura);

                Assert.Equal(0, resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corteActualizado = context.CortesDeCaja
                        .FirstOrDefault(c => DbFunctions.TruncateTime(c.FechaApertura) == fechaApertura.Date);

                    Assert.NotNull(corteActualizado);
                    Assert.True(corteActualizado.FechaCierre > fechaNoCerrado);
                    Assert.InRange(
                        (DateTime.Now - corteActualizado.FechaCierre).TotalSeconds,
                        0,
                        10
                    );
                }
            }
        }

        [Fact]
        public void RegistrarFechaCierre_CorteYaCerrado_DebeRetornarUno()
        {
            using (var scope = new TransactionScope())
            {
                DateTime fechaApertura = DateTime.Now.Date;
                DateTime fechaCierreExistente = DateTime.Now.Date.AddMinutes(-30);

                using (var context = new ItaliasPizzaDbContext())
                {
                    // Asegurarse que FechaCierre sea mayor que fechaNoCerrado (1900-01-01)
                    var corte = new CorteDeCaja
                    {
                        FechaApertura = fechaApertura,
                        FechaCierre = fechaCierreExistente,
                        Cambio = 1000,
                        IdEmpleado = 1,
                        VentaDelDia = 0,    
                        Gasto = 0           
                    };

                    context.CortesDeCaja.Add(corte);
                    context.SaveChanges();
                }

                int resultado = CorteDeCajaDAO.RegistrarFechaCierre(fechaApertura);

                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void RegistrarFechaCierre_CorteNoExiste_DebeRetornarMenosUno()
        {
            using (var scope = new TransactionScope())
            {
                DateTime fechaNoExistente = new DateTime(2000, 1, 1);

                int resultado = CorteDeCajaDAO.RegistrarFechaCierre(fechaNoExistente);

                Assert.Equal(-1, resultado);
            }
        }

        [Fact]
        public void AgregarVentaAlCorteDelDia_DebeIncrementarVentaDelDiaCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                // Arrange
                DateTime fechaNoCerrado = new DateTime(1900, 1, 1);
                float montoPedido = 150.50f;
                float ventaInicial = 500.0f;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corte = new CorteDeCaja
                    {
                        FechaApertura = DateTime.Now,
                        Cambio = 1000,
                        IdEmpleado = 1,
                        FechaCierre = fechaNoCerrado,
                        VentaDelDia = ventaInicial,
                        Gasto = 0
                    };

                    context.CortesDeCaja.Add(corte);
                    context.SaveChanges();
                }

                // Act
                int resultado = CorteDeCajaDAO.AgregarVentaAlCorteDelDia(montoPedido);

                // Assert
                Assert.Equal(0, resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corteActualizado = context.CortesDeCaja
                        .FirstOrDefault(c => c.FechaCierre == fechaNoCerrado);

                    Assert.NotNull(corteActualizado);
                    Assert.Equal(ventaInicial + montoPedido, corteActualizado.VentaDelDia);
                }
            }
        }

        [Fact]
        public void AgregarVentaAlCorteDelDia_SinCorteAbierto_DebeRetornarMenosUno()
        {
            using (var scope = new TransactionScope())
            {
                // Arrange - No creamos ningún corte abierto

                // Act
                int resultado = CorteDeCajaDAO.AgregarVentaAlCorteDelDia(150.50f);

                // Assert
                Assert.Equal(-1, resultado);
            }
        }

        [Fact]
        public void AgregarVentaAlCorteDelDia_ConMontoInvalido_DebeRetornarMenosDos()
        {
            using (var scope = new TransactionScope())
            {
                // Arrange
                DateTime fechaNoCerrado = new DateTime(1900, 1, 1);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corte = new CorteDeCaja
                    {
                        FechaApertura = DateTime.Now,
                        Cambio = 1000,
                        IdEmpleado = 1,
                        FechaCierre = fechaNoCerrado,
                        VentaDelDia = 0,
                        Gasto = 0
                    };

                    context.CortesDeCaja.Add(corte);
                    context.SaveChanges();
                }

                // Act - Monto negativo
                int resultado = CorteDeCajaDAO.AgregarVentaAlCorteDelDia(-50.0f);

                // Assert
                Assert.Equal(-2, resultado);
            }
        }

        

    }
}
