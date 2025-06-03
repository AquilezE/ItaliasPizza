using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.Utils;
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
using ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio;
using ItaliasPizzaDB;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Local
{
    /// <summary>
    /// Interaction logic for LocalRealizadoPage.xaml
    /// </summary>
    /// 


    public partial class LocalRealizadoPage : Page
    {

        public PedidoParaLocal PedidoParaLocal { get; set; }

        public ObservableCollection<DetallePedidoDTO> Detalles { get; set; } = new ObservableCollection<DetallePedidoDTO>();

        public UsuarioSingleton usuarioSingleton { get; set; } = UsuarioSingleton.Instance;

        public float Total { get; set; }


        public LocalRealizadoPage(PedidoParaLocal pedidoParaLocal)
        {
            InitializeComponent();

            this.DataContext = pedidoParaLocal;

            this.PedidoParaLocal = pedidoParaLocal;

            LlenarDetallesPedido();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();

        }


        private void Receta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int IdProducto)
            {
                VentanaVerReceta ventanaReceta = new VentanaVerReceta(IdProducto);
                ventanaReceta.ShowDialog();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            int result = PedidoDAO.CancelarPedido(PedidoParaLocal.IdPedido);

            if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLocal.IdPedido} no se puede cancelar.");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLocal.IdPedido} se ha cancelado");
            }

            NavigationService?.GoBack();
        }

        private void Preparar_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLocal == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
            }

            if (UsuarioSingleton.Instance.NombreCargo != RolesEnum
                    .Cocinero.ToString())
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No tienes permisos para preparar este pedido.");
            }

            int result = PedidoDAO.CambiarEstadoPedido(PedidoParaLocal.IdPedido, (int)StatusPedidoEnum.Preparando);

            Console.WriteLine(result);

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
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLocal.IdPedido} ha entrado a preparacion");

                var preparadoPage = new LocalPreparando(PedidoParaLocal);
                NavigationService?.Navigate(preparadoPage);
            }


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
