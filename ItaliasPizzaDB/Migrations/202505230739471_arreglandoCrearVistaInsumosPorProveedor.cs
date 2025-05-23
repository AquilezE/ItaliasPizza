namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arreglandoCrearVistaInsumosPorProveedor : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    CREATE VIEW VistaInsumosPorProveedor AS
SELECT 
    i.IdInsumo,
    p.IdProveedor,
    p.Nombre AS NombreProveedor,
    i.Nombre AS NombreInsumo,
    i.Precio,
    u.UnidadDeMedidaNombre AS Unidad,
    i.IdCategoriaInsumo
FROM Proveedors p
JOIN ProveedorInsumoes pi ON pi.IdProveedor = p.IdProveedor
JOIN Insumoes i ON i.IdInsumo = pi.IdInsumo
JOIN UnidadDeMedidas u ON i.IdUnidadDeMedida = u.IdUnidadDeMedida;
    ");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW VistaInsumosPorProveedor");
        }
    }
}
