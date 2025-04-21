using ItaliasPizzaDB.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DatabaseTests
{
    public class UnidadDeMedidaDAOTests
    {
        [Fact]
        public void ObtenerUnidadDeMedida_DebeRetornarListaDeCategoriasInsumo()
        {
            //Aqui usamos las que se generaron en el inicializador
            //Kilogramo, Gramo, Litro, Mililitro

            var unidadesObtenidas = UnidadDeMedidaDAO.ObtenerUnidadesDeMedida();

            Assert.NotEmpty(unidadesObtenidas);
            Assert.Equal(4, unidadesObtenidas.Count);

        }
    }
}
