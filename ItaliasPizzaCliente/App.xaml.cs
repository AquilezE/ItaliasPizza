using ItaliasPizzaCliente.Singletons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliasPizzaCliente
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        UsuarioSingleton usuarioSingleton = UsuarioSingleton.Instance;
        
        public App()
        {
           
        }
    }
}
