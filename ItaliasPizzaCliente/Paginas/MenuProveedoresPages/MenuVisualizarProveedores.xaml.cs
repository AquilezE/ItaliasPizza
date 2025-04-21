using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
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

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{
    /// <summary>
    /// Interaction logic for MenuVisualizarProveedores.xaml
    /// </summary>
    public partial class MenuVisualizarProveedores : Page
    {

        public ObservableCollection<Proveedor> Proveedores { get; set; }

        public MenuVisualizarProveedores()
        {
            InitializeComponent();
            Proveedores = new ObservableCollection<Proveedor>();
            DataContext = this;
            CargarProveedores();
        }



        private void AgregarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var dialogoNotificacion = new DialogoNotificacion();
            dialogoNotificacion.ShowWarningNotification("Agregar proveedor");
        }

        private void EditarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (dgProveedores.SelectedItem == null)
            {
                var dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowWarningNotification("No haz seleccionado nada");

            }
            else
            {

                var proveedor = (Proveedor)dgProveedores.SelectedItem;
                var dialogoNotificacion = new DialogoNotificacion();
                dialogoNotificacion.ShowInfoNotification($"Proveedor agarrado: {proveedor.Nombre}, {proveedor.IdProveedor}");
                dgProveedores.SelectedItem = null;
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Buscando");

            txtNombre.Text = txtNombre.Text.Trim();
            txtTelefono.Text = txtTelefono.Text.Trim();

            if (string.IsNullOrEmpty(txtNombre.Text) && string.IsNullOrEmpty(txtTelefono.Text))
            {
                CargarProveedores();
                Console.WriteLine("SINPARAM");

            }
            else
            {
                CargarProveedores(txtNombre.Text, txtTelefono.Text);
                Console.WriteLine("CONPARAM");
            }
        }

        private async void CargarProveedores()
        {
            var proveedoresLista = await Task.Run(ProveedorDAO.ObtenerProveedores);
            Proveedores.Clear();
            foreach (var proveedor in proveedoresLista)
            {
                Console.WriteLine(proveedor.Nombre);
                Proveedores.Add(proveedor);
            }
        }

        private async void CargarProveedores(string nombre, string telefono)
        {
            var proveedoresLista = await Task.Run(() => ProveedorDAO.ObtenerProveedores(nombre, telefono));
            Proveedores.Clear();
            foreach (var proveedor in proveedoresLista)
            {
                Console.WriteLine(proveedor.Nombre);
                Proveedores.Add(proveedor);
            }
        }
    }
}
