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
    /// Interaction logic for PedidoPreview.xaml
    /// </summary>
    public partial class PedidoPreview : UserControl
    {
        public PedidoPreview()
        {
            InitializeComponent();
        }

        public int IdPedido
        {
            get { return (int)GetValue(IdPedidoProperty); }
            set { SetValue(IdPedidoProperty, value); }
        }

        public static readonly DependencyProperty IdPedidoProperty =
            DependencyProperty.Register("IdPedido", typeof(int), typeof(PedidoPreview), new PropertyMetadata(0));

        public float Total
        {
            get { return (float)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(float), typeof(PedidoPreview), new PropertyMetadata(0f));

        public int Mesa
        {
            get { return (int)GetValue(MesaProperty); }
            set { SetValue(MesaProperty, value); }
        }

        public static readonly DependencyProperty MesaProperty =
            DependencyProperty.Register("Mesa", typeof(int), typeof(PedidoPreview), new PropertyMetadata(0));

        public string NombreCliente { 
            get { return (string)GetValue(NombreClienteProperty); }
            set { SetValue(NombreClienteProperty, value); }
        }

        public static readonly DependencyProperty NombreClienteProperty =
            DependencyProperty.Register("NombreCliente", typeof(string), typeof(PedidoPreview), new PropertyMetadata(string.Empty));



        public DateTime Fecha
        {
            get { return (DateTime)GetValue(FechaProperty); }
            set { SetValue(FechaProperty, value); }
        }

        public static readonly DependencyProperty FechaProperty =
            DependencyProperty.Register("Fecha", typeof(DateTime), typeof(PedidoPreview), new PropertyMetadata(DateTime.Now));

        public event EventHandler VerDetallesClicked;

        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            VerDetallesClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
