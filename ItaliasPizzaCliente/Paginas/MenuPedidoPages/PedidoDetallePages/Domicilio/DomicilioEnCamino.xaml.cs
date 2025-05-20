using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
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
using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio
{
    /// <summary>
    /// Interaction logic for DomicilioEnCamino.xaml
    /// </summary>
    public partial class DomicilioEnCamino : Page
    {

        public PedidoParaLlevar PedidoParaLlevar { get; set; }
        public String razonNoEntregado { get; set; } = "";
        public DomicilioEnCamino(PedidoParaLlevar pedidoParaLlevar)
        {
            InitializeComponent();
            this.DataContext = this;
            this.PedidoParaLlevar = pedidoParaLlevar;

        }

        private void Entregado_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLlevar == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
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
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLlevar.IdPedido} se ha marcado como entregado");

            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            var menuPage = new MenuVerPedidos();
            NavigationService?.Navigate(menuPage);
        }
    }
}
