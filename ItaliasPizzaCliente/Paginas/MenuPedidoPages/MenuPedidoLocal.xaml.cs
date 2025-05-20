using ItaliasPizzaCliente.Paginas.MenuInventarioPages;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static ItaliasPizzaCliente.Paginas.MenuPedidoPages.MenuPedidoDomicilio;
using ItaliasPizzaCliente.Utils;
using Microsoft.Win32;

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Lógica de interacción para PedidoLocal.xaml
    /// </summary>
    public partial class MenuPedidoLocal : Page
    {
        public MenuPedidoLocal()
        {
            InitializeComponent();
            CargarProductos();
            CargarMesasManual();
        }

        private void cbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (cbMesa.SelectedItem == null || dgProductosSeleccionados.Items.Count == 0)
            {
                new DialogoNotificacion().ShowErrorNotification("Por favor seleccione una mesa y al menos un producto.");
                return;
            }

            try
            {
                var productosSeleccionados = dgProductosSeleccionados.ItemsSource as ObservableCollection<ProductoSeleccionado>;
                var detalles = productosSeleccionados.Select(p =>
                    (p.IdProducto, p.Cantidad, p.Subtotal)).ToList();
                int idEmpleado = 2;
                int mesa = (int)cbMesa.SelectedItem;

                float totalPedido = productosSeleccionados.Sum(p => p.Subtotal);

                int idPedido = PedidoDAO.RegistrarPedidoParaLocal(idEmpleado, mesa, detalles);

                if (idPedido <= 0)
                {
                    new DialogoNotificacion().ShowErrorNotification("Error al registrar el pedido.");
                    return;
                }

                int resultadoInventario = PedidoDAO.ActualizarInventarioPorPedido(idPedido);

                if (resultadoInventario == 0)
                {
                    int resultadoVenta = CorteDeCajaDAO.AgregarVentaAlCorteDelDia(totalPedido);

                    if (resultadoVenta != 0)
                    {
                        string mensajeAdvertencia;
                        if (resultadoVenta == -1)
                            mensajeAdvertencia = "El pedido se registró pero no hay corte de caja abierto";
                        else if (resultadoVenta == -2)
                            mensajeAdvertencia = "El pedido se registró pero el monto es inválido";
                        else
                            mensajeAdvertencia = "El pedido se registró pero hubo un problema al actualizar el corte de caja";

                        MessageBox.Show(mensajeAdvertencia, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    new DialogoNotificacion().ShowSuccessNotification($"Pedido registrado exitosamente. Número de pedido: {idPedido}");

                    dgProductosSeleccionados.ItemsSource = new ObservableCollection<ProductoSeleccionado>();
                    CalcularTotal();
                }
                else
                {
                    string mensajeError;
                    switch (resultadoInventario)
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

                    EliminarPedidoSiFallóInventario(idPedido);

                    MessageBox.Show($"Error: {mensajeError}. El pedido no pudo ser procesado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el pedido: {ex.Message}");
            }
        }

        private void EliminarPedidoSiFallóInventario(int idPedido)
        {
            try
            {
                using (var context = new ItaliasPizzaDbContext())
                {
                    var pedido = context.Pedidos.Find(idPedido);
                    if (pedido != null)
                    {
                        context.Pedidos.Remove(pedido);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar pedido fallido: {ex.Message}");
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

        private void CalcularTotal()
        {
            if (dgProductosSeleccionados.ItemsSource is ObservableCollection<ProductoSeleccionado> productos)
            {
                float total = productos.Sum(p => p.Subtotal);
                txtTotal.Text = total.ToString("C2"); //
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

        private void CargarProductos()
        {
            dgProductosDisponibles.ItemsSource = PedidoDAO.ObtenerProductosActivos();
        }

        private void CargarMesasManual()
        {
            for (int i = 1; i <= 10; i++)
            {
                cbMesa.Items.Add(i);
            }

            if (cbMesa.Items.Count > 0)
            {
                cbMesa.SelectedIndex = 0;
            }
        }

        public class ProductoSeleccionado
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public int Cantidad { get; set; } = 1;
            public float Subtotal => Precio * Cantidad; 
        }
    }
}
