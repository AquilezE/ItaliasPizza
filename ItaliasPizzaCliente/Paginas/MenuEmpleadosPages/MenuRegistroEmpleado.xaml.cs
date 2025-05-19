using System;
using System.Collections.Generic;
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
using ItaliasPizzaCliente.Paginas.MenuInventarioPages;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using ItaliasPizzaCliente.Utils;

namespace ItaliasPizzaCliente.Paginas.MenuEmpleadosPages
{
    /// <summary>
    /// Lógica de interacción para RegistroEmpleado.xaml
    /// </summary>
    public partial class MenuRegistroEmpleado : Page
    {

      
        public MenuRegistroEmpleado()
        {
            InitializeComponent();
        }
        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            

            Validador validador = new Validador();
            string mensajeErrorUsuario = validador.ValidarNombreUsuario(txtUsuario.Text.Trim());

            if (!string.IsNullOrWhiteSpace(mensajeErrorUsuario))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowWarningNotification(mensajeErrorUsuario);
                return;
            }

            string mensajeErrorContrasena = validador.ValidarContrasenia(txtContraseña.Password.Trim());

            if (!string.IsNullOrWhiteSpace(mensajeErrorContrasena))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowWarningNotification(mensajeErrorContrasena);
                return;
            }


            try
            {
                var nuevoEmpleado = new Empleado
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellidos = txtApellidos.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    IdCargo = ObtenerIdCargoSeleccionado(),
                    Status = true 
                };

                var nuevaCuenta = new CuentaAcceso
                {
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contraseña = txtContraseña.Password 
                };

                bool registroExitoso = EmpleadoDAO.RegistrarEmpleadoConCuenta(nuevoEmpleado, nuevaCuenta);

                if (registroExitoso)
                {
                    MessageBox.Show("Empleado registrado exitosamente.", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();

                    
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el empleado. Verifique que el teléfono o nombre de usuario no estén ya registrados.", "Error en registro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar empleado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int ObtenerIdCargoSeleccionado()
        {

            string cargoSeleccionado = (cbCargo.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (cargoSeleccionado)
            {
                case "Gerente":
                    return 1; 
                case "Cajero":
                    return 2; 
                case "Cocinero":
                    return 3;
                case "Repartidor":
                    return 4;
                case "Mesero":
                    return 5;
                default:
                    return 0; 
            }

        }

        

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtContraseña.Password = string.Empty;
            cbCargo.SelectedIndex = -1;
        }
    }

}
