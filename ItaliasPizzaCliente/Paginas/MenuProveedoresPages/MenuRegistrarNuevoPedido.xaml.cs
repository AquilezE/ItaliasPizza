using ItaliasPizzaCliente.Paginas.MenuProductoPages;
using ItaliasPizzaCliente.Utils;
using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static ItaliasPizzaCliente.Paginas.MenuProveedoresPages.VentanaResumenPedido;

namespace ItaliasPizzaCliente.Paginas.MenuProveedoresPages
{

    public class InsumoProveedorViewUI : INotifyPropertyChanged
    {
        // Propiedades del DTO
        public int IdInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public float Precio { get; set; }
        public string Unidad { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdCategoriaInsumo { get; set; }

        // Propiedades de UI
        private bool _seleccionado;
        public bool Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (_seleccionado != value)
                {
                    _seleccionado = value;
                    OnPropertyChanged(nameof(Seleccionado));
                }
            }
        }

        private int _cantidad;
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged(nameof(Cantidad));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public partial class MenuRegistrarNuevoPedido : Page
    {
        public ObservableCollection<InsumoProveedorViewUI> Insumos { get; set; }
        public ObservableCollection<Proveedor> Proveedores { get; set; }
        public ObservableCollection<CategoriaInsumo> Categorias { get; set; }
        private readonly DialogoNotificacion notificador = new DialogoNotificacion();

        public MenuRegistrarNuevoPedido()
        {
            InitializeComponent();
            Insumos = new ObservableCollection<InsumoProveedorViewUI>();
            Proveedores = new ObservableCollection<Proveedor>(ProveedorDAO.ObtenerProveedores());
            Categorias = new ObservableCollection<CategoriaInsumo>(CategoriaInsumoDAO.ObtenerCategoriasInsumo());

            CbProveedores.ItemsSource = Proveedores;
            CbProveedores.DisplayMemberPath = "Nombre";
            CbProveedores.SelectedValuePath = "IdProveedor";

            CbCategorias.ItemsSource = Categorias;
            CbCategorias.DisplayMemberPath = "CategoriaInsumoNombre";

            dgInsumos.ItemsSource = Insumos;
        }

        private void CbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarInsumos();
        }

        private void FiltrarInsumos()
        {
            if (CbProveedores.SelectedItem is Proveedor proveedorSeleccionado)
            {
                var insumosProveedorDTOs = ProveedorDAO.ObtenerProveedoresInsumos(proveedorSeleccionado.IdProveedor);

                // Si hay categoría seleccionada, filtrar adicionalmente
                if (CbCategorias.SelectedItem is CategoriaInsumo categoriaSeleccionada && categoriaSeleccionada.IdCategoriaInsumo > 0)
                {
                    insumosProveedorDTOs = insumosProveedorDTOs
                        .Where(i => i.IdCategoriaInsumo == categoriaSeleccionada.IdCategoriaInsumo)
                        .ToList();
                }

                Insumos.Clear();
                foreach (var item in insumosProveedorDTOs)
                {
                    Insumos.Add(new InsumoProveedorViewUI
                    {
                        IdInsumo = item.IdInsumo,
                        NombreInsumo = item.NombreInsumo,
                        Precio = item.Precio,
                        Unidad = item.Unidad,
                        IdProveedor = item.IdProveedor,
                        NombreProveedor = item.NombreProveedor,
                        IdCategoriaInsumo = item.IdCategoriaInsumo,
                        Cantidad = 0,
                        Seleccionado = false
                    });
                }
            }
        }

        private void CbCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarInsumos();
        }

        private void BtnRegistrarPedido_Click(object sender, RoutedEventArgs e)
        {
            var proveedorSeleccionado = CbProveedores.SelectedItem as Proveedor;
            if (proveedorSeleccionado == null)
            {
                notificador.ShowWarningNotification("Debes seleccionar un proveedor.");
                return;
            }

            // Obtener insumos seleccionados
            var seleccionados = Insumos
                .Where(i => i.Seleccionado && i.Cantidad > 0)
                .Select(i => new InsumoResumenPedido
                {
                    IdInsumo = i.IdInsumo,
                    Nombre = i.NombreInsumo,
                    Cantidad = i.Cantidad,
                    Unidad = i.Unidad,
                    PrecioUnitario = i.Precio
                }).ToList();

            if (!seleccionados.Any())
            {
                notificador.ShowWarningNotification("Selecciona al menos un insumo con cantidad.");
                return;
            }

            // Mostrar ventana resumen
            var ventanaResumen = new VentanaResumenPedido(seleccionados);
            var resultado = ventanaResumen.ShowDialog();

            if (resultado == true)
            {
                // Convertir a lista de detalles de pedido
                var detalles = seleccionados
                    .Select(i => new DetallePedidoProveedor
                    {
                        IdInsumo = i.IdInsumo,
                        Cantidad = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario
                    })
                    .ToList();

                // Guardar en base de datos
                PedidoProveedorDAO.GuardarPedido(proveedorSeleccionado.IdProveedor, detalles);

                new VentanaEmergente("Pedido registrado con éxito", "", "Aceptar", "", false).ShowDialog();
                Insumos.Clear();
            }
        }

        private void BtnRestar_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = dgInsumos;
            if (dataGrid.CommitEdit(DataGridEditingUnit.Cell, true))
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (((FrameworkElement)sender).DataContext is InsumoProveedorViewUI vm && vm.Cantidad > 0)
            {
                vm.Cantidad--;
            }
        }

        private void BtnSumar_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = dgInsumos;
            if (dataGrid.CommitEdit(DataGridEditingUnit.Cell, true))
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (((FrameworkElement)sender).DataContext is InsumoProveedorViewUI vm)
            {
                vm.Cantidad++;
            }
        }
    }
}