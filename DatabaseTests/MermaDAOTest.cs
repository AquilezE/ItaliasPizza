using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace DatabaseTests
{
    public class MermaDAOTest
    {
        [Fact]
        public void RegistrarMerma_RegistraCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoParaMerma",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Cantidad= 20f,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;
                }

                float cantidadMerma = 3.5f;
                bool resultado = MermaDAO.RegistrarMerma(idInsumo, cantidadMerma);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var mermaRegistrada = context.Mermas
                        .FirstOrDefault(m => m.IdInsumo == idInsumo && m.Cantidad == cantidadMerma);

                    Assert.NotNull(mermaRegistrada);
                    Assert.Equal(idInsumo, mermaRegistrada.IdInsumo);
                    Assert.Equal(cantidadMerma, mermaRegistrada.Cantidad, 2);
                    Assert.True((DateTime.Now - mermaRegistrada.Fecha).TotalMinutes < 1);
                }
            }
        }

    }
}
