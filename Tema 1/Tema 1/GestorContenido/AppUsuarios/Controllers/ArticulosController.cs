﻿using AppUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AppUsuarios.Controllers
{
    // Este controlador se encarga de consumir el API de Articulos.
    // Hereda de Controller, lo que permite retornar vistas.
    public class ArticulosController : Controller
    {
        // Declaramos una instancia de HttpClient para realizar peticiones al API.
        // Se utiliza una instancia única (obtenida desde la clase Conexion) para evitar problemas de recursos.
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        // Constructor del controlador.
        // Se inicializa el HttpClient mediante la clase Conexion.
        public ArticulosController(IMemoryCache cache)
        {
            // La clase Conexion gestiona la configuración y retorna una instancia de HttpClient.
            _httpClient = new Conexion().Iniciar();
            _cache = cache;
        }

        /// <summary>
        /// Acción para listar todas las etiquetas.
        /// Realiza una petición GET al endpoint 
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {

                if (!_cache.TryGetValue("ArticulosCache", out List<Articulos> articulos))
                {

                    // Se realiza una petición GET al API para obtener todas los artículos.
                    HttpResponseMessage response = await _httpClient.GetAsync("api/Articulos/GetArticulos");

                    // Se comprueba si la respuesta del API fue exitosa (código HTTP 200).
                    if (response.IsSuccessStatusCode)
                    {
                        // Se lee el contenido de la respuesta (JSON) como cadena.
                        string json = await response.Content.ReadAsStringAsync();


                        // Se seserializa el JSON en una lista de objetos Articulo.
                        articulos = JsonConvert.DeserializeObject<List<Articulos>>(json);

                        // Se pasa la lista de articulos a la vista para ser mostrada.
                        return View(articulos);
                        _cache.Set("ArticulosCache", articulos, TimeSpan.FromMinutes(5));
                    }
                    else
                    {

                        // Si el API devuelve un error, se almacena el mensaje en TempData.
                        TempData["Mensaje"] = $"Error al obtener articulos: {response.ReasonPhrase}";
                        articulos = new List<Articulos>();
                    }

                }
                //retorna los articulos obtenidos  (ya sea desde caché o API
                return View(articulos);
            }
            catch (Exception ex)
            {
                // Se captura cualquier excepción y se guarda el mensaje para mostrarlo al usuario.
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
                return View(new List<Articulos>());  // En caso de error, se retorna la vista con una lista vacía.

            }
        }



        // BUSCAR ETIQUETA POR ID
        [HttpGet]   // Modificar buscar articulo por titulo , o codigo 

        public async Task<IActionResult> BuscarArticuloPorId(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/Articulos/GetArticulosForId/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Articulo no encontrado.";
                    return RedirectToAction("Index");
                }

                // Se lee el contenido de la respuesta (JSON) como cadena.
                string json = await response.Content.ReadAsStringAsync();

                // Se deserializa el JSON en una lista de objetos Etiqueta.
                List<Articulos> articulos = JsonConvert.DeserializeObject<List<Articulos>>(json);

                return View(articulos);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al buscar articulos: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        //Pendiente en api si da, en consumo no.


        /// <summary>
        /// Acción para mostrar los detalles de articulo.
        /// Realiza una petición GET al endpoint 
        /// </summary>
        /// <param name="id">Identificador
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Se construye la URL del endpoint con el identificador proporcionado.
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Articulos/GetArticulosForId/{id}");

                // Si la respuesta es exitosa, se procede a deserializar el JSON.
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Articulos articulos = JsonConvert.DeserializeObject<Articulos>(json);

                    // Se retorna la vista Details mostrando la información del articulo.
                    return View(articulos);
                }
                else
                {
                    // Si el articulo no se encuentra, se almacena un mensaje de error.
                    TempData["Mensaje"] = "Etiqueta no encontrada";
                }
            }
            catch (Exception ex)
            {
                // Capturamos y mostramos cualquier excepción ocurrida.
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // En caso de error, redirige a la acción Index para listar articulos.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Acción GET para mostrar el formulario de creación de un articulos.
        /// Simplemente retorna la vista donde el usuario() puede ingresar datos.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Acción POST para crear un nuevo artículos.
        /// Envía una petición POST al endpoint 
        /// </summary>
        /// <param name="etiqueta">Objeto Articulos con la información a crear</param>
        [HttpPost]
        public async Task<IActionResult> Create(Articulos articulos)
        {
            // Se verifica que los datos enviados cumplan las validaciones del modelo.
            if (!ModelState.IsValid)
                return View(articulos);

            try
            {
                // Se convierte el objeto etiqueta a formato JSON.
                string json = JsonConvert.SerializeObject(articulos);

                // Se crea el contenido de la petición con el JSON, indicando el tipo de contenido "application/json".
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Se envía la petición POST al API para agregar el nuevo articulos.
                HttpResponseMessage response = await _httpClient.PostAsync("api/Articulos/AddArticulo", content);

                if (response.IsSuccessStatusCode)
                {

                    _cache.Remove("ArticulosCache");// borramos la cache con informacion obsoleta
                    // Si la creación es exitosa, se guarda un mensaje y se redirige a la lista de articulos.
                    TempData["Mensaje"] = "Articulo creado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Si ocurre un error, se guarda el mensaje devuelto por el API.
                    TempData["Mensaje"] = $"Error al crear Articulo: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                // Captura excepciones y guarda el mensaje para el usuario.
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // En caso de error, se retorna la misma vista para corregir los datos ingresados.
            return View(articulos);
        }

        /// <summary>
        /// Acción GET para mostrar el formulario de edición de articulo.
        /// Realiza una petición GET al endpoint "
        /// </summary>
        /// <param name="id">Identificador del artículo a editar</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // Se realiza una petición GET para obtener el articulo a editar.
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Etiquetas/GetEtiquetaForId/{id}");
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    Articulos articulos = JsonConvert.DeserializeObject<Articulos>(json);

                    // Se retorna la vista Edit con los datos de la etiqueta.
                    return View(articulos);
                }
                else
                {
                    TempData["Mensaje"] = "Articulo no encontrado";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // Si ocurre un error, se redirige a la lista de articulos.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Acción POST para actualizar un articulo.
        /// Envía una petición PUT al endpoint "api/Etiquetas/UpdateEtiqueta/{idEtiqueta}" con los datos modificados.
        /// </summary>
        /// <param name="etiqueta">Objeto Articulo con la información actualizada</param>
        [HttpPost]
        public async Task<IActionResult> Edit(Articulos articulos)
        {
            // Se valida que el modelo cumpla con los requisitos definidos.
            if (!ModelState.IsValid)
                return View(articulos);

            try
            {
                // Se serializa el objeto actualizado a formato JSON.
                string json = JsonConvert.SerializeObject(articulos);
                if (articulos.IdArticulo == 0)
                {
                    TempData["Mensaje"] = "Error: El ID de la etiqueta es inválido no puede ser 0.";
                    return View(articulos);
                }

                // Se prepara el contenido de la petición PUT.
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Se envía la petición PUT al API para actualizar articulos.
                HttpResponseMessage response = await _httpClient.PutAsync($"api/Articulos/UpdateArticulos/{articulos.IdArticulo}", content);
                if (response.IsSuccessStatusCode)
                {
                    // Si la actualización fue exitosa, se guarda un mensaje y se redirige a Index.
                    TempData["Mensaje"] = "Articulo actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Mensaje"] = $"Error al actualizar Articulo: {response.ReasonPhrase}";
                }

            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // Si se produce un error, se retorna la vista Edit con el objeto para corregirlo.
            return View(articulos);
        }

        /// <summary>
        /// Acción GET para mostrar la confirmación de eliminación de un Articulo.
        /// Realiza una petición GET al endpoint "api/Etiquetas/GetEtiquetaForId/{id}" para mostrar los datos.
        /// </summary>
        /// <param name="id">Identificador  
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Se solicita la etiqueta a eliminar mediante el API.
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Etiquetas/GetArticulosForId/{id}");
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    Etiquetas etiqueta = JsonConvert.DeserializeObject<Etiquetas>(json);

                    // Se retorna la vista Delete para confirmar la eliminación.
                    return View(etiqueta);

                }
                else
                {
                    TempData["Mensaje"] = "Articulo no encontrado";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // Si ocurre un error, se redirige a la lista de articulos.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Acción POST para confirmar y ejecutar la eliminación de un articulo.
        /// Envía una petición DELETE al endpoint 
        /// </summary>
        /// <param name="id">Identificador de la etiqueta a eliminar</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Valida el token anti-CSRF para mayor seguridad
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Se envía la petición DELETE para eliminar el articulo especificada.
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Etiquetas/DeleteArticulos/{id}");
                if (response.IsSuccessStatusCode)
                {

                    _cache.Remove("ArticulosCache");
                    // Si la eliminación fue exitosa, se guarda un mensaje de confirmación.
                    TempData["Mensaje"] = "Articulo eliminado correctamente.";
                }
                else
                {
                    TempData["Mensaje"] = $"Error al eliminar articulo: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Excepción: {ex.Message}";
            }

            // Se redirige a la acción Index, que muestra la lista actualizada de etiquetas.
            return RedirectToAction("Index");
        }


    }
}