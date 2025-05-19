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
using System.Windows.Shapes;
using ItaliasPizzaDB.DataAccessObjects;
using System.ComponentModel;
using ItaliasPizzaCliente.Utils;

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaRegistrarReceta.xaml
    /// </summary>
    
    public partial class VentanaRegistrarReceta : Window
    {
        private readonly int? idProductoEnModificacion;

        public ObservableCollection<CategoriaInsumo> Categorias { get; set; }
        private List<InsumoSeleccionadoViewModel> insumosViewModelMaestro;
        private List<InsumoSeleccionadoViewModel> insumosViewModel;
        public Receta RecetaCreada { get; private set; }
        public VentanaRegistrarReceta()
        {
            InitializeComponent();
            Categorias = new ObservableCollection<CategoriaInsumo>();

            DataContext = this;
            CargarCategorias();
        }

        public VentanaRegistrarReceta(int idProducto)
        {
            InitializeComponent();
            Categorias = new ObservableCollection<CategoriaInsumo>();
            idProductoEnModificacion = idProducto;
            DataContext = this;

            CargarCategorias();
            CargarRecetaDeProducto(idProducto); 
            Title = "Modificar Receta";
            BtnClickRegistrar.Content = "Guardar Cambios"; 
        }

        private void CargarRecetaDeProducto(int idProducto)
        {
            var receta = RecetaDAO.ObtenerRecetaPorIdProducto(idProducto);
            if (receta == null) return;

            TbInstrucciones.Text = receta.Instrucciones;

            // Marcar los insumos seleccionados y su cantidad
            foreach (var insumoVM in insumosViewModelMaestro)
            {
                var insumoEnReceta = receta.InsumosParaReceta
                    .FirstOrDefault(i => i.IdInsumo == insumoVM.IdInsumo);
                if (insumoEnReceta != null)
                {
                    insumoVM.Seleccionado = true;
                    insumoVM.Cantidad = insumoEnReceta.Cantidad;
                }
            }

            ActualizarVistaFiltrada(insumosViewModelMaestro);
        }



        public void CargarTodosLosInsumos()
        {
            var insumos = InsumoDAO.ObtenerInsumos(-1, -1, true); // Debe incluir UnidadDeMedida
            if (insumosViewModelMaestro == null)
            {
                insumosViewModelMaestro = insumos.Select(i => new InsumoSeleccionadoViewModel
                {
                    IdInsumo = i.IdInsumo,
                    Nombre = i.Nombre,
                    Precio = i.Precio,
                    IdCategoriaInsumo = i.IdCategoriaInsumo,
                    Unidad = i.UnidadDeMedida?.UnidadDeMedidaNombre ?? "",
                    Cantidad = 1,
                    Seleccionado = false
                }).ToList();
            }

            ActualizarVistaFiltrada(insumosViewModelMaestro);
        }

        private void ActualizarVistaFiltrada(List<InsumoSeleccionadoViewModel> insumosFiltrados)
        {
            insumosViewModel = insumosFiltrados
                .Select(i => insumosViewModelMaestro.First(m => m.IdInsumo == i.IdInsumo))
                .ToList();

            DgInsumos.ItemsSource = insumosViewModel;
        }


        public void CargarCategorias()
        {
            var categoriasLista = CategoriaInsumoDAO.ObtenerCategoriasInsumo();

            // Insertar la opción de "Todas las categorías"
            categoriasLista.Insert(0, new CategoriaInsumo
            {
                IdCategoriaInsumo = -1,
                CategoriaInsumoNombre = "Todas las categorías"
            });

            Categorias.Clear();
            foreach (var categoria in categoriasLista)
            {
                Categorias.Add(categoria);
            }

            // Selecciona automáticamente la opción "Todas las categorías"
            CbCategoria.SelectedIndex = 0;

            // Carga todos los insumos al iniciar
            CargarTodosLosInsumos();
        }


        private void CbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var categoriaSeleccionada = CbCategoria.SelectedItem as CategoriaInsumo;
            if (categoriaSeleccionada == null) return;

            if (categoriaSeleccionada.IdCategoriaInsumo == -1)
            {
                ActualizarVistaFiltrada(insumosViewModelMaestro);
            }
            else
            {
                var filtrados = insumosViewModelMaestro
                    .Where(i => i.IdCategoriaInsumo == categoriaSeleccionada.IdCategoriaInsumo)
                    .ToList();

                ActualizarVistaFiltrada(filtrados);
            }

        }

        private void TbBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {

            string texto = TbBuscarNombre.Text.Trim().ToLower();
            var filtrados = insumosViewModelMaestro
                .Where(i => i.Nombre.ToLower().Contains(texto))
                .ToList();

            ActualizarVistaFiltrada(filtrados);
        }

        public class InsumoSeleccionadoViewModel : INotifyPropertyChanged
        {
            public bool Seleccionado { get; set; }
            public int IdInsumo { get; set; }
            public int IdCategoriaInsumo {get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public string Unidad { get; set; }
            private float _cantidad;
            public float Cantidad
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

        private void BtnSumar_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = DgInsumos;
            if (dataGrid.CommitEdit(DataGridEditingUnit.Cell, true))
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            var vm = ((FrameworkElement)sender).DataContext as InsumoSeleccionadoViewModel;
            if (vm != null)
            {
                vm.Cantidad++;
            }
        }

        private void BtnRestar_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = DgInsumos;
            if (dataGrid.CommitEdit(DataGridEditingUnit.Cell, true))
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            var vm = ((FrameworkElement)sender).DataContext as InsumoSeleccionadoViewModel;
            if (vm != null && vm.Cantidad > 1)
            {
                vm.Cantidad--;
            }
        }

        private void BtnClicRegistrar(object sender, RoutedEventArgs e)
        {
            var insumosSeleccionados = new List<InsumoParaReceta>();

            foreach (var insumoVM in insumosViewModelMaestro)
            {
                if (insumoVM.Seleccionado)
                {
                    insumosSeleccionados.Add(new InsumoParaReceta
                    {
                        IdInsumo = insumoVM.IdInsumo, 
                        Cantidad = insumoVM.Cantidad
                    });
                }
            }

            if (insumosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debes seleccionar al menos un insumo para la receta.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (idProductoEnModificacion.HasValue)
            {
                
                Receta recetaExistente = new Receta
                {
                    Instrucciones = TbInstrucciones.Text.Trim(),
                    InsumosParaReceta = insumosSeleccionados
                };

                RecetaDAO.ActualizarRecetaDeProducto(idProductoEnModificacion.Value, recetaExistente);
                MessageBox.Show("Receta actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
               
                RecetaCreada = new Receta
                {
                    Instrucciones = TbInstrucciones.Text.Trim(),
                    InsumosParaReceta = insumosSeleccionados
                };

                DialogResult = true;
            }
            Close();
        }

        private void BtnClic_Cancelar(object sender, RoutedEventArgs e)
        {
            var ventanaConfirmacion = new VentanaEmergente(
                "¿Desea cancelar el registro de receta?",
                "Se perderán los datos registrados",
                "Aceptar",
                "Cancelar"
            );

            ventanaConfirmacion.Owner = this;
            var resultado = ventanaConfirmacion.ShowDialog();

            if (resultado == true && ventanaConfirmacion.Resultado)
            {
                this.DialogResult = false; // Indica que se canceló
                this.Close();
            }
        }
    }
}
