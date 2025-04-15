namespace ItaliasPizzaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cargoes",
                c => new
                    {
                        IdCargo = c.Int(nullable: false, identity: true),
                        NombreCargo = c.String(),
                    })
                .PrimaryKey(t => t.IdCargo);
            
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        IdEmpleado = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellidos = c.String(),
                        Telefono = c.String(),
                        Status = c.Boolean(nullable: false),
                        IdCargo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdEmpleado)
                .ForeignKey("dbo.Cargoes", t => t.IdCargo, cascadeDelete: true)
                .Index(t => t.IdCargo);
            
            CreateTable(
                "dbo.CorteDeCajas",
                c => new
                    {
                        IdCorteDeCaja = c.Int(nullable: false, identity: true),
                        FechaApertura = c.DateTime(nullable: false),
                        FechaCierre = c.DateTime(nullable: false),
                        Cambio = c.Single(nullable: false),
                        VentaDelDia = c.Single(nullable: false),
                        Gasto = c.Single(nullable: false),
                        IdEmpleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCorteDeCaja)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado, cascadeDelete: true)
                .Index(t => t.IdEmpleado);
            
            CreateTable(
                "dbo.CuentaAccesoes",
                c => new
                    {
                        IdEmpleado = c.Int(nullable: false),
                        NombreUsuario = c.String(),
                        Contraseña = c.String(),
                    })
                .PrimaryKey(t => t.IdEmpleado)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado)
                .Index(t => t.IdEmpleado);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Single(nullable: false),
                        IdEmpleado = c.Int(nullable: false),
                        IdStatusPedido = c.Int(nullable: false),
                        IdDireccion = c.Int(),
                        IdCliente = c.Int(),
                        Mesa = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IdPedido)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado, cascadeDelete: true)
                .ForeignKey("dbo.StatusPedidoes", t => t.IdStatusPedido, cascadeDelete: true)
                .ForeignKey("dbo.Direccions", t => t.IdDireccion)
                .ForeignKey("dbo.Clientes", t => t.IdCliente)
                .Index(t => t.IdEmpleado)
                .Index(t => t.IdStatusPedido)
                .Index(t => t.IdDireccion)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.DetallePedidoes",
                c => new
                    {
                        IdDetallePedido = c.Int(nullable: false, identity: true),
                        IdPedido = c.Int(nullable: false),
                        IdProducto = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Subtotal = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdDetallePedido)
                .ForeignKey("dbo.Pedidoes", t => t.IdPedido, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.IdProducto, cascadeDelete: true)
                .Index(t => t.IdPedido)
                .Index(t => t.IdProducto);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Precio = c.Single(nullable: false),
                        Status = c.Boolean(nullable: false),
                        MaxPerOrder = c.Int(nullable: false),
                        IdReceta = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto)
                .ForeignKey("dbo.Recetas", t => t.IdReceta, cascadeDelete: true)
                .Index(t => t.IdReceta);
            
            CreateTable(
                "dbo.Recetas",
                c => new
                    {
                        IdReceta = c.Int(nullable: false, identity: true),
                        Instrucciones = c.String(),
                    })
                .PrimaryKey(t => t.IdReceta);
            
            CreateTable(
                "dbo.InsumoParaRecetas",
                c => new
                    {
                        IdInsumoParaReceta = c.Int(nullable: false, identity: true),
                        Cantidad = c.Single(nullable: false),
                        IdReceta = c.Int(nullable: false),
                        IdInsumo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdInsumoParaReceta)
                .ForeignKey("dbo.Insumoes", t => t.IdInsumo, cascadeDelete: true)
                .ForeignKey("dbo.Recetas", t => t.IdReceta, cascadeDelete: true)
                .Index(t => t.IdReceta)
                .Index(t => t.IdInsumo);
            
            CreateTable(
                "dbo.Insumoes",
                c => new
                    {
                        IdInsumo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Status = c.Boolean(nullable: false),
                        IdCategoriaInsumo = c.Int(nullable: false),
                        IdUnidadDeMedida = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdInsumo)
                .ForeignKey("dbo.CategoriaInsumoes", t => t.IdCategoriaInsumo, cascadeDelete: true)
                .ForeignKey("dbo.UnidadDeMedidas", t => t.IdUnidadDeMedida, cascadeDelete: true)
                .Index(t => t.IdCategoriaInsumo)
                .Index(t => t.IdUnidadDeMedida);
            
            CreateTable(
                "dbo.CategoriaInsumoes",
                c => new
                    {
                        IdCategoriaInsumo = c.Int(nullable: false, identity: true),
                        CategoriaInsumoNombre = c.String(),
                    })
                .PrimaryKey(t => t.IdCategoriaInsumo);
            
            CreateTable(
                "dbo.ProveedorInsumoes",
                c => new
                    {
                        IdProveedor = c.Int(nullable: false),
                        IdInsumo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdProveedor, t.IdInsumo })
                .ForeignKey("dbo.Insumoes", t => t.IdInsumo, cascadeDelete: true)
                .ForeignKey("dbo.Proveedors", t => t.IdProveedor, cascadeDelete: true)
                .Index(t => t.IdProveedor)
                .Index(t => t.IdInsumo);
            
            CreateTable(
                "dbo.Proveedors",
                c => new
                    {
                        IdProveedor = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Telefono = c.String(),
                        Direccion = c.String(),
                    })
                .PrimaryKey(t => t.IdProveedor);
            
            CreateTable(
                "dbo.PedidoProveedors",
                c => new
                    {
                        IdPedidoProveedor = c.Int(nullable: false, identity: true),
                        FechaPedido = c.DateTime(nullable: false),
                        Total = c.Single(nullable: false),
                        Status = c.Byte(nullable: false),
                        IdProveedor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPedidoProveedor)
                .ForeignKey("dbo.Proveedors", t => t.IdProveedor, cascadeDelete: true)
                .Index(t => t.IdProveedor);
            
            CreateTable(
                "dbo.DetallePedidoProveedors",
                c => new
                    {
                        IdDetallePedidoProveedor = c.Int(nullable: false, identity: true),
                        Cantidad = c.Single(nullable: false),
                        PrecioUnitario = c.Single(nullable: false),
                        IdPedidoProveedor = c.Int(nullable: false),
                        IdInsumo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDetallePedidoProveedor)
                .ForeignKey("dbo.Insumoes", t => t.IdInsumo, cascadeDelete: true)
                .ForeignKey("dbo.PedidoProveedors", t => t.IdPedidoProveedor, cascadeDelete: true)
                .Index(t => t.IdPedidoProveedor)
                .Index(t => t.IdInsumo);
            
            CreateTable(
                "dbo.UnidadDeMedidas",
                c => new
                    {
                        IdUnidadDeMedida = c.Int(nullable: false, identity: true),
                        UnidadDeMedidaNombre = c.String(),
                    })
                .PrimaryKey(t => t.IdUnidadDeMedida);
            
            CreateTable(
                "dbo.StatusPedidoes",
                c => new
                    {
                        IdStatusPedido = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.IdStatusPedido);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellidos = c.String(),
                        Telefono = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Direccions",
                c => new
                    {
                        IdDireccion = c.Int(nullable: false, identity: true),
                        Calle = c.String(),
                        Numero = c.Int(nullable: false),
                        CodigoPostal = c.String(),
                        Colonia = c.String(),
                        Ciudad = c.String(),
                        Estado = c.String(),
                        Status = c.Boolean(nullable: false),
                        Referencia = c.String(),
                        IdCliente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDireccion)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.Transaccions",
                c => new
                    {
                        IdTransaccion = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Single(nullable: false),
                        Descripcion = c.String(),
                        IdTipoTransaccion = c.Int(nullable: false),
                        IdEmpleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTransaccion)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado, cascadeDelete: true)
                .ForeignKey("dbo.TipoTransaccions", t => t.IdTipoTransaccion, cascadeDelete: true)
                .Index(t => t.IdTipoTransaccion)
                .Index(t => t.IdEmpleado);
            
            CreateTable(
                "dbo.TipoTransaccions",
                c => new
                    {
                        IdTipoTransaccion = c.Int(nullable: false, identity: true),
                        TipoTransaccionesNombre = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoTransaccion);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaccions", "IdTipoTransaccion", "dbo.TipoTransaccions");
            DropForeignKey("dbo.Transaccions", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Pedidoes", "IdCliente", "dbo.Clientes");
            DropForeignKey("dbo.Pedidoes", "IdDireccion", "dbo.Direccions");
            DropForeignKey("dbo.Direccions", "IdCliente", "dbo.Clientes");
            DropForeignKey("dbo.Pedidoes", "IdStatusPedido", "dbo.StatusPedidoes");
            DropForeignKey("dbo.Pedidoes", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Productoes", "IdReceta", "dbo.Recetas");
            DropForeignKey("dbo.InsumoParaRecetas", "IdReceta", "dbo.Recetas");
            DropForeignKey("dbo.Insumoes", "IdUnidadDeMedida", "dbo.UnidadDeMedidas");
            DropForeignKey("dbo.ProveedorInsumoes", "IdProveedor", "dbo.Proveedors");
            DropForeignKey("dbo.DetallePedidoProveedors", "IdPedidoProveedor", "dbo.PedidoProveedors");
            DropForeignKey("dbo.DetallePedidoProveedors", "IdInsumo", "dbo.Insumoes");
            DropForeignKey("dbo.PedidoProveedors", "IdProveedor", "dbo.Proveedors");
            DropForeignKey("dbo.ProveedorInsumoes", "IdInsumo", "dbo.Insumoes");
            DropForeignKey("dbo.InsumoParaRecetas", "IdInsumo", "dbo.Insumoes");
            DropForeignKey("dbo.Insumoes", "IdCategoriaInsumo", "dbo.CategoriaInsumoes");
            DropForeignKey("dbo.DetallePedidoes", "IdProducto", "dbo.Productoes");
            DropForeignKey("dbo.DetallePedidoes", "IdPedido", "dbo.Pedidoes");
            DropForeignKey("dbo.CuentaAccesoes", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.CorteDeCajas", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Empleadoes", "IdCargo", "dbo.Cargoes");
            DropIndex("dbo.Transaccions", new[] { "IdEmpleado" });
            DropIndex("dbo.Transaccions", new[] { "IdTipoTransaccion" });
            DropIndex("dbo.Direccions", new[] { "IdCliente" });
            DropIndex("dbo.DetallePedidoProveedors", new[] { "IdInsumo" });
            DropIndex("dbo.DetallePedidoProveedors", new[] { "IdPedidoProveedor" });
            DropIndex("dbo.PedidoProveedors", new[] { "IdProveedor" });
            DropIndex("dbo.ProveedorInsumoes", new[] { "IdInsumo" });
            DropIndex("dbo.ProveedorInsumoes", new[] { "IdProveedor" });
            DropIndex("dbo.Insumoes", new[] { "IdUnidadDeMedida" });
            DropIndex("dbo.Insumoes", new[] { "IdCategoriaInsumo" });
            DropIndex("dbo.InsumoParaRecetas", new[] { "IdInsumo" });
            DropIndex("dbo.InsumoParaRecetas", new[] { "IdReceta" });
            DropIndex("dbo.Productoes", new[] { "IdReceta" });
            DropIndex("dbo.DetallePedidoes", new[] { "IdProducto" });
            DropIndex("dbo.DetallePedidoes", new[] { "IdPedido" });
            DropIndex("dbo.Pedidoes", new[] { "IdCliente" });
            DropIndex("dbo.Pedidoes", new[] { "IdDireccion" });
            DropIndex("dbo.Pedidoes", new[] { "IdStatusPedido" });
            DropIndex("dbo.Pedidoes", new[] { "IdEmpleado" });
            DropIndex("dbo.CuentaAccesoes", new[] { "IdEmpleado" });
            DropIndex("dbo.CorteDeCajas", new[] { "IdEmpleado" });
            DropIndex("dbo.Empleadoes", new[] { "IdCargo" });
            DropTable("dbo.TipoTransaccions");
            DropTable("dbo.Transaccions");
            DropTable("dbo.Direccions");
            DropTable("dbo.Clientes");
            DropTable("dbo.StatusPedidoes");
            DropTable("dbo.UnidadDeMedidas");
            DropTable("dbo.DetallePedidoProveedors");
            DropTable("dbo.PedidoProveedors");
            DropTable("dbo.Proveedors");
            DropTable("dbo.ProveedorInsumoes");
            DropTable("dbo.CategoriaInsumoes");
            DropTable("dbo.Insumoes");
            DropTable("dbo.InsumoParaRecetas");
            DropTable("dbo.Recetas");
            DropTable("dbo.Productoes");
            DropTable("dbo.DetallePedidoes");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.CuentaAccesoes");
            DropTable("dbo.CorteDeCajas");
            DropTable("dbo.Empleadoes");
            DropTable("dbo.Cargoes");
        }
    }
}
