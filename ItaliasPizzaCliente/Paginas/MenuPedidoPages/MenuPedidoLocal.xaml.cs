using ItaliasPizzaCliente.Paginas.MenuInventarioPages;
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

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Lógica de interacción para PedidoLocal.xaml
    /// </summary>
    public partial class MenuPedidoLocal : Page
    {
        public MenuPedidoLocal()
        {
            InitializeComponent();
        }

        private void cbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Agergar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MenuVisualizarInventario());
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IncrementarCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DecrementarCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBoxProducto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
