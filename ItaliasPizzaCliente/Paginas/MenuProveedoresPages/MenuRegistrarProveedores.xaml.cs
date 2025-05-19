using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
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

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{
    /// <summary>
    /// Lógica de interacción para MenuRegistrarProveedores.xaml
    /// </summary>
    public partial class MenuRegistrarProveedores : Page
    {

        private List<string> insumosAgregados = new List<string>();

        public MenuRegistrarProveedores()
        {
            InitializeComponent();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarFormatos())
            {
                return;
            }

            string telefonoProveedor= txtTelefonoProveedor.Text.Replace(" ", "");
            if (ProveedorDAO.ValidarProveedorPorTelefono(telefonoProveedor) == 1)
            {
                new DialogoNotificacion().ShowErrorNotification("Ya existe un proveedor con ese número de teléfono.");
                return;
            }
            string nombreProveedor = txtNombreProveedor.Text;
            if (ProveedorDAO.ValidarProveedorPorNombre(nombreProveedor) == 1)
            {
                new DialogoNotificacion().ShowErrorNotification("Ya existe un proveedor con ese nombre");
                return;
            }

            string direccionCompleta = $"{txtCalle.Text} #{txtNumero.Text.Replace(" ", "")}, {txtColonia.Text}, C.P. {txtCP.Text}, {txtEstado.Text}";

            Proveedor nuevoProveedor = new Proveedor
            {
                Nombre = txtNombreProveedor.Text,
                Direccion = direccionCompleta,
                Telefono = txtTelefonoProveedor.Text,

            };

            Proveedor creado = ProveedorDAO.CrearProveedor(nuevoProveedor);
            if (creado == null)
            {
                new DialogoNotificacion().ShowErrorNotification("No se pudo guardar el proveedor.");
                return;
            }

            foreach (string nombreInsumo in insumosAgregados)
            {
                var insumo = InsumoDAO.BuscarInsumoPorNombre(nombreInsumo);

                if (insumo != null)
                {
                    ProveedorInsumo proveedorInsumo = new ProveedorInsumo
                    {
                        IdProveedor = creado.IdProveedor,
                        IdInsumo = insumo.IdInsumo
                    };

                    ProveedorDAO.AgregarInsumoAProveedor(proveedorInsumo);
                }
            }
            new DialogoNotificacion().ShowSuccessNotification("Proveedor e insumos registrados correctamente.");
            LimpiarCampos();
        }

        private void BuscarInsumo_Click(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBuscarInsumo.Text.Trim();

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                new DialogoNotificacion().ShowWarningNotification("Ingresa un término de búsqueda.");
                return;
            }

            var insumoEncontrado = InsumoDAO.BuscarInsumoPorNombre(textoBusqueda);

            if (insumoEncontrado != null)
            {
                txtBuscarInsumo.Text = insumoEncontrado.Nombre;
            }
            else
            {
                new DialogoNotificacion().ShowWarningNotification("Insumo no encontrado.");
            }
        }

        private void AgregarInsumo_Click(object sender, RoutedEventArgs e)
        {
            string insumo = txtBuscarInsumo.Text.Trim();

            var insumoEncontrado = InsumoDAO.BuscarInsumoPorNombre(insumo);

            if (insumoEncontrado != null)
            {
                txtBuscarInsumo.Text = insumoEncontrado.Nombre;
            }
            else
            {
                new DialogoNotificacion().ShowWarningNotification("Insumo no encontrado.");
                return;
            }

            if (string.IsNullOrWhiteSpace(insumo))
            {
                new DialogoNotificacion().ShowWarningNotification("Debes buscar o escribir un insumo primero.");
                return;
            }

            if (insumosAgregados.Contains(insumo))
            {
                new DialogoNotificacion().ShowWarningNotification("Este insumo ya fue agregado.");
                return;
            }

            insumosAgregados.Add(insumo);
            lstInsumos.Items.Add(insumo);
            txtBuscarInsumo.Clear();
        }

        private void EliminarInsumo_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            string insumo = (boton.DataContext as string);

            if (insumo != null)
            {
                insumosAgregados.Remove(insumo);
                lstInsumos.Items.Remove(insumo);
            }
        }

        private bool ValidarFormatos()
        {
            Validador validador = new Validador();

            string mensajeError = validador.ValidarNombreInsumo(txtNombreProveedor.Text);

            if (!string.IsNullOrEmpty(mensajeError))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeError);
                return false;
            }

            string mensajeErrorDireccion = validador.ValidarDireccion(txtCalle.Text);

            if (!string.IsNullOrEmpty(mensajeErrorDireccion))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorDireccion);
                return false;
            }
            mensajeErrorDireccion = validador.ValidarDireccion(txtColonia.Text);

            if (!string.IsNullOrEmpty(mensajeErrorDireccion))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorDireccion);
                return false;
            }

            mensajeErrorDireccion = validador.ValidarDireccion(txtEstado.Text);

            if (!string.IsNullOrEmpty(mensajeErrorDireccion))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorDireccion);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumero.Text) || string.IsNullOrWhiteSpace(txtCP.Text) || string.IsNullOrWhiteSpace(txtTelefonoProveedor.Text))
            {
                new DialogoNotificacion().ShowErrorNotification("Asegurse que todos los campos");
                return false;
            }

            if (!ValidarCamposNumericos())
            {
                return false;
            }

            return true;
        }

        private void SoloNumerosTelefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^[\d ]*$") || newText.Replace(" ", "").Length > 10;
        }

        private void SoloNumerosCasa_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^[\d ]*$") || newText.Replace(" ", "").Length > 3;
        }

        private void SoloNumerosCP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !Regex.IsMatch(newText, @"^[\d ]*$") || newText.Replace(" ", "").Length > 5;
        }

        private bool ValidarCamposNumericos()
        {
            string telefono = txtTelefonoProveedor.Text.Replace(" ", "");
            string numero = txtNumero.Text.Replace(" ", "");
            string cp = txtCP.Text.Replace(" ", "");

            if (telefono.Length < 10)
            {
                new DialogoNotificacion().ShowWarningNotification("El número de teléfono debe tener al menos 10 dígitos.");
                return false;
            }

            if (cp.Length != 5)
            {
                new DialogoNotificacion().ShowWarningNotification("El código postal debe tener 5 dígitos.");;
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombreProveedor.Clear();
            txtTelefonoProveedor.Clear();
            txtCalle.Clear();
            txtColonia.Clear();
            txtEstado.Clear();
            txtNumero.Clear();
            txtCP.Clear();
            txtBuscarInsumo.Clear();
            lstInsumos.Items.Clear();
            insumosAgregados.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }
    }
}
