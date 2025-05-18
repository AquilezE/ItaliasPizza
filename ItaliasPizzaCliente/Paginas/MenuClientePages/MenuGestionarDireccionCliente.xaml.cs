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
    /// Lógica de interacción para MenuGestionarDireccionCliente.xaml
    /// </summary>
    public partial class MenuGestionarDireccionCliente : Page
    {

        int tipoDeAccion;
        int idClienteObtenido;

        public MenuGestionarDireccionCliente(Direccion direccion)
        {
            InitializeComponent();
            var direccionObetenida = direccion;

            if (direccionObetenida == null)
            {
                btnAccion.Content = "Registrar";
                tipoDeAccion = 1;
            }
            else
            {
                btnAccion.Content = "Actualizar";

                txtCalle.Text = direccionObetenida.Calle;
                txtColonia.Text = direccionObetenida.Colonia;
                txtNumero.Text = direccionObetenida.Numero.ToString();
                txtCP.Text = direccionObetenida.CodigoPostal;
                txtReferencia.Text = direccionObetenida.Referencia;
                txtCiudad.Text = direccionObetenida.Ciudad;
                txtEstado.Text = direccionObetenida.Estado;
                idClienteObtenido = direccionObetenida.IdCliente;
               

                tipoDeAccion = 2;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarFormatos() == false)
            {
                return;
            }

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
                IdCliente = idClienteObtenido
            };

            var direccionRepetida = ClienteDAO.ValidarDireccionRepetida(nuevaDireccion);
            if (direccionRepetida == true)
            {
                new DialogoNotificacion().ShowErrorNotification("Esa dirección ya está registrada para este cliente");
                return;
            }

            if (tipoDeAccion == 1)
            {
                if (!ClienteDAO.AgregarDireccion(nuevaDireccion))
                {
                    new DialogoNotificacion().ShowErrorNotification("No se pudo guardar la dirección.");
                    return;
                }

                new DialogoNotificacion().ShowSuccessNotification("Dirección registrados correctamente.");
                LimpiarCampos();
            }
            else
            {
                if (!ClienteDAO.ActualizarDireccionDeClientePorId(nuevaDireccion))
                {
                    new DialogoNotificacion().ShowErrorNotification("No se pudo guardar la dirección.");
                    return;
                }

                new DialogoNotificacion().ShowSuccessNotification("Dirección actualizada correctamente.");
                LimpiarCampos();
            }
        }

        private bool ValidarCamposNumericos()
        {
            string cp = txtCP.Text.Replace(" ", "");

            if (cp.Length != 5)
            {
                new DialogoNotificacion().ShowWarningNotification("El código postal debe tener 5 dígitos."); ;
                return false;
            }

            return true;
        }

        private bool ValidarFormatos()
        {
            Validador validador = new Validador();

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

            if (string.IsNullOrWhiteSpace(txtNumero.Text) || string.IsNullOrWhiteSpace(txtCP.Text))
            {
                new DialogoNotificacion().ShowErrorNotification("Asegurse que todos los campos este llenos");
                return false;
            }

            if (!ValidarCamposNumericos() == false)
            {
                return false;
            }

            return true;
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

        private void LimpiarCampos()
        { 
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
