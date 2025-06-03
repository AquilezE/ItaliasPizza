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
using System.Windows.Threading;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Interaction logic for MenuVerPedidos.xaml
    /// </summary>
    public partial class MenuVerPedidos : Page
    {

        public ObservableCollection<Pedido> Pedidos { get; set; } = new ObservableCollection<Pedido>();

        private readonly DispatcherTimer _pollTimer;

        public MenuVerPedidos()
        {
            InitializeComponent();
            this.DataContext = this;
            CargarPedidos();


            _pollTimer = new DispatcherTimer();
            _pollTimer.Interval = TimeSpan.FromSeconds(5);
            _pollTimer.Tick += (s, e) => CargarPedidos();
            _pollTimer.Start();



        }

        private void PedidoPreview_VerDetallesClicked(object sender, EventArgs e)
        {
            var pedido = (Pedido)((PedidoPreview)sender).DataContext;
            Page page;



            if (pedido is PedidoParaLocal local)
            {
                Console.Write("pedido es local");
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
                Console.Write("pedido es local");
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

        private void CargarPedidos()
        {
;
            Pedidos.Clear();
            var list = PedidoDAO.ObtenerPedidos(UsuarioSingleton.Instance.IdCargo);
            foreach (var p in list)
                Pedidos.Add(p);
        }


    }
}
