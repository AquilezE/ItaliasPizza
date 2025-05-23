using System;
using System.Transactions;
using Xunit;
using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB.DataAccessObjects;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseTests
{
    [Collection("Database collection")]
    public class SalidaDAOTests
    {
        [Fact]
        public async Task RegistrarTransaccion_DebeRetornarTrue_CuandoEsExitosa()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    if (!context.TiposTransaccion.Any(t => t.IdTipoTransaccion == 1))
                    {
                        context.TiposTransaccion.Add(new TipoTransaccion
                        {
                            IdTipoTransaccion = 1,
                            TipoTransaccionesNombre = "Venta"
                        });
                        await context.SaveChangesAsync();
                    }
                }

                float cantidad = 150.0f;
                string descripcion = "Pago del servicio de internet mes enero";
                int idEmpleado = 2;
                int idTipoTransaccion = 1;

                bool resultado = await SalidaDAO.RegistrarTransaccion(cantidad, descripcion, idEmpleado, idTipoTransaccion);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var transaccion = context.Transacciones
                        .OrderByDescending(t => t.IdTransaccion)
                        .FirstOrDefault();

                    Assert.NotNull(transaccion);
                    Assert.Equal(cantidad, transaccion.Cantidad);
                    Assert.Equal(descripcion, transaccion.Descripcion);
                    Assert.Equal(idEmpleado, transaccion.IdEmpleado);
                    Assert.Equal(idTipoTransaccion, transaccion.IdTipoTransaccion);
                    Assert.Equal(DateTime.Now.Date, transaccion.Fecha.Date);
                }
            }
        }

        [Fact]
        public async Task RegistrarTransaccion_DebeRetornarFalse_SiFallaAlGuardar()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                float cantidad = 100.0f;
                string descripcion = "Intento fallido";
                int idEmpleado = -999;
                int idTipoTransaccion = 1;

                bool resultado = await SalidaDAO.RegistrarTransaccion(cantidad, descripcion, idEmpleado, idTipoTransaccion);

                
                Assert.False(resultado);
            }
        }

        [Fact]
        public void RegistrarGastoEnCorteDeCaja_DebeSumarAlGastoDelCorteMasReciente()
        {
            using (var scope = new TransactionScope())
            {
                int idCorte;
                float gastoInicial = 100.0f;
                float gastoAAgregar = 50.0f;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corte = new CorteDeCaja
                    {
                        FechaApertura = DateTime.Now,
                        FechaCierre = DateTime.Now.AddHours(1),
                        Cambio = 500.0f,
                        VentaDelDia = 2000.0f,
                        Gasto = gastoInicial,
                        IdEmpleado = 2
                    };

                    context.CortesDeCaja.Add(corte);
                    context.SaveChanges();
                    idCorte = corte.IdCorteDeCaja;
                }

                int resultado = SalidaDAO.RegistrarGastoEnCorteDeCaja(gastoAAgregar);

                Assert.Equal(0, resultado); 

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corteActualizado = context.CortesDeCaja.Find(idCorte);
                    Assert.Equal(gastoInicial + gastoAAgregar, corteActualizado.Gasto);
                }
            }
        }

        [Fact]
        public void RegistrarGastoEnCorteDeCaja_DebeFallaSiNoHayCortes()
        {
            using (var scope = new TransactionScope())
            {
                int resultado = SalidaDAO.RegistrarGastoEnCorteDeCaja(50.0f);

                Assert.Equal(1, resultado); 
            }
        }

        [Fact]
        public void RegistrarGastoEnCorteDeCaja_DebeFallaSiMontoNoEsPositivo()
        {
            using (var scope = new TransactionScope())
            {
                int resultado1 = SalidaDAO.RegistrarGastoEnCorteDeCaja(0);
                Assert.Equal(2, resultado1); 

                int resultado2 = SalidaDAO.RegistrarGastoEnCorteDeCaja(-100);
                Assert.Equal(2, resultado2); 
            }
        }

        [Fact]
        public void RegistrarGastoEnCorteDeCaja_DebeUsarElCorteMasReciente()
        {
            using (var scope = new TransactionScope())
            {
                int idCorteReciente;
                float gastoInicial = 200.0f;

                using (var context = new ItaliasPizzaDbContext())
                {
                    
                    var corteAntiguo = new CorteDeCaja
                    {
                        FechaApertura = DateTime.Now.AddDays(-1),
                        FechaCierre = DateTime.Now.AddDays(-1).AddHours(1),
                        Gasto = 100.0f,
                        IdEmpleado = 2
                    };

                    var corteReciente = new CorteDeCaja
                    {
                        FechaApertura = DateTime.Now,
                        FechaCierre = DateTime.Now.AddHours(1),
                        Gasto = gastoInicial,
                        IdEmpleado = 2
                    };

                    context.CortesDeCaja.Add(corteAntiguo);
                    context.CortesDeCaja.Add(corteReciente);
                    context.SaveChanges();
                    idCorteReciente = corteReciente.IdCorteDeCaja;
                }

                int resultado = SalidaDAO.RegistrarGastoEnCorteDeCaja(75.0f);
                Assert.Equal(0, resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var corteRecienteActualizado = context.CortesDeCaja.Find(idCorteReciente);
                    Assert.Equal(gastoInicial + 75.0f, corteRecienteActualizado.Gasto);

                    var corteAntiguo = context.CortesDeCaja
                        .Where(c => c.IdCorteDeCaja != idCorteReciente)
                        .First();
                    Assert.Equal(100.0f, corteAntiguo.Gasto);
                }
            }
        }


    }
}
