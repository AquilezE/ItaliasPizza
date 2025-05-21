namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aniadiVistasySps : DbMigration
    {
        public override void Up()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Migrations\Sql\VistasCrearReportes.sql";
            Sql(System.IO.File.ReadAllText(path));

            path = AppDomain.CurrentDomain.BaseDirectory + @"\Migrations\Sql\SPCrearReportes.sql";
            Sql(System.IO.File.ReadAllText(path));

        }
        
        public override void Down()
        {
            Sql(@"DROP VIEW IF EXISTS dbo.vw01PedidosReportes;
        DROP VIEW IF EXISTS dbo.vw02InventarioReportes;
        DROP VIEW IF EXISTS dbo.vw03PedidoProveedorReportes;");

           
            Sql(@"DROP PROCEDURE IF EXISTS dbo.sp01_GetPedidosReportes;
        DROP PROCEDURE IF EXISTS dbo.sp02_GetInventarioReport;
        DROP PROCEDURE IF EXISTS dbo.sp03_GetPedidosProveedorReport;");
        }
    }
}
