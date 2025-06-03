using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaModificarProducto.xaml
    /// </summary>
    public partial class VentanaModificarProducto : Window
    {
        
        Receta receta = null;
        private bool teniaRecetaOriginal = false;
        private Producto productoActual;
        private string rutaImagenOriginal;
        private string rutaImagenNueva = null;
        public ObservableCollection<CategoriaProducto> Categorias { get; set; }
        private readonly DialogoNotificacion notificador = new DialogoNotificacion();

        public VentanaModificarProducto(int idProducto)
        {
            InitializeComponent();
            Categorias = new ObservableCollection<CategoriaProducto>();
            DataContext = this;
            CargarCategorias();
            CargarProducto(idProducto);
        }

        private void CargarProducto(int idProducto)
        {

            productoActual = ProductoDAO.ObtenerProductoCompleto(idProducto);

            productoActual = ProductoDAO.ObtenerProductoCompleto(idProducto);
            if (productoActual == null)
            {
                MessageBox.Show("No se pudo cargar el producto.");
                Close();
                return;
            }

            teniaRecetaOriginal = productoActual.IdReceta != null;
            ToggleBProductoConReceta.IsChecked = teniaRecetaOriginal;

            // Rellenar campos
            TbNombre.Text = productoActual.Nombre;
            TbNombre.Tag = productoActual.Nombre;

            TbCodigo.Text = productoActual.Codigo;
            TbCodigo.Tag = productoActual.Codigo;

            TbPrecio.Text = productoActual.Precio.ToString();
            TbPrecio.Tag = productoActual.Precio.ToString();

            TbRestricciones.Text = productoActual.Restricciones;
            TbRestricciones.Tag = productoActual.Restricciones;

            TbDescripcion.Text = productoActual.Descripcion;
            TbDescripcion.Tag = productoActual.Descripcion;

            CbCategoria.SelectedValue = productoActual.IdCategoriaProducto;
            ToggleBProductoConReceta.IsChecked = productoActual.IdReceta != null;

            // Cargar imagen
            if (!string.IsNullOrEmpty(productoActual.ImagenRuta))
            {
                rutaImagenOriginal = Path.Combine("ImagenesProductos", productoActual.ImagenRuta);
                Console.WriteLine("Ruta completa imagen original: " + Path.GetFullPath(rutaImagenOriginal));

                if (File.Exists(rutaImagenOriginal))
                {
                    BitmapImage imagen = new BitmapImage();
                    imagen.BeginInit();
                    imagen.UriSource = new Uri(Path.GetFullPath(rutaImagenOriginal), UriKind.Absolute);
                    imagen.CacheOption = BitmapCacheOption.OnLoad;
                    imagen.DecodePixelWidth = 120;
                    imagen.EndInit();

                    ImgPreview.Source = imagen;
                    ImgPreview.Visibility = Visibility.Visible;
                }
            }
        }

        public void CargarCategorias()
        {
            var categoriasLista = CategoriaProductoDAO.ObtenerCategoriasProducto();
            Categorias.Clear();

            foreach (var categoria in categoriasLista)
            {
                Categorias.Add(categoria);
            }

        }

        private void TextBox_LimpiarAlEnfocar(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == (string)tb.Tag)
            {
                tb.Text = "";
            }
        }



        private void BtnCargarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)    
            {
                rutaImagenNueva = openFileDialog.FileName;

                // Mostrar vista previa de la imagen nueva seleccionada
                BitmapImage imagen = new BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.UriSource = new Uri(rutaImagenNueva, UriKind.Absolute); // URI válida
                imagen.DecodePixelWidth = 120;
                imagen.EndInit();

                ImgPreview.Source = imagen;
                ImgPreview.Visibility = Visibility.Visible;
            }
        }
        private bool CampoEsValido(TextBox tb)
        {
            return !string.IsNullOrWhiteSpace(tb.Text);
        }


        private void TbPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void BtnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (!CampoEsValido(TbNombre) || !CampoEsValido(TbCodigo) ||
                !CampoEsValido(TbPrecio) || !CampoEsValido(TbRestricciones) ||
                !CampoEsValido(TbDescripcion) || CbCategoria.SelectedItem == null)
            {
                notificador.ShowWarningNotification("Completa todos los campos antes de continuar.");
                return;
            }

            // Actualizar campos
            productoActual.Nombre = TbNombre.Text;
            productoActual.Codigo = TbCodigo.Text;
            productoActual.Precio = float.Parse(TbPrecio.Text);
            productoActual.Restricciones = TbRestricciones.Text;
            productoActual.Descripcion = TbDescripcion.Text;
            productoActual.IdCategoriaProducto = (int)CbCategoria.SelectedValue;

            // Variables de control
            bool seEliminaraReceta = false;
            int? recetaIdAEliminar = null;

            // Si desactivaron la receta y el producto la tenía
            if (ToggleBProductoConReceta.IsChecked == false && productoActual.IdReceta != null)
            {
                recetaIdAEliminar = productoActual.IdReceta; // ⬅️ lo guardamos antes
                productoActual.IdReceta = null;
                seEliminaraReceta = true;
            }

            if (ToggleBProductoConReceta.IsChecked == true && !teniaRecetaOriginal)
            {
                var dialogo = new VentanaEmergente(
                    "Este producto no tiene receta",
                    "¿Deseas registrar una receta antes de guardar?",
                    "Registrar Receta", "Cancelar");

                if (dialogo.ShowDialog() == true && dialogo.Resultado)
                {
                    var ventanaReceta = new VentanaRegistrarReceta();
                    ventanaReceta.Owner = this;

                    if (ventanaReceta.ShowDialog() == true)
                    {
                        receta = ventanaReceta.RecetaCreada;
                        RecetaDAO.GuardarReceta(receta);
                        productoActual.IdReceta = receta.IdReceta;
                    }
                    else
                    {
                        notificador.ShowWarningNotification("No se completó el registro de receta.");
                        return;
                    }
                }
                else
                {
                    // Usuario canceló desde el diálogo inicial
                    return;
                }
            }

            // Guardar el producto
                bool actualizado = new ProductoDAO().ActualizarProducto(productoActual, rutaImagenNueva);

            // Si se actualizó exitosamente
            if (actualizado)
            {
                if (seEliminaraReceta && recetaIdAEliminar != null)
                {
                    RecetaDAO.EliminarReceta(recetaIdAEliminar.Value); // ✅ usamos el valor guardado
                }

                new VentanaEmergente("Cambios guardados exitosamente.", "", "Aceptar", "", false).ShowDialog();
                DialogResult = true;
            }
            else
            {
                new VentanaEmergente("Error al guardar los cambios.", "", "Aceptar", "", false).ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
 