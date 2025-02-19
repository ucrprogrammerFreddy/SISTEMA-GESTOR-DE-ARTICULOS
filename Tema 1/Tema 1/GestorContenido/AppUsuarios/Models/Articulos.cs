// Importa las bibliotecas necesarias para la manipulación de datos y validaciones
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Define el espacio de nombres donde se encuentra el modelo
namespace AppUsuarios.Models
{
    // Define la clase Articulos que representa un artículo en el sistema
    public class Articulos
    {
        // Define la clave primaria de la entidad
        [Key]
        public int IdArticulo { get; set; }

        // Define el nombre del artículo con restricciones de validación
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        // Define la descripción del artículo con restricciones de validación
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        // Campo opcional para la URL de la imagen del artículo
        public string? ImagenURL { get; set; }

        // Define el estado del artículo con validación de formato
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("[A-Z]", ErrorMessage = "El estado debe ser un único carácter en mayúscula.")]
        public char Estado { get; set; }

        // Relación con Categoría (Un Artículo pertenece a una Categoría)
        public int IdCategoria { get; set; }
        public Categorias Categoria { get; set; }

        // Relación Muchos a Muchos con Usuarios (Autores)
        public ICollection<ArticuloAutor> ArticuloAutor { get; set; } = new List<ArticuloAutor>();

        // Relación con Etiquetas (Un artículo puede tener múltiples etiquetas)
        public ICollection<Etiquetas> Etiqueta { get; set; }
    }
}