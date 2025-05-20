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
using System.Collections.ObjectModel;
using ItaliasPizzaCliente.Paginas.MenuClientePages;
using ItaliasPizzaCliente.Utils;


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
            CargarProductos();
        }

        private void Seleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione una dirección");
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
                new DialogoNotificacion().ShowErrorNotification("Por favor ingrese un número de teléfono");
                return;
            }

            try
            {
                dgDireccionesClientes.ItemsSource = null;

                
                Cliente cliente = ClienteDAO.ObtenerClienteConDireccionesPorTelefono(telefono);

                if (cliente == null)
                {
                    new DialogoNotificacion().ShowErrorNotification("No se encontró un cliente con ese número de teléfono");
                    return;
                }

                if (cliente.Direcciones == null || !cliente.Direcciones.Any())
                {
                    new DialogoNotificacion().ShowErrorNotification("El cliente no tiene direcciones registradas");
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
            var button = (Button)sender;
            var producto = (ProductoSeleccionado)button.Tag;
            producto.Cantidad++;
            dgProductosSeleccionados.Items.Refresh(); 
            CalcularTotal();
        }

        private void DecrementarCantidad_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var producto = (ProductoSeleccionado)button.Tag;
            if (producto.Cantidad > 1) producto.Cantidad--;
            dgProductosSeleccionados.Items.Refresh();
            CalcularTotal();
        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione una dirección");
                return;
            }

            if (!(dgProductosSeleccionados.ItemsSource is ObservableCollection<ProductoSeleccionado> productosSeleccionados)
                || !productosSeleccionados.Any())
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione al menos un producto");
                return;
            }

            try
            {
                dynamic direccionSeleccionada = dgDireccionesClientes.SelectedItem;
                int idCliente = direccionSeleccionada.ClienteID;
                int idDireccion = direccionSeleccionada.DireccionID;

                int idEmpleado = 2; 

                var detalles = productosSeleccionados
                    .Select(p => (p.IdProducto, p.Cantidad, p.Subtotal))
                    .ToList();

                int idPedido = PedidoDAO.RegistrarPedidoParaLlevar(idEmpleado, idCliente, idDireccion, detalles);

                int resultadoActualizacion = PedidoDAO.ActualizarInventarioSinReceta(idPedido);

                if (resultadoActualizacion == 0)
                {
                    float totalPedido = productosSeleccionados.Sum(p => p.Subtotal);
                    // Agregar el total del pedido al corte de caja del día
                    int resultadoVenta = CorteDeCajaDAO.AgregarVentaAlCorteDelDia(totalPedido);

                    if (resultadoVenta != 0)
                    {
                        string mensajeAdvertencia;
                        switch (resultadoVenta)
                        {
                            case -1:
                                mensajeAdvertencia = "El pedido se registró pero no se pudo agregar al corte de caja (no hay corte abierto)";
                                break;
                            case -2:
                                mensajeAdvertencia = "El pedido se registró pero el monto no se pudo agregar al corte de caja (monto inválido)";
                                break;
                            default:
                                mensajeAdvertencia = "El pedido se registró pero hubo un problema al agregar al corte de caja";
                                break;
                        }

                        MessageBox.Show(mensajeAdvertencia, "Advertencia",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    new DialogoNotificacion().ShowSuccessNotification($"Pedido registrado exitosamente. Número de pedido: {idPedido}");

                    dgDireccionesClientes.SelectedItem = null;
                    dgProductosSeleccionados.ItemsSource = new ObservableCollection<ProductoSeleccionado>();
                    txtTotal.Text = "$0.00";
                }
                else
                {
                    string mensajeError;
                    switch (resultadoActualizacion)
                    {
                        case 1:
                            mensajeError = "Pedido no encontrado";
                            break;
                        case 2:
                            mensajeError = "Estado del pedido no válido";
                            break;
                        case 4:
                            mensajeError = "No hay suficiente inventario";
                            break;
                        case 5:
                            mensajeError = "Error en la operación";
                            break;
                        case 6:
                            mensajeError = "Insumo no encontrado";
                            break;
                        default:
                            mensajeError = "Error desconocido al actualizar inventario";
                            break;
                    }

                    MessageBox.Show($"Pedido registrado pero hubo un problema al actualizar el inventario: {mensajeError}",
                        "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CheckBoxProducto_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var producto = (Producto)checkBox.DataContext;

            var productosSeleccionados = dgProductosSeleccionados.ItemsSource as ObservableCollection<ProductoSeleccionado>
                                         ?? new ObservableCollection<ProductoSeleccionado>();

            if (checkBox.IsChecked == true)
            {
                if (!productosSeleccionados.Any(p => p.IdProducto == producto.IdProducto))
                {
                    productosSeleccionados.Add(new ProductoSeleccionado
                    {
                        IdProducto = producto.IdProducto,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio
                    });
                }
            }
            else
            {
                var item = productosSeleccionados.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
                if (item != null) productosSeleccionados.Remove(item);
            }

            dgProductosSeleccionados.ItemsSource = productosSeleccionados;
            CalcularTotal();
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

        public class ProductoSeleccionado
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public int Cantidad { get; set; } = 1; 
            public float Subtotal => Precio * Cantidad; 
        }

        private void CargarProductos()
        {
            dgProductosDisponibles.ItemsSource = PedidoDAO.ObtenerProductosActivos();
        }

        private void CalcularTotal()
        {
            if (dgProductosSeleccionados.ItemsSource is ObservableCollection<ProductoSeleccionado> productos)
            {
                float total = productos.Sum(p => p.Subtotal);
                txtTotal.Text = total.ToString("C2"); 
            }
        }

        private void AgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuRegistrarClientes paginaRegistrar = new MenuRegistrarClientes();

                this.NavigationService.Navigate(paginaRegistrar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la página de registro de clientes: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModificarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione un cliente primero");
                return;
            }

            dynamic seleccionado = dgDireccionesClientes.SelectedItem;
            int idCliente = seleccionado.ClienteID;

            try
            {
                Cliente cliente = ClienteDAO.ObtenerClientePorId(idCliente);

                if (cliente == null)
                {
                    new DialogoNotificacion().ShowErrorNotification("No se pudo obtener la información del cliente");
                    return;
                }

                MenuModificarCliente paginaModificar = new MenuModificarCliente(cliente);

                this.NavigationService.Navigate(paginaModificar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la página de modificación: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AgregarDireccion_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione un cliente primero");
                return;
            }

            try
            {
                dynamic seleccionado = dgDireccionesClientes.SelectedItem;
                int idCliente = seleccionado.ClienteID;

                var paginaGestionDireccion = new ItaliasPizzaCliente.Paginas.MenuClientePages.MenuGestionarDireccionCliente(null);
                
                paginaGestionDireccion.idClienteObtenido = idCliente;

                this.NavigationService.Navigate(paginaGestionDireccion);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar agregar dirección: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ModificarDireccion_Click(object sender, RoutedEventArgs e)
        {
            if (dgDireccionesClientes.SelectedItem == null)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione una dirección");
                return;
            }

            try
            {
                dynamic seleccionado = dgDireccionesClientes.SelectedItem;
                int idDireccion = seleccionado.DireccionID;

                Direccion direccion = ClienteDAO.ObtenerDireccionPorId(idDireccion);

                if (direccion == null)
                {
                    new DialogoNotificacion().ShowErrorNotification("No se pudo obtener la información de la dirección");
                    return;
                }

                var paginaGestionDireccion = new ItaliasPizzaCliente.Paginas.MenuClientePages.MenuGestionarDireccionCliente(direccion);

                this.NavigationService.Navigate(paginaGestionDireccion);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar modificar dirección: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
