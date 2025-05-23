using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using Xunit;

namespace DatabaseTests
{
    public class CategoriaInsumoDAOTests
    {
        [Fact]
        public void ObtenerCategoriasInsumo_DebeRetornarListaDeCategoriasInsumo()
        {
           

            var categoriasObtenidas = CategoriaInsumoDAO.ObtenerCategoriasInsumo();

            Assert.NotEmpty(categoriasObtenidas);
            Assert.Equal(6, categoriasObtenidas.Count);
        }
    }
}
