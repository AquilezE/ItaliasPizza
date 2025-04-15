using ItaliasPizzaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliasPizzaDB.Models;

namespace DatabaseTests.Inicializador
{
    public class InitializadorTestDb: DropCreateDatabaseAlways<ItaliasPizzaDbContext>
    {
        protected override void Seed(ItaliasPizzaDbContext context)
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

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
