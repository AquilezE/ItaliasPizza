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

namespace ItaliasPizzaCliente.Paginas.MenuInventarioPages
{
    /// <summary>
    /// Interaction logic for MenuVisualizarInventario.xaml
    /// </summary>
    public partial class MenuVisualizarInventario : Page
    {

        public ObservableCollection<Insumo> Insumos { get; set; }
        public ObservableCollection<CategoriaInsumo> Categorias { get; set; }
        public ObservableCollection<UnidadDeMedida> Unidades { get; set; }
        public bool NoActivoSelected { get; set; } = false;

        public MenuVisualizarInventario()
        {

            InitializeComponent();
            Insumos = new ObservableCollection<Insumo>();
            Categorias = new ObservableCollection<CategoriaInsumo>();
            Unidades = new ObservableCollection<UnidadDeMedida>();

            DataContext = this;

            Insumo insumo = new Insumo();
            insumo.IdInsumo = 1;

            CargarCategorias();
            CargarUnidades();

            
        }


        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Buscando");

            txtNombre.Text = txtNombre.Text.Trim();

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                CargarInsumos();
                Console.WriteLine("SINPARAM");

            }
            else
            {
                CargarInsumos();
                Console.WriteLine("CONPARAM");
            }
        }

        private void noActivosToggle_Checked(object sender, RoutedEventArgs e)
        {

            NoActivoSelected = !NoActivoSelected;
            Console.WriteLine(NoActivoSelected);
        }

        private async void CargarInsumos()
        {
            int idCategoriaSeleccionada = -1;
            int idUnidadSeleccionada = -1;

            if (cbCategorias.SelectedItem != null)
            {
                var categoria = (CategoriaInsumo)cbCategorias.SelectedItem;
                idCategoriaSeleccionada = categoria.IdCategoriaInsumo;
            }
            
            if (cbUnidades.SelectedItem != null)
            {
                var unidad = (UnidadDeMedida)cbUnidades.SelectedItem;
                idUnidadSeleccionada = unidad.IdUnidadDeMedida;
            }

            var insumosLista = await Task.Run(() => InsumoDAO.ObtenerInsumos(idCategoriaSeleccionada, idUnidadSeleccionada, !NoActivoSelected));

            Insumos.Clear();
            foreach (var insumo in insumosLista)
            {
                Insumos.Add(insumo);
            }

        }

        private async void CargarCategorias()
        {
            var categoriasLista = await Task.Run(CategoriaInsumoDAO.ObtenerCategoriasInsumo);
            Categorias.Clear();

            foreach (var categoria in categoriasLista)
            {
                Categorias.Add(categoria);
            }
        }

        private async void CargarUnidades()
        {
            var unidadesLista = await Task.Run(UnidadDeMedidaDAO.ObtenerUnidadesDeMedida);
            Unidades.Clear();

            foreach (var unidad in unidadesLista)
            {
                Unidades.Add(unidad);
            }
        }



    }
}
