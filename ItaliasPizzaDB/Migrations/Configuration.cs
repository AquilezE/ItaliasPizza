namespace ItaliasPizzaDB.Migrations
{
    using ItaliasPizzaDB.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ItaliasPizzaDB.ItaliasPizzaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ItaliasPizzaDB.ItaliasPizzaDbContext context)
        {
            //Status de Pedido
            var statusEnProceso = new StatusPedido
            {
                IdStatusPedido = 1,
                Status = "Realizado"
            };
            context.StatusPedidos.Add(statusEnProceso);

            var statusPreparando = new StatusPedido
            {
                IdStatusPedido = 2,
                Status = "Preparando"
            };
            context.StatusPedidos.Add(statusPreparando);
            var statusListo = new StatusPedido
            {
                IdStatusPedido = 3,
                Status = "Listo Para Entregar"
            };
            context.StatusPedidos.Add(statusListo);

            var statusEntregado = new StatusPedido
            {
                IdStatusPedido = 4,
                Status = "Entregado"
            };
            context.StatusPedidos.Add(statusEntregado);
            var statusCancelado = new StatusPedido
            {
                IdStatusPedido = 5,
                Status = "Cancelado"
            };
            context.StatusPedidos.Add(statusCancelado);
            var statusNoEntregado = new StatusPedido
            {
                IdStatusPedido = 6,
                Status = "No Entregado"
            };
            context.StatusPedidos.Add(statusNoEntregado);
            var statusEnCamino = new StatusPedido
            {
                IdStatusPedido = 7,
                Status = "En Camino"
            };
            context.StatusPedidos.Add(statusEnCamino);


            //Cargos
            var cargoGerente = new Cargo
            {
                IdCargo = 1,
                NombreCargo = "Gerente"
            };
            context.Cargos.Add(cargoGerente);

            var cargoCajero = new Cargo
            {
                IdCargo = 2,
                NombreCargo = "Cajero"
            };

            context.Cargos.Add(cargoCajero);

            var cargoCocinero = new Cargo
            {
                IdCargo = 3,
                NombreCargo = "Cocinero"
            };

            context.Cargos.Add(cargoCocinero);

            var cargoRepartidor = new Cargo
            {
                IdCargo = 4,
                NombreCargo = "Repartidor"
            };
            context.Cargos.Add(cargoRepartidor);

            var cargoMesero = new Cargo
            {
                IdCargo = 5,
                NombreCargo = "Mesero"
            };
            context.Cargos.Add(cargoMesero);

            //Empleados
            var empleadoGerenteTest = new Empleado
            {
                IdEmpleado = 1,
                Nombre = "TestGerenteUser",
                Apellidos = "Prueba",
                Telefono = "1234567890",
                Status = true,
                IdCargo = cargoGerente.IdCargo
            };
            context.Empleados.Add(empleadoGerenteTest);

            var empleadoCajeroTest = new Empleado
            {
                IdEmpleado = 2,
                Nombre = "TestCajeroUser",
                Apellidos = "Prueba",
                Telefono = "9876543210",
                Status = true,
                IdCargo = cargoCajero.IdCargo
            };
            context.Empleados.Add(empleadoCajeroTest);

            var empleadoCocineroTest = new Empleado
            {
                IdEmpleado = 3,
                Nombre = "TestCocineroUser",
                Apellidos = "Prueba",
                Telefono = "4567891230",
                Status = true,
                IdCargo = cargoCocinero.IdCargo
            };
            context.Empleados.Add(empleadoCocineroTest);

            var empleadoRepartidorTest = new Empleado
            {
                IdEmpleado = 4,
                Nombre = "TestRepartidorUser",
                Apellidos = "Prueba",
                Telefono = "3216549870",
                Status = true,
                IdCargo = cargoRepartidor.IdCargo
            };
            context.Empleados.Add(empleadoRepartidorTest);

            var empleadoMeseroTest = new Empleado
            {
                IdEmpleado = 5,
                Nombre = "TestMeseroUser",
                Apellidos = "Prueba",
                Telefono = "6543217890",
                Status = true,
                IdCargo = cargoMesero.IdCargo
            };
            context.Empleados.Add(empleadoMeseroTest);

            //CuentasAcceso
            var cuentaAccesoGerente = new CuentaAcceso
            {
                IdEmpleado = empleadoGerenteTest.IdEmpleado,
                NombreUsuario = empleadoGerenteTest.Nombre,
                Contraseña = "kekistan"
            };
            context.CuentasAcceso.Add(cuentaAccesoGerente);

            var cuentaAccesoCajero = new CuentaAcceso
            {
                IdEmpleado = empleadoCajeroTest.IdEmpleado,
                NombreUsuario = empleadoCajeroTest.Nombre,
                Contraseña = "kekistan"
            };
            context.CuentasAcceso.Add(cuentaAccesoCajero);
            var cuentaAccesoCocinero = new CuentaAcceso
            {
                IdEmpleado = empleadoCocineroTest.IdEmpleado,
                NombreUsuario = empleadoCocineroTest.Nombre,
                Contraseña = "kekistan"
            };
            context.CuentasAcceso.Add(cuentaAccesoCocinero);
            var cuentaAccesoRepartidor = new CuentaAcceso
            {
                IdEmpleado = empleadoRepartidorTest.IdEmpleado,
                NombreUsuario = empleadoRepartidorTest.Nombre,
                Contraseña = "kekistan"
            };
            context.CuentasAcceso.Add(cuentaAccesoRepartidor);
            var cuentaAccesoMesero = new CuentaAcceso
            {
                IdEmpleado = empleadoMeseroTest.IdEmpleado,
                NombreUsuario = empleadoMeseroTest.Nombre,
                Contraseña = "kekistan"
            };
            context.CuentasAcceso.Add(cuentaAccesoMesero);

            //Unidades de Medida
            var unidadDeMedidaKg = new UnidadDeMedida
            {
                IdUnidadDeMedida = 1,
                UnidadDeMedidaNombre = "Kilogramos"
            };

            context.UnidadesDeMedida.Add(unidadDeMedidaKg);
            var unidadDeMedidaGr = new UnidadDeMedida
            {
                IdUnidadDeMedida = 2,
                UnidadDeMedidaNombre = "Gramos"
            };
            context.UnidadesDeMedida.Add(unidadDeMedidaGr);
            var unidadDeMedidaLt = new UnidadDeMedida
            {
                IdUnidadDeMedida = 3,
                UnidadDeMedidaNombre = "Litros"
            };
            context.UnidadesDeMedida.Add(unidadDeMedidaLt);
            var unidadDeMedidaMl = new UnidadDeMedida
            {
                IdUnidadDeMedida = 4,
                UnidadDeMedidaNombre = "Mililitros"
            };
            context.UnidadesDeMedida.Add(unidadDeMedidaMl);

            //Categorias de Insumos
            var categoriaInsumoCarnes = new CategoriaInsumo
            {
                IdCategoriaInsumo = 1,
                CategoriaInsumoNombre = "Carnes"
            };
            context.CategoriasInsumo.Add(categoriaInsumoCarnes);
            var categoriaInsumoLacteos = new CategoriaInsumo
            {
                IdCategoriaInsumo = 2,
                CategoriaInsumoNombre = "Lácteos"
            };
            context.CategoriasInsumo.Add(categoriaInsumoLacteos);
            var categoriaInsumoVegetales = new CategoriaInsumo
            {
                IdCategoriaInsumo = 3,
                CategoriaInsumoNombre = "Vegetales"
            };
            context.CategoriasInsumo.Add(categoriaInsumoVegetales);


            //Categorias de Productos

            var categoriaProductoPizza = new CategoriaProducto
            {
                IdCategoriaProducto = 1,
                CategoriaProductoNombre = "Pizza"
            };
            context.CategoriasProducto.Add(categoriaProductoPizza);

            var categoriaProductoBebidas = new CategoriaProducto
            {
                IdCategoriaProducto = 2,
                CategoriaProductoNombre = "Bebida"
            };
            context.CategoriasProducto.Add(categoriaProductoBebidas);

            var categoriaProductoPapas = new CategoriaProducto
            {
                IdCategoriaProducto = 3,
                CategoriaProductoNombre = "Papas"
            };
            context.CategoriasProducto.Add(categoriaProductoPapas);



            // Insumos de prueba
            var insumoCarne = new Insumo
            {
                IdInsumo = 9,
                Nombre = "Carne molida",
                Status = true,
                Precio = 100,
                IdCategoriaInsumo = categoriaInsumoCarnes.IdCategoriaInsumo,
                IdUnidadDeMedida = unidadDeMedidaKg.IdUnidadDeMedida
            };
            context.Insumos.Add(insumoCarne);
            var insumoPepperoni = new Insumo
            {
                IdInsumo = 12,
                Nombre = "Pepperoni",
                Status = true,
                Precio = 70,
                IdCategoriaInsumo = categoriaInsumoCarnes.IdCategoriaInsumo,
                IdUnidadDeMedida = unidadDeMedidaKg.IdUnidadDeMedida
            };
            context.Insumos.Add(insumoPepperoni);
            var insumoQueso = new Insumo
            {
                IdInsumo = 10,
                Nombre = "Queso mozzarella",
                Status = true,
                Precio = 1,
                IdCategoriaInsumo = categoriaInsumoLacteos.IdCategoriaInsumo,
                IdUnidadDeMedida = unidadDeMedidaGr.IdUnidadDeMedida
            };
            context.Insumos.Add(insumoQueso);

            var insumoTomate = new Insumo
            {
                IdInsumo = 11,
                Nombre = "Tomate fresco",
                Status = true,
                Precio = 50,
                IdCategoriaInsumo = categoriaInsumoVegetales.IdCategoriaInsumo,
                IdUnidadDeMedida = unidadDeMedidaKg.IdUnidadDeMedida
            };
            context.Insumos.Add(insumoTomate);

            // Proveedores de prueba
            var proveedorUno = new Proveedor
            {
                IdProveedor = 1,
                Nombre = "Distribuidora Carnes del Norte",
                Telefono = "123456789",
                Direccion = "Av. Norte 123"
            };
            context.Proveedores.Add(proveedorUno);

            var proveedorDos = new Proveedor
            {
                IdProveedor = 2,
                Nombre = "Lácteos del Sur",
                Telefono = "987654321",
                Direccion = "Calle Sur 456"
            };
            context.Proveedores.Add(proveedorDos);

            var proveedorTres = new Proveedor
            {
                IdProveedor = 3,
                Nombre = "Hortalizas de Puebla",
                Telefono = "456789123",
                Direccion = "Camino Central 789"
            };
            context.Proveedores.Add(proveedorTres);

            // Relación Proveedor-Insumo
            var proveedorInsumoCarne = new ProveedorInsumo
            {
                IdProveedor = proveedorUno.IdProveedor,
                IdInsumo = insumoCarne.IdInsumo
            };
            context.ProveedoresInsumos.Add(proveedorInsumoCarne);

            var proveedorInsumoQueso = new ProveedorInsumo
            {
                IdProveedor = proveedorDos.IdProveedor,
                IdInsumo = insumoQueso.IdInsumo
            };
            context.ProveedoresInsumos.Add(proveedorInsumoQueso);

            var proveedorInsumoTomate = new ProveedorInsumo
            {
                IdProveedor = proveedorTres.IdProveedor,
                IdInsumo = insumoTomate.IdInsumo
            };
            context.ProveedoresInsumos.Add(proveedorInsumoTomate);

            context.SaveChanges();
            base.Seed(context);



        }

    }
}