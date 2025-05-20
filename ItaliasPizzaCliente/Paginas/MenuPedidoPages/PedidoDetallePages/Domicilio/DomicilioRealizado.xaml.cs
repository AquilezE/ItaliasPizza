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
using ItaliasPizzaDB.DataTransferObjects;
using ItaliasPizzaDB.DataAccessObjects;
using System.Xml.Schema;
using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaDB;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaCliente.Singletons;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio
{
    /// <summary>
    /// Interaction logic for DomicilioRealizado.xaml
    /// </summary>
    public partial class DomicilioRealizado : Page
    {

        public PedidoParaLlevar PedidoParaLlevar { get; set; }

        public ObservableCollection<DetallePedidoDTO> Detalles { get; set; } = new ObservableCollection<DetallePedidoDTO>();

        public UsuarioSingleton usuarioSingleton { get; set; } = UsuarioSingleton.Instance;

        public float Total { get; set; }

        public DomicilioRealizado(PedidoParaLlevar pedidoParaLlevar)
        {
            InitializeComponent();

            this.DataContext = this;

            this.PedidoParaLlevar = pedidoParaLlevar;

            // LlenarDetallesPedido();
            LlenarDetallesPedidoMock();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {

            int result = PedidoDAO.CambiarEstadoPedido((int)StatusPedidoEnum.Cancelado, PedidoParaLlevar.IdPedido);

            if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLlevar.IdPedido} no se puede cancelar.");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLlevar.IdPedido} se ha cancelado");
            }

            NavigationService?.GoBack();
        }

        private void Preparar_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLlevar == null)
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

            //int result = PedidoDAO.CambiarEstadoPedido((int)StatusPedidoEnum.Preparando, PedidoParaLlevar.IdPedido);
            int result = 0;

            if (result == (int)StatusPedidoEnum.Cancelado)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLlevar.IdPedido} fue Cancelado.");
            }
            else if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLlevar.IdPedido} no se puede preparar.");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLlevar.IdPedido} ha entrado a preparacion");

                var preparadoPage = new DomicilioPreparado(PedidoParaLlevar);
                NavigationService?.Navigate(preparadoPage);
            }


        }


        private void LlenarDetallesPedido()
        {
            var list = PedidoDAO.ObtenerDetallesPorPedido(PedidoParaLlevar.IdPedido);
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

        private void LlenarDetallesPedidoMock()
        {
            Detalles.Clear();
            Detalles.Add(new DetallePedidoDTO { IdProducto = 1, ProductoNombre = "Pizza", Cantidad = 2, Subtotal = 200, IdReceta = 10 });
            Detalles.Add(new DetallePedidoDTO { IdProducto = 2, ProductoNombre = "Bebida", Cantidad = 1, Subtotal = 50, IdReceta = 20 });
            Detalles.Add(new DetallePedidoDTO { IdProducto = 3, ProductoNombre = "Ensalada", Cantidad = 1, Subtotal = 30, IdReceta = 11 });
            Detalles.Add(new DetallePedidoDTO { IdProducto = 4, ProductoNombre = "Postre", Cantidad = 1, Subtotal = 20, IdReceta = 30 });
        }

        private void Receta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int IdProducto)
            {
                VentanaVerReceta ventanaReceta = new VentanaVerReceta(IdProducto);
                ventanaReceta.ShowDialog();
            }
        }
    }
}
