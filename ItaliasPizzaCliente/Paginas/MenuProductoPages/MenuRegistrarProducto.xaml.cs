using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.DataAccessObjects.ItaliasPizzaDB.DataAccessObjects;
using Path = System.IO.Path;
using System.Collections.ObjectModel;

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para MenuRegistrarProducto.xaml
    /// </summary>


    public partial class MenuRegistrarProducto : Page
    {


        string textoEjemploNombre = "Pizza de Pepperoni";
        string textoEjemploCodigo = "P22";
        string textoEjemploPrecio = "120";
        string textoEjemploRestricciones = "Coloca restricciones del producto";
        string textoEjemploDescripcion = "Describe el producto...";
        Receta receta = null;
        private string rutaImagenSeleccionada = null;
        public ObservableCollection<CategoriaInsumo> Categorias { get; set; }

        public MenuRegistrarProducto()
        {
            InitializeComponent();
            Categorias = new ObservableCollection<CategoriaInsumo>();

            DataContext = this;
            CargarCategorias();
        }

        public void CargarCategorias()
        {
            var categoriasLista = CategoriaInsumoDAO.ObtenerCategoriasInsumo();
            Categorias.Clear();

            foreach (var categoria in categoriasLista)
            {
                Categorias.Add(categoria);
            }

        }

        private void TbCodigo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbCodigo.Text == textoEjemploCodigo && TbCodigo.Foreground == Brushes.Gray)
            {
                TbCodigo.Text = "";
                TbCodigo.Foreground = Brushes.Black;
            }
        }

        private void TbCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbCodigo.Text))
            {
                TbCodigo.Text = textoEjemploCodigo;
                TbCodigo.Foreground = Brushes.Gray;
            }
        }

        private void TbPrecio_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbPrecio.Text == textoEjemploPrecio && TbPrecio.Foreground == Brushes.Gray)
            {
                TbPrecio.Text = "";
                TbPrecio.Foreground = Brushes.Black;
            }
        }

        private void TbPrecio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbPrecio.Text))
            {
                TbPrecio.Text = textoEjemploPrecio;
                TbPrecio.Foreground = Brushes.Gray;
            }
        }

        private void TbRestricciones_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbRestricciones.Text == textoEjemploRestricciones && TbRestricciones.Foreground == Brushes.Gray)
            {
                TbRestricciones.Text = "";
                TbRestricciones.Foreground = Brushes.Black;
            }
        }

        private void TbRestricciones_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbRestricciones.Text))
            {
                TbRestricciones.Text = textoEjemploRestricciones;
                TbRestricciones.Foreground = Brushes.Gray;
            }
        }

        private void TbNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbNombre.Text == textoEjemploNombre && TbNombre.Foreground == Brushes.Gray)
            {
                TbNombre.Text = "";
                TbNombre.Foreground = Brushes.Black;
            }
        }

        private void TbNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbNombre.Text))
            {
                TbNombre.Text = textoEjemploNombre;
                TbNombre.Foreground = Brushes.Gray;
            }
        }

        private void TbDescripcion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbDescripcion.Text == textoEjemploDescripcion && TbDescripcion.Foreground == Brushes.Gray)
            {
                TbDescripcion.Text = "";
                TbDescripcion.Foreground = Brushes.Black;
            }
        }

        private void TbDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbDescripcion.Text))
            {
                TbDescripcion.Text = textoEjemploDescripcion;
                TbDescripcion.Foreground = Brushes.Gray;
            }
        }

        private void BtnCargarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                rutaImagenSeleccionada = openFileDialog.FileName;

                BitmapImage imagen = new BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.UriSource = new Uri(rutaImagenSeleccionada);
                imagen.DecodePixelWidth = 120; // Reducimos para vista previa
                imagen.EndInit();

                ImgPreview.Source = imagen;
                ImgPreview.Visibility = Visibility.Visible;
            }
        }



        private void TbPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private bool CampoEsValido(TextBox tb)
        {

            return !string.IsNullOrWhiteSpace(tb.Text) && tb.Foreground != Brushes.Gray;
        }


        private bool VerificarReceta()
        {
            return ToggleBProductoConReceta.IsChecked == true;
        }

        private void CrearReceta()
        {
            if (VerificarReceta())
            {
                var confirmar = new VentanaEmergente(
                    "Para continuar debe registrar la receta del producto",
                    "¿Desea continuar?",
                    "Aceptar", "Cancelar"
                );

                if (confirmar.ShowDialog() == true && confirmar.Resultado)
                {
                    var ventanaReceta = new VentanaRegistrarReceta();
                    ventanaReceta.Owner = Window.GetWindow(this); // para centrar sobre esta

                    if (ventanaReceta.ShowDialog() == true)
                    {
                        receta = ventanaReceta.RecetaCreada;
                    }
                    else
                    {
                        receta = null;
                    }
                }
                else
                {
                    receta = null;
                }
            }
        }


        private void BtnRegistrarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (!CampoEsValido(TbNombre))
            {
                MessageBox.Show("El campo Nombre es obligatorio.");
                return;
            }

            if (!CampoEsValido(TbCodigo))
            {
                MessageBox.Show("El campo Código es obligatorio.");
                return;
            }

            if (!CampoEsValido(TbPrecio))
            {
                MessageBox.Show("El campo Precio es obligatorio.");
                return;
            }

            if (!CampoEsValido(TbRestricciones))
            {
                MessageBox.Show("El campo Restricciones es obligatorio.");
                return;
            }

            if (!CampoEsValido(TbDescripcion))
            {
                MessageBox.Show("El campo Descripción es obligatorio.");
                return;
            }

            if (ImgPreview.Source == null || ImgPreview.Visibility != Visibility.Visible)
            {
                MessageBox.Show("Debes cargar una imagen del producto.");
                return;
            }

            if (CbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Debes seleccionar una categoría.");
                return;
            }

            var dialogo = new VentanaEmergente(
                "¿Desea registrar el producto en el menú?",
                "No se podrá revertir este registro",
                "Aceptar",
                "Cancelar");

            CrearReceta();

            var producto = new Producto
            {
                Nombre = TbNombre.Text,
                Codigo = TbCodigo.Text,
                Categoria = CbCategoria.Text,
                Descripcion = TbDescripcion.Text,
                Restricciones = TbRestricciones.Text,
                Precio = float.Parse(TbPrecio.Text),
                Status = true,
                MaxPerOrder = 1
            };

            var dao = new ProductoDAO();
            bool guardado = dao.AgregarProducto(producto, VerificarReceta(), receta, rutaImagenSeleccionada);



            if (guardado)
            {
                new VentanaEmergente("Producto registrado con éxito", "", "Aceptar", "", false).ShowDialog();
            }
            else
            {
                new VentanaEmergente("Error al registrar el producto.", "", "Aceptar", "", false).ShowDialog();
            }


        }


    }
}
