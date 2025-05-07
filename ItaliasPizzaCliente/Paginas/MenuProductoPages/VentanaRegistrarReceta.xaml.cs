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
            var insumos = InsumoDAO.ObtenerInsumos();
            DgInsumos.ItemsSource = insumos;
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
                DgInsumos.ItemsSource = insumos;
            }
        }

        private void TbBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
