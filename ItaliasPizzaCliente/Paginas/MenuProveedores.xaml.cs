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
using ItaliasPizzaCliente.UserControllers;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;


namespace ItaliasPizzaCliente.Paginas
{
    /// <summary>
    /// Interaction logic for MenuProveedores.xaml
    /// </summary>
    public partial class MenuProveedores : Page
    {
        public MenuProveedores()
        {
            InitializeComponent();
            navFrame.Navigated += navFrame_Navigated;
            DataContext = Singletons.UsuarioSingleton.Instance;
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
