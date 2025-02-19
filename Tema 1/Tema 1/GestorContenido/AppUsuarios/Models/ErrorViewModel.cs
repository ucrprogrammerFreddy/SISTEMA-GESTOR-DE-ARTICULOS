// Define el espacio de nombres donde se encuentra el modelo
namespace AppUsuarios.Models
{
    // Define la clase ErrorViewModel para manejar información sobre errores
    public class ErrorViewModel
    {
        // Propiedad que almacena el identificador de la solicitud (puede ser nulo)
        public string? RequestId { get; set; }

        // Propiedad calculada que indica si se debe mostrar el RequestId
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
