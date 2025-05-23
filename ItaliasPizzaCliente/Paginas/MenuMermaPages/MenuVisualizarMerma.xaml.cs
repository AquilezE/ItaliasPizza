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
using ItaliasPizzaCliente.Paginas.MenuMermaPages;
using ItaliasPizzaDB.DataAccessObjects;

namespace ItaliasPizzaCliente.Paginas.MenuMergaPages
{
    /// <summary>
    /// Lógica de interacción para MenuVisualizarMerma.xaml
    /// </summary>
    public partial class MenuVisualizarMerma : Page
    {
        private List<MermaVisual> todasLasMermas; // Datos completos
        private int paginaActual = 1;
        private int totalPaginas;
        private int elementosPorPagina = 5;

        public class MermaVisual
        {
            public DateTime Fecha { get; set; }
            public float TotalPerdido { get; set; }
            public string FechaFormateada
            {
                get
                {
                    return Fecha.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-MX"));
                }
            }
        }




        public MenuVisualizarMerma()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (DpDesde.SelectedDate == null || DpHasta.SelectedDate == null)
            {
                MessageBox.Show("Selecciona ambas fechas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime desde = DpDesde.SelectedDate.Value.Date;
            DateTime hasta = DpHasta.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            var filtradas = MermaDAO.ObtenerResumenDeMermas()
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .Select(m => new MermaVisual
                {
                    Fecha = m.Fecha,
                    TotalPerdido = m.TotalPerdido
                })
                .ToList();

            todasLasMermas = filtradas;
            paginaActual = 1;
            CalcularTotalPaginas();
            MostrarPagina(paginaActual);
        }

        private void CargarDatos()
        {

            todasLasMermas = MermaDAO.ObtenerResumenDeMermas()
                .Select(m => new MermaVisual
                {
                    Fecha = m.Fecha,
                    TotalPerdido = m.TotalPerdido
                }).ToList();

            CalcularTotalPaginas();
            MostrarPagina(paginaActual);
        }
        private void CalcularTotalPaginas()
        {
            totalPaginas = (int)Math.Ceiling((double)todasLasMermas.Count / elementosPorPagina);
        }

        private void MostrarPagina(int numeroPagina)
        {
            if (todasLasMermas == null || todasLasMermas.Count == 0)
            {
                MermasDataGrid.ItemsSource = null;
                return;
            }

            int inicio = (numeroPagina - 1) * elementosPorPagina;
            var pagina = todasLasMermas.Skip(inicio).Take(elementosPorPagina).ToList();
            MermasDataGrid.ItemsSource = pagina;
        }


        private void BtnPagina_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Content.ToString(), out int pagina))
            {
                paginaActual = pagina;
                MostrarPagina(paginaActual);
            }
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual < totalPaginas)
            {
                paginaActual++;
                MostrarPagina(paginaActual);
            }
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
                MostrarPagina(paginaActual);
            }
        }

        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DateTime fecha)
            {
                var detallesDTO = MermaDAO.ObtenerDetallePorFecha(fecha);

                var detallesVisual = detallesDTO.Select(d => new MermaDetalleVisual
                {
                    NombreInsumo = d.NombreInsumo,
                    Unidad = d.Unidad,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList();

                var ventana = new VentanaDetalleMerma(detallesVisual, fecha);
                ventana.ShowDialog();
            }
        }

    }
}
