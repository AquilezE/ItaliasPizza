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
using System.Windows.Shapes;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Interaction logic for NoEntregadoWindow.xaml
    /// </summary>
    public partial class NoEntregadoWindow : Window
    {
        public string Razon { get; private set; } = null;
        private const int MaxLength = 300;


        public NoEntregadoWindow()
        {
            InitializeComponent();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; 
        }

        private void Enviar_Click(object sender, RoutedEventArgs e)
        {
            Razon = ReasonTextBox.Text.Trim();
            DialogResult = true;
        }

        private void ReasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btEnviar.IsEnabled = ReasonTextBox.Text.Trim().Length > 0;
        }
    }
}
