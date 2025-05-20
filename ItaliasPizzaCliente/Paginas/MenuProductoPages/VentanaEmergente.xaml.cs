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

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaEmergente.xaml
    /// </summary>
    public partial class VentanaEmergente : Window
    {
        public bool Resultado { get; private set; } = false;

        public VentanaEmergente(string titulo, string mensaje, string textoPrimario, string textoSecundario, bool mostrarSecundario = true)
        {
            InitializeComponent();

            TxtTitulo.Text = titulo;
            TxtMensaje.Text = mensaje;
            BtnPrimario.Content = textoPrimario;

            if (mostrarSecundario)
            {
                BtnSecundario.Content = textoSecundario;
                BtnSecundario.Visibility = Visibility.Visible;
            }
            else
            {
                BtnSecundario.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnPrimario_Click(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            DialogResult = true;
            Close();
        }

        private void BtnSecundario_Click(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            DialogResult = false;
            Close();
        }
    }
}

