namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMerma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mermas",
                c => new
                    {
                        IdMerma = c.Int(nullable: false, identity: true),
                        IdInsumo = c.Int(nullable: false),
                        Cantidad = c.Single(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdMerma)
                .ForeignKey("dbo.Insumoes", t => t.IdInsumo, cascadeDelete: true)
                .Index(t => t.IdInsumo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mermas", "IdInsumo", "dbo.Insumoes");
            DropIndex("dbo.Mermas", new[] { "IdInsumo" });
            DropTable("dbo.Mermas");
        }
    }
}
