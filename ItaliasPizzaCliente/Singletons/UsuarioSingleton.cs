using ItaliasPizzaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaCliente.Singletons
{
    public sealed class UsuarioSingleton
    {
        private static readonly Lazy<UsuarioSingleton> lazy = new Lazy<UsuarioSingleton>(() => new UsuarioSingleton());

        public static UsuarioSingleton Instance => lazy.Value;

        private UsuarioSingleton()
        {
        }

        public string NombreUsuario { get; set; }
        public string NombreCargo { get; set; }
        public int IdEmpleado { get; set; }

        public static void Reset()
        {
            Instance.NombreUsuario = null;
            Instance.NombreCargo = null;
            Instance.IdEmpleado = 0;
        }

        public static void SetUsuario(Empleado empleado)
        {
            Instance.NombreUsuario = empleado.Nombre;
            Instance.NombreCargo = empleado.Cargo.NombreCargo;
            Instance.IdEmpleado = empleado.IdEmpleado;
        }
    }
}
