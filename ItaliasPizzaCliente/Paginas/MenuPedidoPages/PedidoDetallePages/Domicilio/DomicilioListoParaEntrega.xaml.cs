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
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Domicilio
{
    /// <summary>
    /// Interaction logic for DomicilioListoParaEntrega.xaml
    /// </summary>
    public partial class DomicilioListoParaEntrega : Page
    {
        public PedidoParaLlevar PedidoParaLlevar { get; set; }
        public DomicilioListoParaEntrega(PedidoParaLlevar pedidoParaLlevar)
        {
            InitializeComponent();
            this.DataContext = this;

            this.PedidoParaLlevar = pedidoParaLlevar;
        }

        public void Cerrar_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(PedidoParaLlevar.Cliente.Nombre);

            NavigationService?.GoBack();
        }

        public void InicarEnvio_Click(object sender, RoutedEventArgs e)
        {
            if (PedidoParaLlevar == null)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification("No se ha seleccionado un pedido.");
                return;
            }

            int result = PedidoDAO.CambiarEstadoPedido(PedidoParaLlevar.IdPedido, (int)StatusPedidoEnum.EnCamino);

            if (result != 0)
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowErrorNotification($"El pedido {PedidoParaLlevar.IdPedido} no se pudo entregar");
            }
            else
            {
                DialogoNotificacion dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowSuccessNotification($"El pedido {PedidoParaLlevar.IdPedido} se ha marcado para entrega");

                var menuPage = new DomicilioEnCamino(PedidoParaLlevar);
                NavigationService?.Navigate(menuPage);

            }
        }

    }
}
