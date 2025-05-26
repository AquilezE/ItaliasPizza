using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using MahApps.Metro.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ItaliasPizzaCliente.Paginas.MenuInventarioPages
{
    /// <summary>
    /// Interaction logic for MenuRegistrarInsumo.xaml
    /// </summary>
    public partial class MenuRegistrarInsumo : Page
    {
        public MenuRegistrarInsumo()
        {
            InitializeComponent();
            CargarCategorias();
            CargarUnidadesDeMedida();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidarFormatos())
            {
                return;
            }

            var resultadoInsumoActivo = InsumoDAO.ValidarInsumoPorNombreActivo(txtNombre.Text);

            if (resultadoInsumoActivo == 1)
            {
                new DialogoNotificacion().ShowErrorNotification("Ya hay un insumo registrado con el nombre dado");
                return;
            }

            var resultadoInsumoDesactivado = InsumoDAO.ValidarInsumoPorNombreDesactivado(txtNombre.Text);

            if (resultadoInsumoDesactivado == 1)
            {
                VentanaEmergente ventana = new VentanaEmergente(
                    "Insumo desactivado",
                    "Este insumo ya existe pero está desactivado. ¿Deseas activarlo?",
                    "Sí, activar",
                    "No",
                    true
                );

                bool? dialogoResultado = ventana.ShowDialog();

                if (dialogoResultado == true && ventana.Resultado)
                {
                    
                    InsumoDAO.ActivarInsumo(txtNombre.Text);
                    new DialogoNotificacion().ShowSuccessNotification("Insumo activado correctamente");
                    LimpiarCampos();
                    return;
                }
                else
                {
                    return;
                }

            }

            var nuevoInsumo = new Insumo
            {
                Nombre = txtNombre.Text,
                Status = true,
                Cantidad = float.Parse(txtCantidad.Text),
                IdCategoriaInsumo = (int)comboBoxCategoria.SelectedValue,
                IdUnidadDeMedida = (int)comboBoxUnidadMedida.SelectedValue,
                Precio = float.Parse(txtPrecioUnitario.Text)
            };

            InsumoDAO.CrearInsumo(nuevoInsumo);
            new DialogoNotificacion().ShowSuccessNotification("Insumo registrado correctamente");
            LimpiarCampos();
        }
        private void Button_ClickCancelar(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void CargarCategorias()
        {
            var categorias = CategoriaInsumoDAO.ObtenerCategoriasInsumo()
        .Select(c => new
        {
            IdCategoriaInsumo = c.IdCategoriaInsumo,
            CategoriaInsumoNombre = c.CategoriaInsumoNombre
        }).ToList();


            comboBoxCategoria.ItemsSource = categorias;
            comboBoxCategoria.SelectedValuePath = "IdCategoriaInsumo";
            comboBoxCategoria.SelectedIndex = 0;
            comboBoxCategoria.SelectedItem = categorias.FirstOrDefault();
        }

        private void CargarUnidadesDeMedida()
        {
            var unidades = UnidadDeMedidaDAO.ObtenerUnidadesDeMedida()
        .Select(u => new
        {
            IdUnidadDeMedida = u.IdUnidadDeMedida,
            UnidadDeMedidaNombre = u.UnidadDeMedidaNombre
        }).ToList();

            comboBoxUnidadMedida.ItemsSource = unidades;
            comboBoxUnidadMedida.SelectedValuePath = "IdUnidadDeMedida";
            comboBoxUnidadMedida.SelectedIndex = 0;
            comboBoxUnidadMedida.SelectedItem = unidades.FirstOrDefault();
        }

        private bool ValidarFormatos()
        {
            Validador validador = new Validador();

            string mensajeError = validador.ValidarNombreInsumo(txtNombre.Text);

            if (!string.IsNullOrEmpty(mensajeError))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeError);
                return false;
            }

            if (!(comboBoxCategoria.SelectedValue is int) || !(comboBoxUnidadMedida.SelectedValue is int))
            {
                new DialogoNotificacion().ShowErrorNotification("Seleccione categoría y unidad válidas");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecioUnitario.Text))
            {
                new DialogoNotificacion().ShowErrorNotification("Ingrese cantidad y precio unitario");
                return false;
            }

            return true;
        }

        private void SoloNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^[\d ]*$") || newText.Replace(" ", "").Length > 5;
        }

        private void SoloDecimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^[\d ]*(\.\d{0,2})?$") ||
                        newText.Replace(" ", "").Length > 8;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
            comboBoxCategoria.SelectedIndex = -1;
            comboBoxUnidadMedida.SelectedIndex = -1;
        }
    }
}