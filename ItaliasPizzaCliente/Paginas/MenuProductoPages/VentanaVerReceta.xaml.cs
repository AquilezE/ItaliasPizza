using ItaliasPizzaDB.DataAccessObjects;
using System;
using System.Collections.Generic;
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
using System.IO;

namespace ItaliasPizzaCliente.Paginas.MenuProductoPages
{
    /// <summary>
    /// Lógica de interacción para VentanaVerReceta.xaml
    /// </summary>
    public partial class VentanaVerReceta : Window
    {
        public VentanaVerReceta(int idProducto)
        {
            InitializeComponent();

            var producto = ProductoDAO.ObtenerProductoCompleto(idProducto);
            if (producto == null)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            if (producto.Receta == null)
            {
                MessageBox.Show("Este producto no tiene receta.");
                return;
            }

            string rutaImagenCompleta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImagenesProductos", producto.ImagenRuta);


            if (producto?.Receta != null)
            {
                var recetaVm = new RecetaVisualizacion
                {
                    NombreProducto = producto.Nombre,
                    ImagenRuta = rutaImagenCompleta,
                    Instrucciones = producto.Receta.Instrucciones ?? "No hay instrucciones registradas.",
                    Insumos = producto.Receta.InsumosParaReceta.Select(i => new RecetaVisualizacion.InsumoDetalle
                    {
                        Nombre = i.Insumo.Nombre,
                        Cantidad = i.Cantidad,
                        Unidad = i.Insumo.UnidadDeMedida.UnidadDeMedidaNombre
                    }).ToList()
                };

                this.DataContext = recetaVm;
            }
        }

        public class RecetaVisualizacion
        {
            public string NombreProducto { get; set; }
            public string ImagenRuta { get; set; }
            public string Instrucciones { get; set; }
            public List<InsumoDetalle> Insumos { get; set; }

            public class InsumoDetalle
            {
                public string Nombre { get; set; }
                public float Cantidad { get; set; }
                public string Unidad { get; set; }
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
