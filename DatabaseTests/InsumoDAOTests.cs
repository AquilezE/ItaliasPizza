using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using Xunit;

namespace DatabaseTests
{
    public class InsumoDAOTests
    {

        [Fact]
        public void ObtenerInsumosPorCategoria_DevuelveInsumosDeCategoriaEspecifica()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo1;
                int idInsumo2;
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo1 = new Insumo
                    {
                        Nombre = "TestInsumo1",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true,
                    };

                    var insumo2 = new Insumo
                    {
                        Nombre = "TestInsumo2",
                        IdCategoriaInsumo = 2,
                        IdUnidadDeMedida = 1,
                        Status = true,
                    };
                    context.Insumos.Add(insumo1);
                    context.Insumos.Add(insumo2);

                    context.SaveChanges();

                    idInsumo1 = insumo1.IdInsumo;
                    idInsumo2 = insumo2.IdInsumo;
                }
                List<Insumo> insumos = InsumoDAO.ObtenerInsumos(1, -1, true);
                Assert.NotNull(insumos);
                Assert.Single(insumos);
                Assert.Equal(idInsumo1, insumos[0].IdInsumo);
                Assert.Equal("TestInsumo1", insumos[0].Nombre);
            }
        }

        [Fact]
        public void ObtenerInsumosPorUnidadDeMedida_DevuelveInsumosDeUnidadDeMedida()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo1;
                int idInsumo2;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo1 = new Insumo
                    {
                        Nombre = "TestInsumo1",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true,
                    };

                    var insumo2 = new Insumo
                    {
                        Nombre = "TestInsumo2",
                        IdCategoriaInsumo = 2,
                        IdUnidadDeMedida = 2,
                        Status = true,
                    };

                    context.Insumos.Add(insumo1);
                    context.Insumos.Add(insumo2);

                    context.SaveChanges();

                    idInsumo1 = insumo1.IdInsumo;
                    idInsumo2 = insumo2.IdInsumo;
                }

                List<Insumo> insumos = InsumoDAO.ObtenerInsumos(-1, 2, true);
                Assert.NotNull(insumos);
                Assert.Single(insumos);
                Assert.Equal(idInsumo2, insumos[0].IdInsumo);
                Assert.Equal("TestInsumo2", insumos[0].Nombre);
            }
        }

        [Fact]
        public void ObtenerInsumosInactivos_DevuelveInsumosInactivos()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo1;
                int idInsumo2;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo1 = new Insumo
                    {
                        Nombre = "TestInsumo1",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = false,
                    };

                    var insumo2 = new Insumo
                    {
                        Nombre = "TestInsumo2",
                        IdCategoriaInsumo = 2,
                        IdUnidadDeMedida = 1,
                        Status = true,
                    };

                    context.Insumos.Add(insumo1);
                    context.Insumos.Add(insumo2);

                    context.SaveChanges();

                    idInsumo1 = insumo1.IdInsumo;
                    idInsumo2 = insumo2.IdInsumo;
                }

                List<Insumo> insumos = InsumoDAO.ObtenerInsumos(-1, -1, false);
                Assert.NotNull(insumos);
                Assert.Single(insumos);
                Assert.Equal(idInsumo1, insumos[0].IdInsumo);
                Assert.Equal("TestInsumo1", insumos[0].Nombre);
            }
        }
        
        [Fact]
        public void ObtenerInsumos_DevuelveListaVaciaSiEstaVacia()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    context.Insumos.RemoveRange(context.Insumos);
                    context.SaveChanges();
                }
                List<Insumo> insumos = InsumoDAO.ObtenerInsumos(-1, -1, true);
                Assert.NotNull(insumos);
                Assert.Empty(insumos);
            }
        }

    }
}
