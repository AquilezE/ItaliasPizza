using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseTests.Inicializador;
using ItaliasPizzaDB;

namespace DatabaseTests
{
    public class DataBaseFixture
    {
        public DataBaseFixture() {

            Database.SetInitializer(new InitializadorTestDb());

            using (var context = new ItaliasPizzaDbContext())
            {
                context.Database.Initialize(true);
            }   
        
        }

    }
}
