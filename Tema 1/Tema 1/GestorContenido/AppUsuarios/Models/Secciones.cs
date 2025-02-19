using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppUsuarios.Models
{
    public class Secciones
    {
        [Key]
        public int IdSeccion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")] // 🔹 Corrección del mensaje
        [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
        [DataType(DataType.Text)]
        public string Descripcion { get; set; }

        public string? ImagenURL { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "El estado debe ser una única letra en mayúscula.")]
        [StringLength(1)] 
        [DataType(DataType.Text)]
        public string Estado { get; set; } 

      
    }
}
