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

        public static readonly RoutedEvent ProveedorDoubleClickedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ProveedorDoubleClicked),
                RoutingStrategy.Bubble,
                typeof(EventHandler<ProveedorDoubleClickedEventArgs>),
                typeof(MenuVisualizarProveedores)
            );

        public event EventHandler<ProveedorDoubleClickedEventArgs> ProveedorDoubleClicked
        {
            add => AddHandler(ProveedorDoubleClickedEvent, value);
            remove => RemoveHandler(ProveedorDoubleClickedEvent, value);
        }

        private void OnProveedorRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var proveedor = (Proveedor)((DataGridRow)sender).Item;

            var args = new ProveedorDoubleClickedEventArgs(ProveedorDoubleClickedEvent, proveedor);
            RaiseEvent(args);
        }

        public ObservableCollection<Proveedor> Proveedores { get; set; }

        public MenuVisualizarProveedores()
        {
            InitializeComponent();
            Proveedores = new ObservableCollection<Proveedor>();
            DataContext = this;
            CargarProveedores();
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

    public class ProveedorDoubleClickedEventArgs : RoutedEventArgs
    {
        public Proveedor SelectedProveedor { get; }
        public ProveedorDoubleClickedEventArgs(RoutedEvent routedEvent, Proveedor p)
            : base(routedEvent)
        {
            SelectedProveedor = p;
        }
    }
}
