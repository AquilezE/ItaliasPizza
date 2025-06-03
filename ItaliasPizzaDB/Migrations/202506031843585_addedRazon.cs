namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRazon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidoes", "RazonNoEntregado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedidoes", "RazonNoEntregado");
        }
    }
}
