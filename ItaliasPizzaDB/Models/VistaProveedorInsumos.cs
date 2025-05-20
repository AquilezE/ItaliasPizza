using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItaliasPizzaDB.Models
{
    [Table("VistaInsumosPorProveedor")]
    public class VistaProveedorInsumos
    {
        [Key] 
        public int IdInsumo { get; set; }

        public int IdProveedor { get; set; }

        public string NombreProveedor { get; set; }

        public string NombreInsumo { get; set; }

        public float Precio { get; set; }

        public string Unidad { get; set; }
        public int IdCategoriaInsumo { get; set; }

        // Propiedad adicional para capturar cantidad (no viene de BD)
        [NotMapped]
        public int Cantidad { get; set; }
    }
}