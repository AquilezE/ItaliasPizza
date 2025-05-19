using ItaliasPizzaCliente.Paginas.MenuInventarioPages;
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
    /// Lógica de interacción para MenuModificarProveedores.xaml
    /// </summary>
    public partial class MenuModificarProveedores : Page
    {
        private Proveedor proveedorActual;
        private List<string> insumosAgregados = new List<string>();

        public MenuModificarProveedores(Proveedor provedorObtenido)
        {
            InitializeComponent();
            InitializeComponent();
            proveedorActual = provedorObtenido;
            CargarDatosProveedor();
            CargarInsumosDelProveedor();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarFormatos() == false)
            {
                return;
            }

            string telefonoProveedor = txtTelefonoProveedor.Text.Replace(" ", "");
            if (ProveedorDAO.ValidarProveedorPorNombreDiferente(telefonoProveedor, proveedorActual.IdProveedor) == 1)
            {
                new DialogoNotificacion().ShowErrorNotification("Ya existe un proveedor con ese número de teléfono.");
                return;
            }
            string nombreProveedor = txtNombreProveedor.Text;
            if (ProveedorDAO.ValidarProveedorPorNombreDiferente(nombreProveedor, proveedorActual.IdProveedor) == 1)
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
                IdProveedor = proveedorActual.IdProveedor,

            };

            bool creado = ProveedorDAO.ActualizarProveedor(nuevoProveedor);
            if (!creado)
            {
                new DialogoNotificacion().ShowErrorNotification("No se pudo guardar el proveedor.");
                return;
            }

            new DialogoNotificacion().ShowSuccessNotification("Proveedor actualizado correctamente.");
            NavigationService?.Navigate(new MenuVisualizarProveedores());
        }

        private void AgregarInsumo_Click(object sender, RoutedEventArgs e)
        {
            string insumoNombre = txtBuscarInsumo.Text.Trim();

            if (string.IsNullOrEmpty(insumoNombre))
            {
                new DialogoNotificacion().ShowWarningNotification("Debes ingresar el nombre de un insumo.");
                return;
            }

            if (insumosAgregados.Contains(insumoNombre))
            {
                new DialogoNotificacion().ShowWarningNotification("Este insumo ya está agregado al proveedor.");
                return;
            }

            var insumo = InsumoDAO.BuscarInsumoPorNombre(insumoNombre);
            if (insumo != null)
            {
                ProveedorInsumo proveedorInsumo = new ProveedorInsumo
                {
                    IdProveedor = proveedorActual.IdProveedor,
                    IdInsumo = insumo.IdInsumo
                };

                bool agregado = ProveedorDAO.AgregarInsumoAProveedor(proveedorInsumo);
                if (agregado)
                {
                    insumosAgregados.Add(insumo.Nombre);
                    lstInsumos.Items.Add(insumo.Nombre);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MenuVisualizarProveedores());
        }

        private void EliminarInsumo_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            string insumoNombre = boton.DataContext as string;



            var insumo = InsumoDAO.BuscarInsumoPorNombre(insumoNombre);
            if (insumo != null)
            {
                ProveedorInsumo proveedorInsumo = new ProveedorInsumo
                {
                    IdProveedor = proveedorActual.IdProveedor,
                    IdInsumo = insumo.IdInsumo
                };

                bool eliminado = ProveedorDAO.EliminarInsumoDeProveedor(proveedorInsumo);
                if (eliminado)
                {
                    insumosAgregados.Remove(insumoNombre);
                    lstInsumos.Items.Remove(insumoNombre);
                }
            }
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

        private void CargarDatosProveedor()
        {
            txtNombreProveedor.Text = proveedorActual.Nombre;
            txtTelefonoProveedor.Text = proveedorActual.Telefono;

          
            string[] partes = proveedorActual.Direccion.Split(',');

            if (partes.Length >= 4)
            {
                txtCalle.Text = partes[0].Split('#')[0].Trim();
                txtNumero.Text = partes[0].Split('#').Length > 1 ? partes[0].Split('#')[1].Trim() : "";
                txtColonia.Text = partes[1].Trim();
                txtCP.Text = partes[2].Replace("C.P.", "").Trim();
                txtEstado.Text = partes[3].Trim();
            }
        }

        private void CargarInsumosDelProveedor()
        {
            var insumos = ProveedorDAO.ObtenerInsumosDeProveedor(proveedorActual.IdProveedor);

            lstInsumos.Items.Clear();
            insumosAgregados.Clear();

            foreach (var insumo in insumos)
            {
                insumosAgregados.Add(insumo.Nombre);
                lstInsumos.Items.Add(insumo.Nombre);
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
                new DialogoNotificacion().ShowWarningNotification("El código postal debe tener 5 dígitos."); ;
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
    }
}
