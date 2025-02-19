// Importa el espacio de nombres necesario para las validaciones de datos
using System.ComponentModel.DataAnnotations;

// Este DTO contiene solo Email y Clave, eliminando datos que no son necesarios en el proceso de autenticación.
namespace AppUsuarios.DTOs
{
    // Define la clase UsuarioDTO para la transferencia de datos de usuario
    public class UsuarioDTO
    {
        // Define el campo Email con validaciones
        [Required(ErrorMessage = "El email es obligatorio.")] // Campo requerido
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")] // Validación de formato de email
        [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres.")] // Longitud máxima permitida
        public string Email { get; set; }

        // Define el campo Clave con validaciones
        [Required(ErrorMessage = "La clave es obligatoria.")] // Campo requerido
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La clave debe tener entre 6 y 50 caracteres.")] // Longitud mínima y máxima
        [DataType(DataType.Password)] // Indica que este campo representa una contraseña
        public string Clave { get; set; }
    }
}
