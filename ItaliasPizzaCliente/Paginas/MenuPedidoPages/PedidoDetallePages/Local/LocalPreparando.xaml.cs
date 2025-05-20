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
    /// Interaction logic for LocalPreparando.xaml
    /// </summary>
    public partial class LocalPreparando : Page
    {
        public LocalPreparando(PedidoParaLocal pedidoParaLocal)
        {
            InitializeComponent();

            this.DataContext = pedidoParaLocal;
        }
    }
}
