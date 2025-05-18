namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productoCambios : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas");
            DropIndex("dbo.Productoes", new[] { "IdReceta" });
            AddColumn("dbo.Productoes", "Nombre", c => c.String());
            AddColumn("dbo.Productoes", "Categoria", c => c.String());
            AddColumn("dbo.Productoes", "Codigo", c => c.String());
            AddColumn("dbo.Productoes", "Restricciones", c => c.String());
            AddColumn("dbo.Productoes", "Descripcion", c => c.String());
            AddColumn("dbo.Productoes", "ImagenRuta", c => c.String());
            AlterColumn("dbo.Productoes", "IdReceta", c => c.Int());
            CreateIndex("dbo.Productoes", "IdReceta");
            AddForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas", "IdReceta");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas");
            DropIndex("dbo.Productoes", new[] { "IdReceta" });
            AlterColumn("dbo.Productoes", "IdReceta", c => c.Int(nullable: false));
            DropColumn("dbo.Productoes", "ImagenRuta");
            DropColumn("dbo.Productoes", "Descripcion");
            DropColumn("dbo.Productoes", "Restricciones");
            DropColumn("dbo.Productoes", "Codigo");
            DropColumn("dbo.Productoes", "Categoria");
            DropColumn("dbo.Productoes", "Nombre");
            CreateIndex("dbo.Productoes", "IdReceta");
            AddForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas", "IdReceta", cascadeDelete: true);
        }
    }
}
