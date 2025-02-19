// Importa los espacios de nombres necesarios para validaciones y mapeo de base de datos
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using AppUsuarios.Models;

// Define el espacio de nombres donde se encuentra el modelo
namespace AppUsuarios.Models
{
    // Define la clase ArticuloAutor que representa la relación entre artículos y autores
    public class ArticuloAutor
    {
        // Define la clave primaria de la entidad
        [Key]
        public int IdArticuloAutor { get; set; }

        // Define la clave foránea para el artículo
        public int IdArticulo { get; set; }
        // Representa la relación con la entidad Articulos
        public Articulos Articulo { get; set; }

        // Define la clave foránea para el usuario
        public int IdUsuario { get; set; }
        // Representa la relación con la entidad Usuario
        public Usuario Usuario { get; set; }
    }
}