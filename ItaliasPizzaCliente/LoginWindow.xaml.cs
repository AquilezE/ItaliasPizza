using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.Models;
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
using System.Windows.Shapes;
using ItaliasPizzaDB.DataAccessObjects;

namespace ItaliasPizzaCliente
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            String contrasenia = txtPassword.Password;
            String usuario = txtUsuario.Text;

            usuario = usuario.Trim();
            contrasenia = contrasenia.Trim();

            Validador validador = new Validador();

            string mensajeError = validador.ValidarNombreUsuario(usuario);

            if (!string.IsNullOrEmpty(mensajeError))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeError);
                return;
            }

            if (await ValidarCuenta(usuario, contrasenia))
            {

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification("Usuario o contraseña incorrectos");
            }

        }

        //TODO: encuentra un nombre mejor pq qpd
        private async Task<bool> ValidarCuenta(string usuario, string contrasenia)
        {
            bool existeCuenta = await CuentaAccesoDAO.VerificarCuenta(usuario, contrasenia);

            if (!existeCuenta) return false;

            Empleado empleado = await EmpleadoDAO.ObtenerEmpleadoPorCuentaAcceso(usuario);

            UsuarioSingleton.SetUsuario(empleado);
            return true;
        }
    }
}
