using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaDB.DataTransferObjects
{
    public class MermaDetalleDTO
    {
        public string NombreInsumo { get; set; }
        public float Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public string Unidad { get; set; } // <- Agrega esta línea

        public float TotalPerdido => Cantidad * PrecioUnitario;
    }


}
