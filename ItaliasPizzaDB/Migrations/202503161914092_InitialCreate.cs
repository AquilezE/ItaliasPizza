namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CuentaAccesoes", "IdEmpleado");
            RenameColumn(table: "dbo.CuentaAccesoes", name: "IdCuentaAcceso", newName: "IdEmpleado");
            RenameIndex(table: "dbo.CuentaAccesoes", name: "IX_IdCuentaAcceso", newName: "IX_IdEmpleado");
            DropColumn("dbo.Empleadoes", "IdCuentaAcceso");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Empleadoes", "IdCuentaAcceso", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.CuentaAccesoes", name: "IX_IdEmpleado", newName: "IX_IdCuentaAcceso");
            RenameColumn(table: "dbo.CuentaAccesoes", name: "IdEmpleado", newName: "IdCuentaAcceso");
            AddColumn("dbo.CuentaAccesoes", "IdEmpleado", c => c.Int(nullable: false));
        }
    }
}
