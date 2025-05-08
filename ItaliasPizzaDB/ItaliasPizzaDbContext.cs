using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB
{
    public class ItaliasPizzaDbContext : DbContext
    {

        public ItaliasPizzaDbContext()
            : base("ItaliasPizzaDbContext") // Name of the connection string
        {
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<CuentaAcceso> CuentasAcceso { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoParaLocal> PedidosParaLocal { get; set; }
        public DbSet<PedidoParaLlevar> PedidosParaLlevar { get; set; }
        public DbSet<StatusPedido> StatusPedidos { get; set; }

        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<InsumoParaReceta> InsumosParaReceta { get; set; }

        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; }
        public DbSet<CategoriaInsumo> CategoriasInsumo { get; set; }
        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProveedorInsumo> ProveedoresInsumos { get; set; }

        public DbSet<PedidoProveedor> PedidosProveedores { get; set; }
        public DbSet<DetallePedidoProveedor> DetallesPedidoProveedor { get; set; }

        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<TipoTransaccion> TiposTransaccion { get; set; }
        public DbSet<CorteDeCaja> CortesDeCaja { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProveedorInsumo>()
                .HasKey(pi => new { pi.IdProveedor, pi.IdInsumo });


            modelBuilder.Entity<Empleado>()
                .HasOptional(e => e.CuentaAcceso)
                .WithRequired(ca => ca.Empleado);

            base.OnModelCreating(modelBuilder);
        }
    }
}
