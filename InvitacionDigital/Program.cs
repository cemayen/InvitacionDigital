using Microsoft.EntityFrameworkCore;
using InvitacionDigital.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Registrar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Autentificacion/InicioSesion"; // Ruta de tu login
        options.AccessDeniedPath = "/Inicio/Error"; // Si intentan entrar a un lugar sin permiso
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Duración de la sesión
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Inicio/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Habilitar Autenticación y Autorización
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");

// Inicializar Base de Datos y Semilla (Seeding)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Crea la base de datos si no existe
    context.Database.EnsureCreated();

    // Crear el Usuario Admin por defecto si no existe
    if (!context.Usuarios.Any(u => u.Rol == "Admin"))
    {
        var admin = new Usuario
        {
            NombreFamilia = "Administrador",
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), //
            CantidadBoletos = 2,
            OpcionCabana = true,
            Rol = "Admin"
        };

        context.Usuarios.Add(admin);
        context.SaveChanges();
    }
}

app.Run();
