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

namespace ItaliasPizzaCliente.Paginas.MenuClientePages
{
    /// <summary>
    /// Lógica de interacción para MenuRegistrarClientes.xaml
    /// </summary>
    public partial class MenuRegistrarClientes : Page
    {
        public MenuRegistrarClientes()
        {
            InitializeComponent();
        }

        private void Button_RegistrarClick(object sender, RoutedEventArgs e)
        {
            if (!ValidarFormatos())
            {
                return;
            }

            string telefonoCliente = txtTelefono.Text.Replace(" ", "");
            if (ClienteDAO.ValidarClientePorTelefono(telefonoCliente) == 1)
            {
                new DialogoNotificacion().ShowErrorNotification("Ya existe un cliente con ese número de teléfono.");
                return;
            }

            Cliente nuevoCliente = new Cliente
            {
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Telefono = telefonoCliente,
                Status = true,

            };

            Cliente creado = ClienteDAO.CrearCliente(nuevoCliente);
            if (creado ==null)
            {
                new DialogoNotificacion().ShowErrorNotification("No se pudo guardar el cliente.");
                return;
            }

            int idCliente = creado.IdCliente;

            Direccion nuevaDireccion = new Direccion
            {
                Calle = txtCalle.Text,
                Colonia = txtColonia.Text,
                Referencia = txtReferencia.Text,
                Numero = int.Parse(txtNumero.Text.Replace(" ", "")),
                CodigoPostal = txtCP.Text,
                Ciudad = txtCiudad.Text,
                Estado = txtEstado.Text,
                Status = true,
                IdCliente = idCliente
            };

            if (!ClienteDAO.AgregarDireccion(nuevaDireccion))
            {
                new DialogoNotificacion().ShowErrorNotification("No se pudo guardar la dirección.");
                return;
            }

            new DialogoNotificacion().ShowSuccessNotification("Cliente y dirección registrados correctamente.");
            LimpiarCampos();
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

            mensajeErrorDireccion = validador.ValidarDireccion(txtReferencia.Text);

            if (!string.IsNullOrEmpty(mensajeErrorDireccion))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorDireccion);
                return false;
            }

            mensajeErrorDireccion = validador.ValidarDireccion(txtCiudad.Text);

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

            string mensajeErrorNombre = validador.ValidarNombrePersona(txtNombre.Text);

            if (!string.IsNullOrEmpty(mensajeErrorNombre))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorNombre);
                return false;
            }

            mensajeErrorNombre = validador.ValidarNombrePersona(txtApellidos.Text);

            if (!string.IsNullOrEmpty(mensajeErrorNombre))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeErrorNombre);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumero.Text) || string.IsNullOrWhiteSpace(txtCP.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                new DialogoNotificacion().ShowErrorNotification("Asegurse que todos los campos");
                return false;
            }

            if (ValidarCamposNumericos())
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
            string telefono = txtTelefono.Text.Replace(" ", "");
            string cp = txtCP.Text.Replace(" ", "");

            if (telefono.Length < 10)
            {
                new DialogoNotificacion().ShowWarningNotification("El número de teléfono debe tener al menos 10 dígitos.");
                return false;
            }

            if (cp.Length != 5)
            {
                new DialogoNotificacion().ShowWarningNotification("El código postal debe tener 5 dígitos."); ;
                return false;
            }


            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtApellidos.Clear();
            txtCalle.Clear();
            txtColonia.Clear();
            txtReferencia.Clear();
            txtNumero.Clear();
            txtCP.Clear();
            txtCiudad.Clear();
            txtEstado.Clear();
        }

    }
}
