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
    /// Lógica de interacción para MenuVisualizarProducto.xaml
    /// </summary>
    public partial class MenuVisualizarProducto : Page
    {

        public ObservableCollection<ProductoVisualizacion> Productos { get; set; }
        public MenuVisualizarProducto()
        {
            InitializeComponent();
            Productos = new ObservableCollection<ProductoVisualizacion>();
            DataContext = this;

            CargarProductos(); // <- Aquí llamas el método
        }

        public class ProductoVisualizacion
        {
            public int idProducto { get; set; }
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public float Precio { get; set; }
            public bool TieneReceta { get; set; }
        }

        private void CargarProductos()
        {
            var productosDb = ProductoDAO.ObtenerProductosConCategoria();
            var productosVm = productosDb.Select(p => new ProductoVisualizacion
            {
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.CategoriaProducto?.CategoriaProductoNombre ?? "Sin categoría",
                Precio = p.Precio,
                TieneReceta = p.IdReceta != null
            }).ToList();

            Productos.Clear();
            foreach (var p in productosVm)
                Productos.Add(p);
        }

        private void BtnVerReceta_Click(object sender, RoutedEventArgs e)
        {
            var vm = ((FrameworkElement)sender).DataContext as ProductoVisualizacion;
            if (vm != null && vm.TieneReceta)
            {
                var ventana = new VentanaVerReceta(vm.idProducto); // le pasas el ID para cargar receta
                ventana.Owner = Window.GetWindow(this);
                ventana.ShowDialog();
            }
        }

        private void TbBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {

 
        }
    }
}
