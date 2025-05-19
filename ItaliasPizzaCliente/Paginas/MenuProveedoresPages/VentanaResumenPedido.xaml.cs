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
using System.Windows.Shapes;

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{
    /// <summary>
    /// Lógica de interacción para VentanaResumenPedido.xaml
    /// </summary>
    public partial class VentanaResumenPedido : Window
    {

        public class InsumoResumenPedido
        {
            public int IdInsumo { get; set; }
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
            public string Unidad { get; set; }
            public float PrecioUnitario { get; set; }
            public float Total => Cantidad * PrecioUnitario;
        }


        public ObservableCollection<InsumoResumenPedido> InsumosResumen { get; set; }
        public float MontoTotal => InsumosResumen.Sum(i => i.Total);

        public VentanaResumenPedido(List<InsumoResumenPedido> resumen)
        {
            InitializeComponent();
            InsumosResumen = new ObservableCollection<InsumoResumenPedido>(resumen);
            DataContext = this;
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
