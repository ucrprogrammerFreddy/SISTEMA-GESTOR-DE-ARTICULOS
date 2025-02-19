using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppUsuarios.Models;

public class SeccionesController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public SeccionesController(IMemoryCache cache)
    {
        _httpClient = new Conexion().Iniciar();
        _cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            if (!_cache.TryGetValue("SeccionesCache", out List<Secciones> secciones))
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Secciones/GetSecciones");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    secciones = JsonConvert.DeserializeObject<List<Secciones>>(json);
                    _cache.Set("SeccionesCache", secciones, TimeSpan.FromMinutes(5));
                    return View(secciones);
                }
                TempData["Mensaje"] = $"Error al obtener secciones: {response.ReasonPhrase}";
                secciones = new List<Secciones>();
            }
            return View(secciones);
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
            return View(new List<Secciones>());
        }
    }

  

    [HttpGet]
    public IActionResult Create()
    {
        return View();
        
    }

    [HttpPost]
    public async Task<IActionResult> Create(Secciones seccion)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error en ModelState: {error.ErrorMessage}");
            }
            TempData["Mensaje"] = "Excepción: El objeto es inválido.";
            return View(seccion);
        }

        try
        {
            // Manejo del archivo de imagen
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    // Ruta donde se guardará la imagen dentro de wwwroot
                    string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(wwwRootPath, uniqueFileName);

                    // Guardar físicamente el archivo en wwwroot/img
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Asignar la URL accesible de la imagen
                    seccion.ImagenURL = "/img/" + uniqueFileName;
                }
            }
            else
            {
                ModelState.AddModelError("ImagenURL", "La imagen es obligatoria.");
                return View(seccion);
            }

            // Serializar el objeto para enviarlo a la API
            string json = JsonConvert.SerializeObject(seccion);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("api/Secciones/AddSeccion", content);

            if (response.IsSuccessStatusCode)
            {
                _cache.Remove("SeccionesCache");
                TempData["Mensaje"] = "Sección creada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Mensaje"] = $"Error al crear sección: {response.ReasonPhrase}";
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return View(seccion);
    }//fin del metodo addSeccion

    //los metodos de abajo no contienen prubas de funcionalidad

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Secciones/GetSeccionForId/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Secciones seccion = JsonConvert.DeserializeObject<Secciones>(json);
                return View(seccion);
            }
            TempData["Mensaje"] = "Sección no encontrada";
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return RedirectToAction("Index");
    }



    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Secciones/GetSeccionForId/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Secciones seccion = JsonConvert.DeserializeObject<Secciones>(json);
                return View(seccion);
            }
            TempData["Mensaje"] = "Sección no encontrada";
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Secciones seccion)
    {
        if (!ModelState.IsValid)
            return View(seccion);
        try
        {
            string json = JsonConvert.SerializeObject(seccion);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"api/Secciones/UpdateSeccion/{seccion.IdSeccion}", content);
            if (response.IsSuccessStatusCode)
            {
                _cache.Remove("SeccionesCache");
                TempData["Mensaje"] = "Sección actualizada correctamente.";
                return RedirectToAction("Index");
            }
            TempData["Mensaje"] = $"Error al actualizar sección: {response.ReasonPhrase}";
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return View(seccion);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Secciones/GetSeccionForId/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Secciones seccion = JsonConvert.DeserializeObject<Secciones>(json);
                return View(seccion);
            }
            TempData["Mensaje"] = "Sección no encontrada";
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Secciones/DeleteSeccion/{id}");
            if (response.IsSuccessStatusCode)
            {
                _cache.Remove("SeccionesCache");
                TempData["Mensaje"] = "Sección eliminada correctamente.";
            }
            else
            {
                TempData["Mensaje"] = $"Error al eliminar sección: {response.ReasonPhrase}";
            }
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = $"Excepción: {ex.Message}";
        }
        return RedirectToAction("Index");
    }
}
