namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioCategoriaProductoForaneo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Productoes", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productoes", "Categoria", c => c.String());
        }
    }
}
