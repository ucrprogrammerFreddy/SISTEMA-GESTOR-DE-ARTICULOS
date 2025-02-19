// Define el espacio de nombres donde se encuentra el modelo
namespace AppUsuarios.Models
{
    // Define la clase Conexion que maneja la configuración de HttpClient
    public class Conexion
    {
        // Método que inicializa y configura una instancia de HttpClient
        public HttpClient Iniciar()
        {
            // Crea una nueva instancia de HttpClient
            var client = new HttpClient();

            // Establece la dirección base del cliente HTTP
            client.BaseAddress = new Uri("https://localhost:5001");

            // Retorna el cliente configurado
            return client;
        }
    }
}

