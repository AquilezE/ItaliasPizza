using ItaliasPizzaDB.DataAccessObjects;
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

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaVerReceta.xaml
    /// </summary>
    public partial class VentanaVerReceta : Window
    {
        public VentanaVerReceta(int idProducto)
        {
            InitializeComponent();

            var receta = RecetaDAO.ObtenerRecetaPorIdProducto(idProducto);
            this.DataContext = receta;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
