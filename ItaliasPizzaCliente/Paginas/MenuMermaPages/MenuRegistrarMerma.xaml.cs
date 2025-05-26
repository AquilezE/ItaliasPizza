using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ItaliasPizzaCliente.Paginas.MenuMermaPages
{
    public partial class MenuRegistrarMerma : Page, INotifyPropertyChanged
    {
        public MenuRegistrarMerma()
        {
            InitializeComponent();
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            lstInsumos.SelectionChanged += LstInsumos_SelectionChanged;
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = txtBuscar.Text.Trim();
            var resultados = InsumoDAO.BuscarInsumosPorNombre(texto);
            lstInsumos.ItemsSource = resultados;
        }

        private void LstInsumos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstInsumos.SelectedItem is ItaliasPizzaDB.Models.Insumo insumoSeleccionado)
            {
                lblNombreInsumo.Text = insumoSeleccionado.Nombre;

                if (insumoSeleccionado.UnidadDeMedida != null)
                    lblUnidad.Text = insumoSeleccionado.UnidadDeMedida.UnidadDeMedidaNombre;
                else
                    lblUnidad.Text = "(Sin unidad)";
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            var insumoSeleccionado = lstInsumos.SelectedItem as ItaliasPizzaDB.Models.Insumo;
            if (insumoSeleccionado == null)
            {
                new DialogoNotificacion().ShowWarningNotification("Por favor seleccione un insumo.");
                return;
            }

            if (!ValidarCantidad(out float cantidad))
                return;

            bool restado = InsumoDAO.RestarInventario(insumoSeleccionado.IdInsumo, cantidad);
            if (!restado)
            {
                new DialogoNotificacion().ShowWarningNotification("No hay inventario suficiente.");
                return;
            }

            bool registrada = MermaDAO.RegistrarMerma(insumoSeleccionado.IdInsumo, cantidad);
            if (registrada)
            {
                new DialogoNotificacion().ShowSuccessNotification("Merma registrada correctamente.");
            }
            else
            {
                new DialogoNotificacion().ShowWarningNotification("Ocurrió un error al registrar la merma.");
            }
        }

        private bool ValidarCantidad(out float cantidad)
        {
            cantidad = 0;
            string texto = txtCantidad.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                new DialogoNotificacion().ShowWarningNotification("La cantidad no puede estar vacía.");
                return false;
            }

            if (!float.TryParse(texto, out cantidad))
            {
                new DialogoNotificacion().ShowWarningNotification("La cantidad debe ser un número válido.");
                return false;
            }

            if (cantidad <= 0)
            {
                new DialogoNotificacion().ShowWarningNotification("La cantidad debe ser mayor a cero.");
                return false;
            }

            return true;
        }

        private void TxtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoActual = txtCantidad.Text.Insert(txtCantidad.SelectionStart, e.Text);

            if (!System.Text.RegularExpressions.Regex.IsMatch(textoActual, @"^\d{0,3}(\.\d{0,2})?$"))
            {
                e.Handled = true;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
