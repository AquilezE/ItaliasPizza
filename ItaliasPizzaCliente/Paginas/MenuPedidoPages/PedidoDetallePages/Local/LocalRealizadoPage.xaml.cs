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

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages.PedidoDetallePages.Local
{
    /// <summary>
    /// Interaction logic for LocalRealizadoPage.xaml
    /// </summary>
    public partial class LocalRealizadoPage : Page
    {
        public LocalRealizadoPage(PedidoParaLocal pedidoParaLocal)
        {
            InitializeComponent();

            this.DataContext = pedidoParaLocal;
        }


    }
}
