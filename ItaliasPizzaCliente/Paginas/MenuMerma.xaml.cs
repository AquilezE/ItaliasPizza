using ItaliasPizzaCliente.Paginas.MenuInventarioPages;
using ItaliasPizzaCliente.UserControllers;
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
using ItaliasPizzaCliente.Paginas.MenuMergaPages;

namespace ItaliasPizzaCliente.Paginas
{
    /// <summary>
    /// Interaction logic for MenuMerma.xaml
    /// </summary>
    public partial class MenuMerma : Page
    {
        public MenuMerma()
        {
            InitializeComponent();
        }

        private void selectionChanged(object sender, RoutedEventArgs e)
        {
            var selected = upperBar.SelectedItem as NavButton;

            navFrame.Navigate(selected.Navlink);
        }

        private void navFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (navFrame.CanGoBack)
            {
                navFrame.RemoveBackEntry();
            }
        }
    }
}
