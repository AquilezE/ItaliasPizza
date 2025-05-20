namespace ItaliasPizzaDB.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CantidadAInsumo : DbMigration
    {
        public override void Up()
        {
            // Cambia el tipo a REAL (equivalente a float en SQL Server)
            AlterColumn("dbo.Insumoes", "Cantidad", c => c.Single(nullable: false, storeType: "real"));

            // O si prefieres usar decimal (recomendado para cantidades):
            // AlterColumn("dbo.Insumoes", "Cantidad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            // Revertir el cambio
            AlterColumn("dbo.Insumoes", "Cantidad", c => c.Single(nullable: false));
        }
    }
}
