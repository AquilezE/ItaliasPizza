using ItaliasPizzaCliente.Singletons;
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

namespace ItaliasPizzaCliente.Paginas.MenuPedidoPages
{
    /// <summary>
    /// Interaction logic for MenuVerPedidos.xaml
    /// </summary>
    public partial class MenuVerPedidos : Page
    {

        public ObservableCollection<Pedido> Pedidos { get; set; } = new ObservableCollection<Pedido>();

        public MenuVerPedidos()
        {
            InitializeComponent();
            this.DataContext = this;
            CargarPedidos(UsuarioSingleton.Instance.NombreCargo);

        }

        private void PedidoPreview_VerDetallesClicked(object sender, EventArgs e)
        {
            var preview = (PedidoPreview)sender;

            var pedido = (Pedido)preview.DataContext;
            
            switch (pedido)
            {
                case PedidoParaLocal _:
                    
                    //TODO: Aqui no se qpd alch

                default:

                    break;
            }
            
        }

        private void CargarPedidos(string instanceNombreCargo)
        {
            var list = new List<Pedido>();

            list.Add(new Pedido()
            {
                IdPedido = 1,
                Total = 100,
                Fecha = DateTime.Now,
            });


            foreach (var p in list)
                Pedidos.Add(p);

        }


    }
}
