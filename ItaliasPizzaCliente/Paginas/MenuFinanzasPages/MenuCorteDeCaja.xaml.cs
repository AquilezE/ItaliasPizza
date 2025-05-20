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

namespace ItaliasPizzaCliente.Paginas.MenuFinanzasPages
{
    /// <summary>
    /// Lógica de interacción para MenuCorteDeCaja.xaml
    /// </summary>
    public partial class MenuCorteDeCaja : Page
    {
        private int idEmpleadoActual = 1;
        private CorteDeCaja ultimoCorteCaja;
        public MenuCorteDeCaja()
        {
            InitializeComponent();
            CargarDatosUltimoCorte();
            ConfigurarValoresIniciales();
        }

        private void FinalizarCorte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarCamposComunes())
                    return;

                float cambioProximoDia = float.Parse(txtCambioProximoDia.Text);
                DateTime fechaSeleccionada = dpFechaSeleccionada.SelectedDate.Value;

                if (ultimoCorteCaja != null && ultimoCorteCaja.FechaCierre.Year == 1900)
                {
                    int resultadoCierre = CorteDeCajaDAO.RegistrarFechaCierre(ultimoCorteCaja.FechaApertura);

                    if (resultadoCierre != 0)
                    {
                        MessageBox.Show("Error al cerrar el corte anterior", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                int resultadoNuevo = CorteDeCajaDAO.RegistrarNuevoCorteDeCaja(
                    idEmpleadoActual,
                    cambioProximoDia,
                    fechaSeleccionada
                );

                if (resultadoNuevo == 0)
                {
                    MessageBox.Show("Operación completada exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                    CargarDatosUltimoCorte();
                }
                else
                {
                    MessageBox.Show("Error al registrar nuevo corte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool ValidarCamposComunes()
        {
            if (!dpFechaSeleccionada.SelectedDate.HasValue)
            {
                MessageBox.Show("Seleccione una fecha válida", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCambioProximoDia.Text))
            {
                MessageBox.Show("Ingrese el cambio para el próximo día", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!float.TryParse(txtCambioProximoDia.Text, out _))
            {
                MessageBox.Show("El cambio debe ser un valor numérico", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }




        private void RegistrarNuevoCorte()
        {
            try
            {
                if (!ValidarCamposParaNuevoCorte())
                    return;

                int resultado = CorteDeCajaDAO.RegistrarNuevoCorteDeCaja(
                    idEmpleadoActual,
                    float.Parse(txtCambioProximoDia.Text),
                    dpFechaSeleccionada.SelectedDate.Value
                );

                if (resultado == 0)
                {
                    MessageBox.Show("Nuevo corte de caja registrado exitosamente",
                                  "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                    CargarDatosUltimoCorte(); 
                }
                else
                {
                    MessageBox.Show("Error al registrar el nuevo corte de caja",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar nuevo corte: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarCamposParaNuevoCorte()
        {
            if (!dpFechaSeleccionada.SelectedDate.HasValue)
            {
                MessageBox.Show("Seleccione una fecha válida", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCambioProximoDia.Text))
            {
                MessageBox.Show("Ingrese el cambio para el próximo día", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!float.TryParse(txtCambioProximoDia.Text, out _))
            {
                MessageBox.Show("El cambio para el próximo día debe ser un valor numérico", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }



        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }


        private void CargarDatosUltimoCorte()
        {
            try
            {
                CorteDeCaja ultimoCorte = CorteDeCajaDAO.ObtenerUltimoCorteDeCaja();

                
                lblFechaApertura.Text = ultimoCorte.FechaApertura.ToString("dd/MM/yyyy HH:mm");
                lblCambio.Text = ultimoCorte.Cambio.ToString("C"); 
                lblVentasDia.Text = ultimoCorte.VentaDelDia.ToString("C");
                lblGastosDia.Text = ultimoCorte.Gasto.ToString("C");

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del último corte: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigurarValoresIniciales()
        {
            dpFechaSeleccionada.SelectedDate = DateTime.Now;
        }

        private void LimpiarCampos()
        {
            dpFechaSeleccionada.Text = string.Empty;
            txtCambioProximoDia.Text = string.Empty;
            dpFechaSeleccionada.SelectedDate = DateTime.Now;
        }
    }
}
