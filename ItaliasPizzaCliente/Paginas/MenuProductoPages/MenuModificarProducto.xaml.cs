using ItaliasPizzaDB.DataAccessObjects.ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
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

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para MenuModificarProducto.xaml
    /// </summary>
    public partial class MenuModificarProducto : Page
    {
        public ObservableCollection<ProductoVisualizacion> Productos { get; set; }

        public MenuModificarProducto()
        {
            InitializeComponent();
            Productos = new ObservableCollection<ProductoVisualizacion>();
            DataContext = this;
            CargarProductos();
        }

        public class ProductoVisualizacion
        {
            public int idProducto { get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public bool TieneReceta { get; set; }
        }
        private void CargarProductos()
        {
            var productosDb = ProductoDAO.ObtenerProductosConCategoria();
            var productosVm = productosDb.Select(p => new ProductoVisualizacion
            {
                idProducto = p.IdProducto,
                Nombre = p.Nombre,
                Precio = p.Precio,
                TieneReceta = p.IdReceta != null,
            }).ToList();

            Productos.Clear();
            foreach (var p in productosVm)
                Productos.Add(p);
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProductoVisualizacion productoSeleccionado)
            {
                var ventana = new VentanaModificarProducto(productoSeleccionado.idProducto);
                ventana.Owner = Window.GetWindow(this); // opcional si usas Window
                ventana.ShowDialog();
                CargarProductos();
            }
        }


    }
}
