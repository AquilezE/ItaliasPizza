using ItaliasPizzaCliente.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ItaliasPizzaCliente.Paginas.MenuInventarioPages
{
    /// <summary>
    /// Interaction logic for PrevisualizadorPDF.xaml
    /// </summary>
    public partial class PrevisualizadorPDF : Window
    {

        private readonly byte[] _pdfBytes;
        private readonly string _tempFileName;


        public PrevisualizadorPDF(string html, string tempFileName)
        {
            InitializeComponent();


            PreviewBrowser.NavigateToString(html);

            _pdfBytes = GeneradorReporte.ConvertHtmlToPdf(html);
            _tempFileName = tempFileName;
                        
        }


   

        private void BotonDescargar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.FileName = _tempFileName;
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllBytes(filePath, _pdfBytes);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
           
        }
    }
    
}
