// Importa el espacio de nombres para trabajar con diagnósticos y trazas
using System.Diagnostics;
// Importa el modelo de datos de la aplicación
using AppUsuarios.Models;
// Importa el espacio de nombres para controladores en ASP.NET Core
using Microsoft.AspNetCore.Mvc;

// Define el espacio de nombres para el controlador
namespace AppUsuarios.Controllers
{
    // Define la clase HomeController que hereda de Controller
    public class HomeController : Controller
    {
        // Define un campo de solo lectura para el logger
        private readonly ILogger<HomeController> _logger;

        // Constructor de la clase que recibe un logger como parámetro e inicializa el campo _logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Método que maneja la solicitud a la página de inicio y devuelve la vista correspondiente
        public IActionResult Index()
        {
            return View();
        }

        // Método que maneja la solicitud a la página de privacidad y devuelve la vista correspondiente
        public IActionResult Privacy()
        {
            return View();
        }

        // Método que maneja los errores y devuelve una vista con información sobre el error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Devuelve la vista de error con un modelo que contiene el ID de la solicitud actual
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}