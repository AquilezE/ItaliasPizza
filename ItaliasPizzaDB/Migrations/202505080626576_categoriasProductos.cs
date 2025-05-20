namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoriasProductos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaProductoes",
                c => new
                    {
                        IdCategoriaProducto = c.Int(nullable: false, identity: true),
                        CategoriaProductoNombre = c.String(),
                    })
                .PrimaryKey(t => t.IdCategoriaProducto);
            
            AddColumn("dbo.Productoes", "IdCategoriaProducto", c => c.Int(nullable: false));
            AddColumn("dbo.Insumoes", "Precio", c => c.Single(nullable: false));
            CreateIndex("dbo.Productoes", "IdCategoriaProducto");
            AddForeignKey("dbo.Productoes", "IdCategoriaProducto", "dbo.CategoriaProductoes", "IdCategoriaProducto", cascadeDelete: true);
            DropColumn("dbo.Insumoes", "Cantidad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Insumoes", "Cantidad", c => c.Single(nullable: false));
            DropForeignKey("dbo.Productoes", "IdCategoriaProducto", "dbo.CategoriaProductoes");
            DropIndex("dbo.Productoes", new[] { "IdCategoriaProducto" });
            DropColumn("dbo.Insumoes", "Precio");
            DropColumn("dbo.Productoes", "IdCategoriaProducto");
            DropTable("dbo.CategoriaProductoes");
        }
    }
}
