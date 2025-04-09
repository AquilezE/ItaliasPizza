using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.UserControllers;
using ItaliasPizzaCliente.Utils;
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

namespace ItaliasPizzaCliente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            navFrame.Navigated += NavFrame_Navigated;
            DataContext = UsuarioSingleton.Instance;
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selected = sidebar.SelectedItem as NavButton;

            navFrame.Navigate(selected.Navlink);


        }

        private void NavFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (navFrame.CanGoBack)
            {
                navFrame.RemoveBackEntry();
            }
        }
    }
}
