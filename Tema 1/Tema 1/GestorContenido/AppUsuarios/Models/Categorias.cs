// Importa las bibliotecas necesarias para validaciones y relaciones de datos
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Define el espacio de nombres donde se encuentra el modelo
namespace AppUsuarios.Models
{
    // Define la clase Categorias que representa una categoría en el sistema
    public class Categorias
    {
        // Define la clave primaria de la entidad
        [Key]
        public int IdCategoria { get; set; }

        // Define el nombre de la categoría con restricciones de validación
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        // Define la descripción de la categoría con restricciones de validación
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(600, ErrorMessage = "La descripción no puede exceder los 600 caracteres.")]
        public string Descripcion { get; set; }

        // Define la URL de la imagen de la categoría
        public string UrlImagen { get; set; }

        // Define el estado de la categoría con validación de formato
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("[A-Z]", ErrorMessage = "El estado debe ser un único carácter en mayúscula.")]
        public char Estado { get; set; }

        // Relación Uno a Uno: Una Categoría pertenece a una sola Sección
        public int IdSeccion { get; set; }
        public Secciones Seccion { get; set; }

        // Relación Uno a Muchos: Una Categoría tiene muchos Artículos
        public ICollection<Articulos> Articulo { get; set; } = new List<Articulos>();
    }
}

