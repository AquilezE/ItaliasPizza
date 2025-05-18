namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recetaModificacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InsumoParaRecetas", "IdReceta", "dbo.Recetas");
            DropForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas");
            DropIndex("dbo.Productoes", new[] { "IdReceta" });
            DropPrimaryKey("dbo.Recetas");
            AlterColumn("dbo.Recetas", "IdReceta", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Recetas", "IdReceta");
            CreateIndex("dbo.Recetas", "IdReceta");
            AddForeignKey("dbo.InsumoParaRecetas", "IdReceta", "dbo.Recetas", "IdReceta", cascadeDelete: true);
            DropColumn("dbo.Productoes", "IdReceta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productoes", "IdReceta", c => c.Int());
            DropForeignKey("dbo.InsumoParaRecetas", "IdReceta", "dbo.Recetas");
            DropIndex("dbo.Recetas", new[] { "IdReceta" });
            DropPrimaryKey("dbo.Recetas");
            AlterColumn("dbo.Recetas", "IdReceta", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Recetas", "IdReceta");
            CreateIndex("dbo.Productoes", "IdReceta");
            AddForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas", "IdReceta");
            AddForeignKey("dbo.InsumoParaRecetas", "IdReceta", "dbo.Recetas", "IdReceta", cascadeDelete: true);
        }
    }
}
