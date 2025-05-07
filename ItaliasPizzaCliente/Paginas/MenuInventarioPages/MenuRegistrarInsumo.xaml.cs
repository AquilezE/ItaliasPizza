using ItaliasPizzaCliente.Singletons;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrarInsumo();
        }

        private void CargarCategorias()
        {
            var categorias = CategoriaInsumoDAO.ObtenerCategoriasInsumo();

            comboBoxCategoria.ItemsSource = categorias;
            comboBoxCategoria.DisplayMemberPath = "CategoriaInsumoNombre";
            comboBoxCategoria.SelectedValuePath = "IdCategoriaInsumo";
        }

        private void CargarUnidadesDeMedida()
        {
            var unidadesDeMedida = CategoriaInsumoDAO.ObtenerUnidadesDeMedidas();

            comboBoxUnidadDeMedida.ItemsSource = unidadesDeMedida;
            comboBoxUnidadDeMedida.DisplayMemberPath = "IdUnidadDeMedida";
            comboBoxUnidadDeMedida.SelectedValuePath = "UnidadDeMedidaNombre";
        }

        private void RegistrarInsumo()
        {
            var nuevoInsumo = new Insumo
            {
                Nombre = txtNombre.Text,
                Status = true,

                //Cantidad = txtCantidadInicial.Text,

                IdCategoriaInsumo = (int)comboBoxCategoria.SelectedValue,
                IdUnidadDeMedida = (int)comboBoxUnidadDeMedida.SelectedValue
            };

            ValidarNombreInsumo(nuevoInsumo);
            RegistrarInsumoEnBaseDeDatos(nuevoInsumo);
        }

        private void ValidarNombreInsumo(Insumo insumo)
        {
            Validador validador = new Validador();

            string mensajeError = validador.ValidarNombreUsuario(txtNombre.Text);

            if (!string.IsNullOrEmpty(mensajeError))
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification(mensajeError);
                return;
            }
        }

        private void RegistrarInsumoEnBaseDeDatos(Insumo insumo)
        {
           
                bool resultado = InsumoDAO.CrearInsumo(insumo);
            
        }

        private async Task<bool> ValidarNombreInsumo(string usuario, string contrasenia)
        {
            bool existeCuenta = await CuentaAccesoDAO.VerificarCuenta(usuario, contrasenia);

            if (!existeCuenta) return false;

            Empleado empleado = await EmpleadoDAO.ObtenerEmpleadoPorCuentaAcceso(usuario);

            UsuarioSingleton.SetUsuario(empleado);
            return true;
        }
    }
}
