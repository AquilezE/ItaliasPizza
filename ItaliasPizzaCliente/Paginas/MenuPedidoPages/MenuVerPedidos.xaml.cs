using ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio;
using ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Local;
using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Interaction logic for MenuVerPedidos.xaml
    /// </summary>
    public partial class MenuVerPedidos : Page
    {

        public ObservableCollection<Pedido> Pedidos { get; set; } = new ObservableCollection<Pedido>();

        public MenuVerPedidos()
        {
            InitializeComponent();
            this.DataContext = this;
            CargarPedidos(UsuarioSingleton.Instance.NombreCargo);

        }

        private void PedidoPreview_VerDetallesClicked(object sender, EventArgs e)
        {
            var pedido = (Pedido)((PedidoPreview)sender).DataContext;
            Page page;


            if (pedido is PedidoParaLocal local)
            {
                switch (local.IdStatusPedido)
                {
                    case 1:
                        page = new LocalRealizadoPage(local);
                        break;
                    case 2:
                        page = new LocalPreparando(local);
                        break;
                    case 3:
                        page = new LocalListoParaEntrega(local);
                        break;
                    default:
                        throw new InvalidOperationException("Estado local desconocido.");
                }
            }
            else if (pedido is PedidoParaLlevar dom)
            {
                switch (dom.IdStatusPedido)
                {
                    case 1:
                        page = new DomicilioRealizado(dom);
                        break;
                    case 2:
                        page = new DomicilioPreparado(dom);
                        break;
                    case 3:
                        page = new DomicilioListoParaEntrega(dom);
                        break;
                    case 4:
                        page = new DomicilioEnCamino(dom);
                        break;
                    default:
                        throw new InvalidOperationException("Estado domicilio desconocido.");
                }
            }
            else
            {
                throw new InvalidOperationException("Tipo de pedido desconocido.");
            }

            NavigationService?.Navigate(page);

        }

        private void CargarPedidos(string instanceNombreCargo)
        {
            var list = new List<Pedido>();

            // Primero, crea algunos clientes y direcciones “hardcodeados”
            var cliente10 = new Cliente
            {
                IdCliente = 10,
                Nombre = "Juan",
                Apellidos = "Pérez",
                Telefono = "555-1234",
                Status = true,
                Direcciones = new List<Direccion>(),
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            var direccion2 = new Direccion
            {
                IdDireccion = 2,
                Calle = "Av. Siempre Viva",
                Numero = 742,
                CodigoPostal = "12345",
                Colonia = "Centro",
                Ciudad = "Springfield",
                Estado = "Estado X",
                Status = true,
                Referencia = "Frente al parque",
                Cliente = cliente10,
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            cliente10.Direcciones.Add(direccion2);

            var cliente11 = new Cliente
            {
                IdCliente = 11,
                Nombre = "María",
                Apellidos = "Gómez",
                Telefono = "555-5678",
                Status = true,
                Direcciones = new List<Direccion>(),
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            var direccion3 = new Direccion
            {
                IdDireccion = 3,
                Calle = "Calle Falsa",
                Numero = 123,
                CodigoPostal = "54321",
                Colonia = "Las Lomas",
                Ciudad = "Metropolis",
                Estado = "Estado Y",
                Status = true,
                Referencia = "Al lado de la panadería",
                Cliente = cliente11,
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            cliente11.Direcciones.Add(direccion3);

            var cliente12 = new Cliente
            {
                IdCliente = 12,
                Nombre = "Luis",
                Apellidos = "Martínez",
                Telefono = "555-9012",
                Status = true,
                Direcciones = new List<Direccion>(),
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            var direccion4 = new Direccion
            {
                IdDireccion = 4,
                Calle = "Boulevard del Sol",
                Numero = 456,
                CodigoPostal = "67890",
                Colonia = "Vista Alegre",
                Ciudad = "Gotham",
                Estado = "Estado Z",
                Status = true,
                Referencia = "Detrás de la gasolinera",
                Cliente = cliente12,
                PedidosParaLlevar = new List<PedidoParaLlevar>()
            };
            cliente12.Direcciones.Add(direccion4);


            // Locales
            list.Add(new PedidoParaLocal
            {
                IdPedido = 101,
                Total = 150f,
                Fecha = DateTime.Now.AddMinutes(-45),
                IdEmpleado = 1,
                IdStatusPedido = (int)StatusPedidoEnum.Realizado,
                Mesa = 3
            });
            list.Add(new PedidoParaLocal
            {
                IdPedido = 102,
                Total = 80f,
                Fecha = DateTime.Now.AddMinutes(-30),
                IdEmpleado = 2,
                IdStatusPedido = (int)StatusPedidoEnum.Preparando,
                Mesa = 5
            });
            list.Add(new PedidoParaLocal
            {
                IdPedido = 103,
                Total = 120f,
                Fecha = DateTime.Now.AddMinutes(-15),
                IdEmpleado = 3,
                IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega,
                Mesa = 1
            });

            // Ahora crea tus pedidos y asígnales los nav‐props:
            list.Add(new PedidoParaLlevar
            {
                IdPedido = 201,
                Total = 60f,
                Fecha = DateTime.Now.AddHours(-1),
                IdEmpleado = 1,
                IdStatusPedido = (int)StatusPedidoEnum.ListoParaEntrega,
                IdCliente = cliente10.IdCliente,
                IdDireccion = direccion2.IdDireccion,
                Cliente = cliente10,
                Direccion = direccion2
            });

            list.Add(new PedidoParaLlevar
            {
                IdPedido = 202,
                Total = 90f,
                Fecha = DateTime.Now.AddHours(-2),
                IdEmpleado = 2,
                IdStatusPedido = (int)StatusPedidoEnum.EnCamino,
                IdCliente = cliente11.IdCliente,
                IdDireccion = direccion3.IdDireccion,
                Cliente = cliente11,
                Direccion = direccion3
            });

            list.Add(new PedidoParaLlevar
            {
                IdPedido = 203,
                Total = 45f,
                Fecha = DateTime.Now.AddMinutes(-50),
                IdEmpleado = 3,
                IdStatusPedido = (int)StatusPedidoEnum.Cancelado,
                IdCliente = cliente12.IdCliente,
                IdDireccion = direccion4.IdDireccion,
                Cliente = cliente12,
                Direccion = direccion4
            });

            foreach (var p in list)
                Pedidos.Add(p);
        }


    }
}
