namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCantidadType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VistaInsumosPorProveedor",
                c => new
                    {
                        IdInsumo = c.Int(nullable: false, identity: true),
                        IdProveedor = c.Int(nullable: false),
                        NombreProveedor = c.String(),
                        NombreInsumo = c.String(),
                        Precio = c.Single(nullable: false),
                        Unidad = c.String(),
                        IdCategoriaInsumo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdInsumo);
            
            AddColumn("dbo.Insumoes", "Cantidad", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Insumoes", "Cantidad");
            DropTable("dbo.VistaInsumosPorProveedor");
        }
    }
}
