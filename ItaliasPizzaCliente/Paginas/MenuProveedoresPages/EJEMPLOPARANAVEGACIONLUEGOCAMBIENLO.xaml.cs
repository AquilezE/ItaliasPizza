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

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{
    /// <summary>
    /// Interaction logic for EJEMPLOPARANAVEGACIONLUEGOCAMBIENLO.xaml
    /// </summary>
    public partial class EJEMPLOPARANAVEGACIONLUEGOCAMBIENLO : Page
    {
        public Proveedor Proveedor { get; set; } = new Proveedor();
        public EJEMPLOPARANAVEGACIONLUEGOCAMBIENLO(object proveedor)
        {
            InitializeComponent();
            Proveedor = (Proveedor)proveedor;
            DataContext = this;

        }
    }
}
