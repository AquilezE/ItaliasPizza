using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Local
{
    /// <summary>
    /// Interaction logic for LocalPreparando.xaml
    /// </summary>
    public partial class LocalPreparando : Page
    {

        public PedidoParaLocal PedidoParaLocal { get; set; }

        public ObservableCollection<DetallePedidoDTO> Detalles { get; set; } = new ObservableCollection<DetallePedidoDTO>();

        public UsuarioSingleton usuarioSingleton { get; set; } = UsuarioSingleton.Instance;

        public LocalPreparando(PedidoParaLocal pedidoParaLocal)
        {
            InitializeComponent();

            this.DataContext = this;
            this.PedidoParaLocal = pedidoParaLocal;
            LlenarDetallesPedido();

        }

        private void LlenarDetallesPedido()
        {
            var list = PedidoDAO.ObtenerDetallesPorPedido(PedidoParaLocal.IdPedido);
            if (list != null && list.Any())
            {
                Detalles.Clear();
                foreach (var dto in list)
                    Detalles.Add(dto);
            }
            else
            {
                MessageBox.Show("No se encontraron detalles para el pedido.");
            }

        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            var menuPage = new MenuVerPedidos();
            NavigationService?.Navigate(menuPage);
        }

        private void Receta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int IdProducto)
            {
                VentanaVerReceta ventanaReceta = new VentanaVerReceta(IdProducto);
                ventanaReceta.ShowDialog();
            }
        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLocal == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
            }

            int result = PedidoDAO.CambiarEstadoPedido(PedidoParaLocal.IdPedido, (int)StatusPedidoEnum.ListoParaEntrega);

            Console.WriteLine(result);


            if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLocal.IdPedido} no se pudo finalizar");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLocal.IdPedido} se ha completado");

                var menuPage = new MenuVerPedidos();
                NavigationService?.Navigate(menuPage);

            }

        }
    }
}
