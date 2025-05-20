using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaCliente.Singletons;
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
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio
{
    /// <summary>
    /// Interaction logic for DomicilioPreparado.xaml
    /// </summary>
    public partial class DomicilioPreparado : Page
    {


        public PedidoParaLlevar PedidoParaLlevar { get; set; }

        public ObservableCollection<DetallePedidoDTO> Detalles { get; set; } = new ObservableCollection<DetallePedidoDTO>();

        public UsuarioSingleton usuarioSingleton { get; set; } = UsuarioSingleton.Instance;

        public DomicilioPreparado(PedidoParaLlevar pedidoParaLlevar)
        {
            InitializeComponent();
            
            this.DataContext = this;
            PedidoParaLlevar = pedidoParaLlevar;


            LlenarDetallesPedidoMock();

        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            var menuPage = new MenuVerPedidos();
            NavigationService?.Navigate(menuPage);
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
            if (PedidoParaLlevar == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
            }

            //int result = PedidoDAO.CambiarEstadoPedido((int)StatusPedidoEnum.ListoParaEntrega, PedidoParaLlevar.IdPedido);
            int result = 0;

            if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLlevar.IdPedido} no se pudo finalizar");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLlevar.IdPedido} se ha completado");

                var menuPage = new MenuVerPedidos();
                NavigationService?.Navigate(menuPage);

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
    }
}
