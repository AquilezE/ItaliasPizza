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
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System.Diagnostics; // Necesario para Debug


namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Lógica de interacción para PedidoDomicilio.xaml
    /// </summary>
    public partial class MenuPedidoDomicilio : Page
    {
        public MenuPedidoDomicilio()
        {
            InitializeComponent();
        }

        private void Seleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una dirección", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic seleccionado = dgDireccionesClientes.SelectedItem;

            int idCliente = seleccionado.ClienteID;
            int idDireccion = seleccionado.DireccionID;
            string direccionCompleta = seleccionado.Direccion;

        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(telefono))
            {
                MessageBox.Show("Por favor ingrese un número de teléfono", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                dgDireccionesClientes.ItemsSource = null;

                
                Cliente cliente = ClienteDAO.ObtenerClienteConDireccionesPorTelefono(telefono);

                if (cliente == null)
                {
                    MessageBox.Show("No se encontró un cliente con ese número de teléfono",
                        "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (cliente.Direcciones == null || !cliente.Direcciones.Any())
                {
                    MessageBox.Show("El cliente no tiene direcciones registradas",
                        "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var direccionesParaMostrar = cliente.Direcciones
                    .Where(d => d != null)
                    .Select(d => new DireccionParaMostrar
                    {
                        Nombre = cliente.Nombre,
                        Telefono = cliente.Telefono,
                        Direccion = $"{d.Calle} #{d.Numero}, {d.Colonia}, CP: {d.CodigoPostal}",
                        DireccionID = d.IdDireccion,
                        ClienteID = cliente.IdCliente
                    })
                    .ToList();

                dgDireccionesClientes.ItemsSource = direccionesParaMostrar;

                foreach (var column in dgDireccionesClientes.Columns)
                {
                    if (column.Header.ToString() == "Nombre" || column.Header.ToString() == "Teléfono")
                    {
                        column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    }
                    else if (column.Header.ToString() == "Dirección")
                    {
                        column.Width = new DataGridLength(1.5, DataGridLengthUnitType.Auto);
                    }
                    else if (column.Header.ToString().Contains("ID"))
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }

                dgDireccionesClientes.Visibility = Visibility.Visible;
                dgDireccionesClientes.UpdateLayout();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar cliente: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void IncrementarCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DecrementarCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBoxProducto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        public class DireccionParaMostrar
        {
            public string Nombre { get; set; }
            public string Telefono { get; set; }
            public string Direccion { get; set; }
            public int DireccionID { get; set; }
            public int ClienteID { get; set; }
        }


    }
}
