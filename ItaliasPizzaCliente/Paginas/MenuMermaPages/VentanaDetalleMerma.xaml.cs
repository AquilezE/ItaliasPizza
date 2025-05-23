using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizzaCliente.Paginas.MenuMermaPages
{
    public partial class VentanaDetalleMerma : Window
    {
        public ObservableCollection<MermaDetalleVisual> PaginaActual { get; set; } = new ObservableCollection<MermaDetalleVisual>();
        private List<MermaDetalleVisual> todasLasFilas;
        private int paginaActual = 1;
        private int totalPaginas;
        private int elementosPorPagina = 4;

        public string FechaFormateada { get; set; }
        public string TotalFormateado => "$" + todasLasFilas.Sum(i => i.TotalPerdido).ToString("F2");
        public List<int> TotalPaginas => Enumerable.Range(1, totalPaginas).ToList();

        public VentanaDetalleMerma(List<MermaDetalleVisual> datos, DateTime fecha)
        {
            InitializeComponent();
            todasLasFilas = datos;
            FechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-MX"));
            CalcularTotalPaginas();
            MostrarPagina(paginaActual);
            DataContext = this;
        }

        private void CalcularTotalPaginas()
        {
            totalPaginas = (int)Math.Ceiling((double)todasLasFilas.Count / elementosPorPagina);
        }

        private void MostrarPagina(int numero)
        {
            PaginaActual.Clear();
            int inicio = (numero - 1) * elementosPorPagina;
            foreach (var item in todasLasFilas.Skip(inicio).Take(elementosPorPagina))
                PaginaActual.Add(item);
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
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

        private void BtnPagina_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Content.ToString(), out int pagina))
            {
                paginaActual = pagina;
                MostrarPagina(pagina);
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class MermaDetalleVisual
    {
        public string NombreInsumo { get; set; }
        public string Unidad { get; set; }
        public float Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public float TotalPerdido => Cantidad * PrecioUnitario;
    }
}
