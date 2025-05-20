using ItaliasPizzaCliente.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MenuGenerarReporte.xaml
    /// </summary>
    public partial class MenuGenerarReporte : Page
    {
        public MenuGenerarReporte()
        {
            InitializeComponent();
            stackPanelCheckboxes.Visibility = Visibility.Collapsed;
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? start = startDatePicker.SelectedDate;
            DateTime? end = endDatePicker.SelectedDate;

            if (start == null || end == null)
            {

                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowWarningNotification("Selecciona ambas fechas.");

                stackPanelCheckboxes.Visibility = Visibility.Collapsed;

                return;
            }

            if (start > end)
            {
                DialogoNotificacion dialogo = new DialogoNotificacion();
                dialogo.ShowErrorNotification("La fecha de inicio no puede ser mayor a la de fin.");
                stackPanelCheckboxes.Visibility = Visibility.Collapsed;

            }
            else
            {
                stackPanelCheckboxes.Visibility = Visibility.Visible;

            }
        }
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            DialogoNotificacion dialogo = new DialogoNotificacion();

            if (pedidosCheckbox.IsChecked == true || inventarioCheckbox.IsChecked == true || mermaCheckbox.IsChecked == true || pedidoProvCheckbox.IsChecked == true)
            {

                DateTime? start = startDatePicker.SelectedDate;
                DateTime? end = endDatePicker.SelectedDate;



                if (start == null || end == null)
                {
                    dialogo.ShowWarningNotification("Selecciona ambas fechas.");
                    return;
                }

                if (start > end)
                {

                    dialogo.ShowErrorNotification("La fecha de inicio no puede ser mayor a la de fin.");
                    return;
                }

                var opcionesReporte = new OpcionesReporte{
                    IncluirPedidos = pedidosCheckbox.IsChecked == true,
                    IncluirInventario = inventarioCheckbox.IsChecked == true,
                    IncluirMerma = mermaCheckbox.IsChecked == true,
                    IncluirPedidosProveedor = pedidoProvCheckbox.IsChecked == true,
                    FechaInicio = start.Value,
                    FechaFin = end.Value
                };

                string reporte = GeneradorReporte.GenerarHTMLReporte(opcionesReporte);
                string tempFileName = $"Reporte-{start.Value.ToString("yyyy-MM-dd")}-{end.Value.ToString("yyyy-MM-dd")}";
                dialogo.ShowSuccessNotification("Reporte generado con éxito.");


                var ventanaPrevisualziar = new PrevisualizadorPDF(reporte, tempFileName);
                ventanaPrevisualziar.Owner = Application.Current.MainWindow;
                ventanaPrevisualziar.Height = 800;
                ventanaPrevisualziar.ShowDialog();


            }
            else
            {
                dialogo.ShowWarningNotification("Selecciona al menos una opción para generar el reporte.");
            }
        
        }
    }
}
