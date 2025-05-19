using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ItaliasPizzaCliente.Utils;

namespace ItaliasPizzaCliente.Paginas.MenuEmpleadosPages
{
    public partial class MenuModificacionEmpleado : Page
    {
        private Empleado _empleadoActual;
        private bool _esBusquedaRealizada = false;

        public MenuModificacionEmpleado()
        {
            InitializeComponent();
            CargarCargos();
        }



        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            //CargarCargos();
            string usuarioBusqueda = txtBuscarUsuario.Text.Trim();

            if (string.IsNullOrEmpty(usuarioBusqueda))
            {
                MessageBox.Show("Por favor ingrese un nombre de usuario para buscar", "Advertencia",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _empleadoActual = EmpleadoDAO.ObtenerEmpleadoPorUsuario(usuarioBusqueda);

                if (_empleadoActual == null)
                {
                    MessageBox.Show("No se encontró un empleado con ese nombre de usuario", "Información",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                txtNombre.Text = _empleadoActual.Nombre;
                txtApellidos.Text = _empleadoActual.Apellidos;
                txtTelefono.Text = _empleadoActual.Telefono;

                if (_empleadoActual.Cargo != null)
                {
                    cbCargo.SelectedValue = _empleadoActual.IdCargo;
                }

                if (_empleadoActual.CuentaAcceso != null)
                {
                    txtUsuario.Text = _empleadoActual.CuentaAcceso.NombreUsuario;
                }

                _esBusquedaRealizada = true;
                MessageBox.Show("Empleado encontrado. Puede modificar los datos.", "Éxito",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar empleado: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (!_esBusquedaRealizada || _empleadoActual == null)
            {
                MessageBox.Show("Debe buscar un empleado primero antes de modificar", "Advertencia",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Validador validador = new Validador();
            string mensajeErrorUsuario = validador.ValidarNombreUsuario(txtUsuario.Text.Trim());

            if (!string.IsNullOrWhiteSpace(mensajeErrorUsuario))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowWarningNotification(mensajeErrorUsuario);
                return;
            }

            string mensajeErrorContrasena = validador.ValidarContrasenia(pwdContrasena.Password.Trim());

            if (!string.IsNullOrWhiteSpace(mensajeErrorContrasena))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowWarningNotification(mensajeErrorContrasena);
                return;
            }

            Empleado empleadoEditado = new Empleado
            {
                IdEmpleado = _empleadoActual.IdEmpleado,
                Nombre = txtNombre.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                IdCargo = ObtenerIdCargoSeleccionado(),
                Status = true
            };

            CuentaAcceso cuentaEditada = null;

            if (!string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                string nuevaContrasena = pwdContrasena.Password;
                string contrasenaActual = _empleadoActual.CuentaAcceso?.Contraseña;

                string contrasenaFinal = !string.IsNullOrEmpty(nuevaContrasena)
                                        ? nuevaContrasena
                                        : contrasenaActual;

                cuentaEditada = new CuentaAcceso
                {
                    IdEmpleado = _empleadoActual.IdEmpleado,
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contraseña = contrasenaFinal
                };
            }

            bool resultado = EmpleadoDAO.ModificarEmpleado(empleadoEditado, cuentaEditada);

            if (resultado)
            {
                MessageBox.Show("Empleado modificado con éxito", "Éxito",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarCampos();
                _esBusquedaRealizada = false;
                _empleadoActual = null;
            }
            else
            {
                MessageBox.Show("No se pudo modificar el empleado. Verifique que los datos sean correctos y no estén duplicados.",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int ObtenerIdCargoSeleccionado()
        {
            if (cbCargo.SelectedItem is Cargo cargoSeleccionado)
            {
                return cargoSeleccionado.IdCargo;
            }
            return 0;
        }


        private bool EsTelefonoValido(string telefono)
        {
            return telefono.Length == 10 && telefono.All(char.IsDigit);
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cbCargo.SelectedIndex = -1;
            txtUsuario.Text = string.Empty;
            pwdContrasena.Password = string.Empty;
            txtBuscarUsuario.Text = string.Empty;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void CargarCargos()
        {
            try
            {
                cbCargo.ItemsSource = EmpleadoDAO.ConsultarCargos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
}