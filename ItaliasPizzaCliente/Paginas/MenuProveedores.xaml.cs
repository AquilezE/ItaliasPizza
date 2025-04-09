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
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.Models;
using ItaliasPizzaDB.Repositorios;


namespace ItaliasPizzaCliente.Paginas
{
    /// <summary>
    /// Interaction logic for MenuProveedores.xaml
    /// </summary>
    public partial class MenuProveedores : Page
    {
        public ObservableCollection<Proveedor> Proveedores { get; set; }

        public MenuProveedores()
        {
            InitializeComponent();
            Proveedores = new ObservableCollection<Proveedor>();
            DataContext = this;
            CargarProveedores();
        }



        private void AgregarProveedor_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Aqui se debe de abrir la ventana de agregar proveedor
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

                //TODO: Aqui se debe de abrir la ventana de editar proveedor, para evitar pedos namas que la pagina tenga un Constructor que reciba un proveedor como parametro
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
            var proveedoresLista = await Task.Run(ProveedorRepositorio.ObtenerProveedores);
            Proveedores.Clear();
            foreach (var proveedor in proveedoresLista)
            {
                Console.WriteLine(proveedor.Nombre);
                Proveedores.Add(proveedor);
            }
        }

        private async void CargarProveedores(string nombre, string telefono)
        {
            var proveedoresLista = await Task.Run(() => ProveedorRepositorio.ObtenerProveedores(nombre, telefono));
            Proveedores.Clear();
            foreach (var proveedor in proveedoresLista)
            {
                Console.WriteLine(proveedor.Nombre);
                Proveedores.Add(proveedor);
            }
        }
    }

}
