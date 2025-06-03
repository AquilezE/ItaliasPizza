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
    /// Interaction logic for LocalListoParaEntrega.xaml
    /// </summary>
    public partial class LocalListoParaEntrega : Page
    {


        public PedidoParaLocal PedidoParaLocal { get; set; }
        public String razonNoEntregado { get; set; } = "";
        public ObservableCollection<DetallePedidoDTO> Detalles { get; set; } = new ObservableCollection<DetallePedidoDTO>();


        public LocalListoParaEntrega(PedidoParaLocal pedidoParaLocal)
        {
            InitializeComponent();

            this.DataContext = pedidoParaLocal;
            this.DataContext = this;
            this.PedidoParaLocal = PedidoParaLocal;
            LlenarDetallesPedido();
        }

        private void NoEntregado_Click(object sender, RoutedEventArgs e)
        {

            var modal = new NoEntregadoWindow();
            bool? resultModal = modal.ShowDialog();


            if (resultModal != true || string.IsNullOrWhiteSpace(modal.Razon))
            {
                return;
            }


            int dbResult = PedidoDAO.MarcarPedidoNoEntregado(PedidoParaLocal.IdPedido, modal.Razon);

            DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
            if (dbResult != 0)
            {
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLocal.IdPedido} no se pudo marcar como no entregado.");
            }
            else
            {
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLocal.IdPedido} se ha marcado como no entregado.");
            }

            NavigationService?.GoBack();
        }

        private void Entregado_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLocal == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
            }

            int result = PedidoDAO.CambiarEstadoPedido(PedidoParaLocal.IdPedido, (int)StatusPedidoEnum.Entregado);

            if (result == (int)StatusPedidoEnum.Cancelado)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLocal.IdPedido} fue Cancelado.");
            }
            else if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLocal.IdPedido} no se puede preparar.");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLocal.IdPedido} se ha marcado como entregado");

                var menuPage = new MenuVerPedidos();
                NavigationService?.Navigate(menuPage);


            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            var menuPage = new MenuVerPedidos();
            NavigationService?.Navigate(menuPage);
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
    }
}
