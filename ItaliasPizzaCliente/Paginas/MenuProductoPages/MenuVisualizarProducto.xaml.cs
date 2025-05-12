using ItaliasPizzaDB.DataAccessObjects.ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private List<ProductoVisualizacion> TodosLosProductos;
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
            public string TextoBoton => TieneReceta ? "Receta" : "Foto";
        }

        private void CargarProductos()
        {
            var productosDb = ProductoDAO.ObtenerProductosConCategoria();
            TodosLosProductos = productosDb.Select(p => new ProductoVisualizacion
            {
                idProducto = p.IdProducto,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.CategoriaProducto?.CategoriaProductoNombre ?? "Sin categoría",
                Precio = p.Precio,
                TieneReceta = p.IdReceta != null,
            }).ToList();

            Productos.Clear();
            foreach (var p in TodosLosProductos)
                Productos.Add(p);
        }


        private void BtnVerDetalleProducto_Click(object sender, RoutedEventArgs e)
        {
            var vm = ((FrameworkElement)sender).DataContext as ProductoVisualizacion;
            if (vm == null) return;

            if (vm.TieneReceta)
            {
                var ventana = new VentanaVerReceta(vm.idProducto);
                ventana.Owner = Window.GetWindow(this);
                ventana.ShowDialog();
            }
            else
            {
                // Mostrar imagen en grande
                var producto = ProductoDAO.ObtenerProductoCompleto(vm.idProducto);
                if (producto != null)
                {
                    string rutaImagenCompleta = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "ImagenesProductos",
                        producto.ImagenRuta);

                    if (File.Exists(rutaImagenCompleta))
                    {
                        ImagenVista.Source = new BitmapImage(new Uri(rutaImagenCompleta));
                        OverlayImagen.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Imagen no encontrada.");
                    }
                }
            }
        }

        private void TbBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = TbBuscarNombre.Text.Trim().ToLower();

            var filtrados = TodosLosProductos
                .Where(p => p.Nombre.ToLower().Contains(texto) ||
                            p.Codigo.ToLower().Contains(texto) ||
                            p.Categoria.ToLower().Contains(texto))
                .ToList();

            Productos.Clear();
            foreach (var p in filtrados)
                Productos.Add(p);
        }

        private void BtnCerrarImagen_Click(object sender, RoutedEventArgs e)
        {
            OverlayImagen.Visibility = Visibility.Collapsed;
            ImagenVista.Source = null;
        }
    }
}
