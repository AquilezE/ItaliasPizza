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
    /// Lógica de interacción para MenuModificarCliente.xaml
    /// </summary>
    public partial class MenuModificarCliente : Page
    {
        Cliente clienteObtenido;
        public MenuModificarCliente(Cliente cliente)
        {
            InitializeComponent();
            clienteObtenido = cliente;
            txtNombre.Text = cliente.Nombre;
            txtApellidos.Text = cliente.Apellidos;
           txtTelefono.Text = cliente.Telefono;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarFormatos() == false)
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
                IdCliente = clienteObtenido.IdCliente

            };

            bool creado = ClienteDAO.ActualizarCliente(nuevoCliente);
            if (!creado)
            {
                new DialogoNotificacion().ShowErrorNotification("No se pudo guardar el cliente.");
                return;
            }
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

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                new DialogoNotificacion().ShowErrorNotification("Asegurse que todos los campos estén llenos");
                return false;
            }

            if (ValidarCamposNumericos())
            {
                return false;
            }

            return true;
        }

        private bool ValidarCamposNumericos()
        {
            string telefono = txtTelefono.Text.Replace(" ", "");

            if (telefono.Length < 10)
            {
                new DialogoNotificacion().ShowWarningNotification("El número de teléfono debe tener al menos 10 dígitos.");
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

    }
}
