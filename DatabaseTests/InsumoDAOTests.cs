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
                var insumoFound = insumos.FirstOrDefault(i => i.Nombre == "TestInsumo1");
                Assert.NotNull(insumoFound);
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

        [Fact]
        public void CrearInsumo_CreaNuevoInsumoCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                var nuevoInsumo = new Insumo
                {
                    Nombre = "NuevoTestInsumo",
                    IdCategoriaInsumo = 1,
                    IdUnidadDeMedida = 1,
                    Status = true
                };

                bool resultado = InsumoDAO.CrearInsumo(nuevoInsumo);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoEnDb = context.Insumos.FirstOrDefault(i => i.Nombre == "NuevoTestInsumo");
                    Assert.NotNull(insumoEnDb);
                    Assert.Equal(1, insumoEnDb.IdCategoriaInsumo);
                    Assert.Equal(1, insumoEnDb.IdUnidadDeMedida);
                    Assert.True(insumoEnDb.Status);
                }
            }
        }

        [Fact]
        public void ActualizarInsumo_ActualizaInsumoExistenteCorrectamente()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoOriginal = new Insumo
                    {
                        Nombre = "InsumoOriginal",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Insumos.Add(insumoOriginal);
                    context.SaveChanges();

                    idInsumo = insumoOriginal.IdInsumo;
                }

                var insumoActualizado = new Insumo
                {
                    IdInsumo = idInsumo,
                    Nombre = "InsumoActualizado",
                    IdCategoriaInsumo = 2,
                    Status = false
                };

                bool resultado = InsumoDAO.ActualizarInsumo(insumoActualizado);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoEnDb = context.Insumos.FirstOrDefault(i => i.IdInsumo == idInsumo);
                    Assert.NotNull(insumoEnDb);
                    Assert.Equal("InsumoActualizado", insumoEnDb.Nombre);
                    Assert.Equal(2, insumoEnDb.IdCategoriaInsumo);
                    Assert.False(insumoEnDb.Status);
                }
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreActivo_RetornaUnoSiInsumoExisteYActivo()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoActivoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                }

                int resultado = InsumoDAO.ValidarInsumoPorNombreActivo("InsumoActivoTest");
                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreActivo_RetornaCeroSiInsumoNoExiste()
        {
            using (var scope = new TransactionScope())
            {
                int resultado = InsumoDAO.ValidarInsumoPorNombreActivo("InsumoInexistenteTest");
                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreActivo_RetornaCeroSiInsumoEstaInactivo()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoInactivoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = false
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                }

                int resultado = InsumoDAO.ValidarInsumoPorNombreActivo("InsumoInactivoTest");
                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreDesactivado_RetornaUnoSiInsumoExisteYEstaInactivo()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoInactivoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = false
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                }

                int resultado = InsumoDAO.ValidarInsumoPorNombreDesactivado("InsumoInactivoTest");
                Assert.Equal(1, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreDesactivado_RetornaCeroSiInsumoNoExiste()
        {
            using (var scope = new TransactionScope())
            {
                int resultado = InsumoDAO.ValidarInsumoPorNombreDesactivado("InsumoInexistenteTest");
                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoPorNombreDesactivado_RetornaCeroSiInsumoExistePeroEstaActivo()
        {
            using (var scope = new TransactionScope())
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoActivoTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                }

                int resultado = InsumoDAO.ValidarInsumoPorNombreDesactivado("InsumoActivoTest");
                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void ValidarInsumoNoRegistradoEnReceta_RetornaUnoSiInsumoEstaRegistrado()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;
                int idReceta;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var receta = new Receta
                    {
                        IdReceta = 1,
                        Instrucciones = "instrucciones pruebas"
                    };
                    context.Recetas.Add(receta);
                    context.SaveChanges();
                    idReceta = receta.IdReceta;

                    var insumo = new Insumo
                    {
                        Nombre = "InsumoParaRecetaTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;

                    var insumoParaReceta = new InsumoParaReceta
                    {
                        IdInsumo = idInsumo,
                        IdReceta = idReceta,
                        Cantidad = 2.5f
                    };
                    context.InsumosParaReceta.Add(insumoParaReceta);
                    context.SaveChanges();
                }

                var insumoTest = new Insumo { IdInsumo = idInsumo };
                int resultado = InsumoDAO.ValidarInsumoNoRegistradoEnReceta(insumoTest);
                Assert.Equal(1, resultado);
            }
        }


        [Fact]
        public void ValidarInsumoNoRegistradoEnReceta_RetornaCeroSiInsumoNoEstaRegistrado()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                   
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoNoRegistradoEnRecetaTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;
                }
                var insumoTest = new Insumo { IdInsumo = idInsumo };
                int resultado = InsumoDAO.ValidarInsumoNoRegistradoEnReceta(insumoTest);
                Assert.Equal(0, resultado);
            }
        }

        [Fact]
        public void EliminarInsumo_CambiaStatusAFalse()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "InsumoEliminarTest",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;
                }

                var insumoEliminar = new Insumo { IdInsumo = idInsumo };
                bool resultado = InsumoDAO.EliminarInsumo(insumoEliminar);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoActualizado = context.Insumos.FirstOrDefault(p => p.IdInsumo == idInsumo);
                    Assert.NotNull(insumoActualizado);
                    Assert.False(insumoActualizado.Status);
                }
            }
        }

        [Fact]
        public void ActivarInsumo_CambiaStatusATrue()
        {
            using (var scope = new TransactionScope())
            {
                string nombreInsumo = "InsumoActivarTest";
                int idInsumo;

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = nombreInsumo,
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = false
                    };
                    context.Insumos.Add(insumo);
                    context.SaveChanges();
                    idInsumo = insumo.IdInsumo;
                }

                bool resultado = InsumoDAO.ActivarInsumo(nombreInsumo);

                Assert.True(resultado);

                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumoActualizado = context.Insumos.FirstOrDefault(p => p.IdInsumo == idInsumo);
                    Assert.NotNull(insumoActualizado);
                    Assert.True(insumoActualizado.Status);                }
            }
        }

        [Fact]
        public void BuscarInsumoPorNombre_DevuelveInsumoCorrecto()
        {
            using (var scope = new TransactionScope())
            {
                int idInsumo;
                using (var context = new ItaliasPizzaDbContext())
                {
                    var insumo = new Insumo
                    {
                        Nombre = "BuscarTestInsumo",
                        IdCategoriaInsumo = 1,
                        IdUnidadDeMedida = 1,
                        Status = true
                    };

                    context.Insumos.Add(insumo);
                    context.SaveChanges();

                    idInsumo = insumo.IdInsumo;
                }

                var resultado = InsumoDAO.BuscarInsumoPorNombre("BuscarTest");

                Assert.NotNull(resultado);
                Assert.Equal(idInsumo, resultado.IdInsumo);
                Assert.Equal("BuscarTestInsumo", resultado.Nombre);
            }
        }


    }
}
