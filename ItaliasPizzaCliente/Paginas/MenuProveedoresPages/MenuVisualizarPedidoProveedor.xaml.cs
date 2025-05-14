using ItaliasPizzaDB;
using ItaliasPizzaDB.DataAccessObjects;
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

            // 1. Obtener fechas seleccionadas
            DateTime desde = DpDesde.SelectedDate.Value.Date;
            DateTime hasta = DpHasta.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            // 2. Llamar al DAO para traer los pedidos en ese rango
            var pedidos = PedidoProveedorDAO.ObtenerPedidosPorRango(desde, hasta);

            // 3. Convertir cada pedido a un objeto visual para mostrar en el DataGrid
            var visuales = pedidos.Select(p => new PedidoProveedorVisual
            {
                IdPedidoProveedor = p.IdPedidoProveedor,
                NombreProveedor = p.Proveedor?.Nombre ?? "Desconocido",
                FechaPedido = p.FechaPedido,
                Total = p.Total,
                Status = ((StatusPedidoEnum)p.Status).ToString()
            }).ToList();

            // 4. Mostrar en el DataGrid
            dgPedidos.ItemsSource = visuales;
        }

    }
}
