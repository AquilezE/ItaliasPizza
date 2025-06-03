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
using ItaliasPizzaDB.DataAccessObjects;

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    public partial class MenuModificarReceta : Page
    {
        public ObservableCollection<ProductoRecetaVisualizacion> ProductosConReceta { get; set; }

        public MenuModificarReceta()
        {
            InitializeComponent();
            ProductosConReceta = new ObservableCollection<ProductoRecetaVisualizacion>();
            DataContext = this;
            CargarProductosConReceta();
        }

        public class ProductoRecetaVisualizacion
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public bool TieneReceta { get; set; }
        }

        private void CargarProductosConReceta()
        {
            var productosDb = ProductoDAO.ObtenerProductosConCategoria();

            var productosConReceta = productosDb
                .Where(p => p.IdReceta != null) // Solo los que tienen receta
                .Select(p => new ProductoRecetaVisualizacion
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    TieneReceta = true
                }).ToList();

            ProductosConReceta.Clear();
            foreach (var producto in productosConReceta)
                ProductosConReceta.Add(producto);
        }

        private void BtnModificarReceta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProductoRecetaVisualizacion productoSeleccionado)
            {
                var ventana = new VentanaRegistrarReceta(productoSeleccionado.IdProducto); // 👈 usar constructor modificado
                ventana.Owner = Window.GetWindow(this);
                ventana.ShowDialog();
                CargarProductosConReceta();
            }
        }

    }
}
