using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataTransferObjects
{
    public class InsumoProveedorDTO
    {
        public int IdInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public float Precio { get; set; }
        public string Unidad { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdCategoriaInsumo { get; set; }
    }

}
