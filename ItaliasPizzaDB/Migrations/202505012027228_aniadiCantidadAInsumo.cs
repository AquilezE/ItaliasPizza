namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aniadiCantidadAInsumo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Insumoes", "Cantidad", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Insumoes", "Cantidad");
        }
    }
}
