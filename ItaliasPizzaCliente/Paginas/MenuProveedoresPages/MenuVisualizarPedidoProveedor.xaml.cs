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

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{
    /// <summary>
    /// Lógica de interacción para MenuVisualizarPedidoProveedor.xaml
    /// </summary>
    public partial class MenuVisualizarPedidoProveedor : Page
    {
        public MenuVisualizarPedidoProveedor()
        {
            InitializeComponent();
        }

        public class PedidoProveedorVisual
        {
            public int IdPedidoProveedor { get; set; }
            public string NombreProveedor { get; set; }
            public DateTime FechaPedido { get; set; }
            public float Total { get; set; }
            public string Status { get; set; }
        }


        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (DpDesde.SelectedDate == null || DpHasta.SelectedDate == null)
            {
                MessageBox.Show("Selecciona ambas fechas.");
                return;
            }

            DateTime desde = DpDesde.SelectedDate.Value.Date;
            DateTime hasta = DpHasta.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            var pedidos = PedidoProveedorDAO.ObtenerPedidosPorRango(desde, hasta);

            var visuales = pedidos.Select(p => new PedidoProveedorVisual
            {
                IdPedidoProveedor = p.IdPedidoProveedor,
                NombreProveedor = p.Proveedor?.Nombre ?? "Desconocido",
                FechaPedido = p.FechaPedido,
                Total = p.Total,
                Status = ((StatusPedidoEnum)p.Status).ToString()
            }).ToList();

            dgPedidos.ItemsSource = visuales;
        }


        private void BtnEntregado_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pedido = button?.Tag as PedidoProveedorVisual;



            if (pedido != null)
            {
                PedidoProveedorDAO.AgregarDetallePedidoAInsumos(pedido.IdPedidoProveedor);
                MessageBox.Show($"Pedido {pedido.IdPedidoProveedor} marcado como entregado.");
            }       
            else
            {
                MessageBox.Show($"NO SE PUDO TU COSA.");
            }


            DateTime desde = DpDesde.SelectedDate.Value.Date;
            DateTime hasta = DpHasta.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            var pedidos = PedidoProveedorDAO.ObtenerPedidosPorRango(desde, hasta);

            var visuales = pedidos.Select(p => new PedidoProveedorVisual
            {
                IdPedidoProveedor = p.IdPedidoProveedor,
                NombreProveedor = p.Proveedor?.Nombre ?? "Desconocido",
                FechaPedido = p.FechaPedido,
                Total = p.Total,
                Status = ((StatusPedidoEnum)p.Status).ToString()
            }).ToList();

            dgPedidos.ItemsSource = visuales;
        }
    }
}
