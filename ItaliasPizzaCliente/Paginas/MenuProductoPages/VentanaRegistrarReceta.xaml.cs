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

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaRegistrarReceta.xaml
    /// </summary>
    public partial class VentanaRegistrarReceta : Window
    {
        public ObservableCollection<CategoriaInsumo> Categorias { get; set; }
        private List<InsumoSeleccionadoViewModel> insumosViewModel;
        public Receta RecetaCreada { get; private set; }
        public VentanaRegistrarReceta()
        {
            InitializeComponent();
            Categorias = new ObservableCollection<CategoriaInsumo>();

            DataContext = this;
            CargarCategorias();
        }

        public void CargarTodosLosInsumos()
        {
            var insumos = InsumoDAO.ObtenerInsumos(-1, -1, true); // Debe incluir UnidadDeMedida
            insumosViewModel = insumos.Select(i => new InsumoSeleccionadoViewModel
            {
                IdInsumo = i.IdInsumo,
                Nombre = i.Nombre,
                Precio = 0, // Reemplaza con i.Precio si tienes
                Unidad = i.UnidadDeMedida?.UnidadDeMedidaNombre ?? "",
                Cantidad = 1,
                Seleccionado = false
            }).ToList();

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
                CargarTodosLosInsumos();
            }
            else
            {
                var insumos = InsumoDAO.ObtenerInsumosPorCategoria(categoriaSeleccionada.IdCategoriaInsumo);
                insumosViewModel = insumos.Select(i => new InsumoSeleccionadoViewModel
                {
                    IdInsumo = i.IdInsumo,
                    Nombre = i.Nombre,
                    Precio = 0, // Ajusta si tienes el campo
                    Unidad = i.UnidadDeMedida?.UnidadDeMedidaNombre ??"",
                    Cantidad = 1,
                    Seleccionado = false
                }).ToList();

                DgInsumos.ItemsSource = insumosViewModel;
            }

        }

        private void TbBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {

            string texto = TbBuscarNombre.Text.Trim().ToLower();
            var resultado = insumosViewModel
                .Where(i => i.Nombre.ToLower().Contains(texto))
                .ToList();

            DgInsumos.ItemsSource = resultado;
        }

        public class InsumoSeleccionadoViewModel
        {
            public bool Seleccionado { get; set; }
            public int IdInsumo { get; set; }
            public string Nombre { get; set; }
            public float Precio { get; set; }
            public string Unidad { get; set; }
            public float Cantidad { get; set; } = 1;
        }

        private void BtnSumar_Click(object sender, RoutedEventArgs e)
        {
            var vm = ((FrameworkElement)sender).DataContext as InsumoSeleccionadoViewModel;
            if (vm != null)
            {
                vm.Cantidad++;
                DgInsumos.Items.Refresh();
            }
        }

        private void BtnRestar_Click(object sender, RoutedEventArgs e)
        {
            var vm = ((FrameworkElement)sender).DataContext as InsumoSeleccionadoViewModel;
            if (vm != null && vm.Cantidad > 1)
            {
                vm.Cantidad--;
                DgInsumos.Items.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var insumosSeleccionados = new List<InsumoParaReceta>();

            foreach (var insumoVM in insumosViewModel)
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

            RecetaCreada = new Receta
            {
                Instrucciones = "Instrucciones de ejemplo...", // Puedes capturar esto de un TextBox si tienes uno
                InsumosParaReceta = insumosSeleccionados
            };

            DialogResult = true;
            Close(); // Cierra la ventana y devuelve la receta a quien la llamó
        }
    }
}
