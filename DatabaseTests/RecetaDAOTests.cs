using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using Xunit;

namespace DatabaseTests
{

    public class RecetaDAOTests
    {
        /*[Fact]
        public void GuardarReceta_ConInsumos_DeberiaGuardarCorrectamente()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int idReceta;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoEjemplo = context.Insumos.FirstOrDefault();
                    Assert.NotNull(insumoEjemplo); // Asegúrate de tener insumos en la BD

                    var receta = new Receta
                    {
                        Instrucciones = "Instrucciones de prueba",
                        InsumosParaReceta = new List<InsumoParaReceta>
                        {
                            new InsumoParaReceta
                            {
                                IdInsumo = insumoEjemplo.IdInsumo,
                                Cantidad = 2
                            }
                        }
                    };

                    context.Recetas.Add(receta);
                    context.SaveChanges();

                    idReceta = receta.IdReceta;
                }

                using (var context = new ItaliasPizzaDbContext())
                {
                    var recetaGuardada = context.Recetas
                        .Include(r => r.InsumosParaReceta)
                        .FirstOrDefault(r => r.IdReceta == idReceta);

                    Assert.NotNull(recetaGuardada);
                    Assert.Equal("Instrucciones de prueba", recetaGuardada.Instrucciones);
                    Assert.Single(recetaGuardada.InsumosParaReceta);
                }
            }
        }*/
    }
}
