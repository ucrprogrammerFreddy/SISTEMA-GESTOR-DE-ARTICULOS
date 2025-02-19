using AppUsuarios.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
// Importa el paquete para trabajar con Entity Framework Core, usado para interactuar con la base de datos.
using Microsoft.EntityFrameworkCore;

// Crea el constructor de la aplicaci�n y configura los servicios.
var builder = WebApplication.CreateBuilder(args);

//
// Configuraci�n de servicios para la aplicaci�n:
//


// ==========================================================================
// Configuraci�n de la autenticaci�n con cookies
// ==========================================================================

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(options =>
    {
        // Define el nombre de la cookie que se utilizar� para la autenticaci�n
        options.Cookie.Name = "CookieAuthentication";
        // La cookie solo se env�a mediante HTTP (previniendo accesos por scripts maliciosos)
        options.Cookie.HttpOnly = true;
        // Tiempo de expiraci�n de la cookie (59 minutos en este caso)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(59);
        // Ruta a la que se redirige el usuario si no est� autenticado
        options.LoginPath = "/Usuarios/Login";
        // Ruta a la que se redirige si el acceso es denegado
        options.AccessDeniedPath = "/Usuarios/AccessDenied";
        // Permite que la cookie se renueve si el usuario contin�a activo (expiraci�n deslizante)
        options.SlidingExpiration = true;
    });

// ==========================================================================
// Configuraci�n de autorizaci�n y pol�ticas de roles
// ==========================================================================
builder.Services.AddAuthorization(options =>
{
    // Pol�tica que requiere que el usuario tenga el rol "Admin"
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    // Pol�tica que requiere que el usuario tenga el rol "Member"
    options.AddPolicy("MemberPolicy", policy => policy.RequireRole("Member"));

});

// ==========================================================================
// Configuraci�n del manejo de sesiones
// ==========================================================================
builder.Services.AddSession(options =>
{
    // Tiempo m�ximo de inactividad antes de que la sesi�n expire (5 minutos)
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    // La cookie de sesi�n solo se transmite por HTTP
    options.Cookie.HttpOnly = true;
    // Marca la cookie como esencial para el funcionamiento de la aplicaci�n
    options.Cookie.IsEssential = true;
});


// ==========================================================================
// Registro de controladores con vistas (MVC)
// ==========================================================================
// Habilita el uso de controladores y vistas en la aplicaci�n.
// Esto registra los controladores y el sistema de vistas en el contenedor de servicios.
builder.Services.AddControllersWithViews();


// Optimizaci�n 
/*builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = false;
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = []
     { 
        "image/jpeg",
      "application/font-woff2",
      "image/gif  
     } 
 )
}

*/


// ==========================================================================
// Configuraci�n del DbContext para Entity Framework Core
// ==========================================================================
// Agrega el servicio `DbContext` para trabajar con la base de datos.
// Aqu� se define c�mo la aplicaci�n se conectar� a la base de datos usando SQL Server.
// `AppUsuarios.Models.DbContextGestionContenido` es la clase que define el modelo de la base de datos.
// La cadena de conexi�n est� almacenada en `appsettings.json` y se accede mediante el nombre "StringConexion".
builder.Services.AddDbContext<AppUsuarios.Models.DbContextGestionContenido>(
      options => options.UseSqlServer( // Especifica que se usar� SQL Server como proveedor de base de datos.
        builder.Configuration.GetConnectionString("StringConexion") // Obtiene la cadena de conexi�n desde la configuraci�n.
        )
);

// ==========================================================================
// Construcci�n de la aplicaci�n con la configuraci�n registrada
// ==========================================================================
// Construye la aplicaci�n, aplicando todas las configuraciones definidas anteriormente.
var app = builder.Build();


// ==========================================================================
// Configuraci�n del pipeline de middleware
// ==========================================================================



// Verifica si el entorno NO es de desarrollo.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Configura un controlador de errores para redirigir a una p�gina de error personalizada.
    app.UseExceptionHandler("/Home/Error");

    // Habilita HTTP Strict Transport Security (HSTS) para aumentar la seguridad al trabajar con HTTPS.
    // Esto obliga a los navegadores a usar HTTPS en todas las conexiones con el sitio.
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Habilita la redirecci�n autom�tica de HTTP a HTTPS.
// Esto asegura que todas las solicitudes se realicen de forma segura.
app.UseHttpsRedirection();

// Habilita el uso de archivos est�ticos en la aplicaci�n.
// Esto incluye archivos como CSS, im�genes, y JavaScript que est�n en la carpeta `wwwroot`.
app.UseStaticFiles();

// Configura el sistema de enrutamiento de solicitudes.
app.UseRouting();

// Habilita el middleware de autorizaci�n.
// Esto asegura que las partes de la aplicaci�n protegidas por pol�ticas de autorizaci�n requieran que los usuarios tengan los permisos adecuados.
app.UseAuthorization();

// Configura el enrutamiento predeterminado de las solicitudes.
// Define la estructura de las URLs esperadas por la aplicaci�n.
// En este caso, la estructura es:
// - `controller`: Controlador que procesar� la solicitud (por defecto "Home").
// - `action`: M�todo dentro del controlador (por defecto "Index").
// - `id`: Par�metro opcional para pasar informaci�n adicional.
app.MapControllerRoute(
    name: "default", // Nombre de la ruta.
    pattern: "{controller=Home}/{action=Index}/{id?}");// Ruta predeterminada con par�metros.

// Inicia la aplicaci�n y comienza a escuchar las solicitudes entrantes.
app.Run();
