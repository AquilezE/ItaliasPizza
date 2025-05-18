
using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using ItaliasPizzaDB.DataAccessObjects;

namespace ItaliasPizzaCliente.Paginas.MenuFinanzasPages
{
    public partial class MenuRegistroSalida : Page
    {
        private int idEmpleado = 1; 

        public MenuRegistroSalida()
        {
            InitializeComponent();
        }

        private async void Registrar_Click(object sender, RoutedEventArgs e)
        {
            
            if (!ValidarCampos())
            {
                return;
            }

            try
            {
                
                float cantidad = float.Parse(txtCantidad.Text, CultureInfo.InvariantCulture);
                string descripcion = txtDescripcion.Text;
                int idTipoTransaccion = ObtenerIdTipoTransaccion();

                bool registroExitoso = await SalidaDAO.RegistrarTransaccion(
                    cantidad, descripcion, idEmpleado, idTipoTransaccion);

                if (registroExitoso)
                {
                    int resultadoGasto = SalidaDAO.RegistrarGastoEnCorteDeCaja(cantidad);
                    MessageBox.Show("Salida registrada correctamente", "Éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la salida", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }

                
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor ingrese una cantidad válida", "Error de formato",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Por favor ingrese la cantidad", "Campo requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCantidad.Focus();
                return false;
            }

            if (cbTipoTransaccion.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione el tipo de transacción", "Campo requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cbTipoTransaccion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor ingrese una descripción", "Campo requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDescripcion.Focus();
                return false;
            }

            return true;
        }

        private int ObtenerIdTipoTransaccion()
        {
            string tipoSeleccionado = (cbTipoTransaccion.SelectedItem as ComboBoxItem).Content.ToString();

            switch (tipoSeleccionado)
            {
                case "Luz": return 2;
                case "Agua": return 3;
                case "Internet": return 4;
                case "Gasolina": return 5;
                case "Transporte": return 6;
                case "Salud": return 7;
                default: return 0; 
            }
        }

        private void LimpiarCampos()
        {
            txtCantidad.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cbTipoTransaccion.SelectedIndex = -1;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}