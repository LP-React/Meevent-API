using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Eventos.Services;
using gRcp_Paises;
using Meevent_API.src.Features.CategoriasEvento.DAO;
using Meevent_API.src.Features.CategoriasEvento.Services;
using Meevent_API.src.Features.Paises.Services;
using Meevent_API.src.Features.Paises.Services.Interfaces;
using Meevent_API.src.Features.PerfilesArtistas.DAO;
using Meevent_API.src.Features.PerfilesOrganizador.DAO;
using Meevent_API.src.Features.PerfilesOrganizadores.DAO;
using Meevent_API.src.Features.PerfilesOrganizadores.Services;
using Meevent_API.src.Features.SubcategoriasEvento.DAO;
using Meevent_API.src.Features.SubcategoriasEvento.Services;
using Meevent_API.src.Features.Usuarios.DAO;
using Meevent_API.src.Features.Usuarios.Service;
using Meevent_API.src.Features.Locales.DAO;
using Meevent_API.src.Features.Locales.Service;
using Meevent_API.src.Features.Ciudades.DAO;
using Meevent_API.src.Features.Ciudades.Service;
using Meevent_API.src.Features.SeguidoresEvento.DAO;
using Meevent_API.src.Features.SeguidoresEvento.Service;

var builder = WebApplication.CreateBuilder(args);

//  CORS básico para Next.js local
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddGrpcClient<ServicioPaises.ServicioPaisesClient>(o =>
{
    o.Address = new Uri("https://localhost:7111");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ===== INYECCIÓN DE DEPENDENCIAS =====
// ---- Registrar DAOs ----

builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();
builder.Services.AddScoped<IPerfilOrganizadorDAO, PerfilOrganizadorDAO>();
builder.Services.AddScoped<IPerfilArtistaDAO, PerfilArtistaDAO>();
builder.Services.AddScoped<ICategoriaEventoDAO, CategoriaEventoDAO>();
builder.Services.AddScoped<ISubcategoriaEventoDAO, SubcategoriaEventoDAO>();
builder.Services.AddScoped<ILocalDAO, LocalDAO>();
builder.Services.AddScoped<ICiudadDAO, CiudadDAO>();
builder.Services.AddScoped<ISeguidoresEventoDAO, SeguidoresEventoDAO>();
builder.Services.AddScoped<IEventoDAO, EventoDAO>();
// parte de ELTON


// ---- Registrar Services ----
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPerfilOrganizadorService, PerfilOrganizadorService>();
builder.Services.AddScoped<ICategoriaEventoService, CategoriaEventoService>();
builder.Services.AddScoped<ISubcategoriaEventoService, SubcategoriaEventoService>();
builder.Services.AddScoped<ILocalService, LocalService>();
builder.Services.AddScoped<ICiudadService, CiudadService>();
builder.Services.AddScoped<ISeguidoresService, SeguidoresService>();
builder.Services.AddScoped<IEventoService, EventoService>();

// parte de ELTON


// ====================================


var app = builder.Build();

// CORS
app.UseCors("AllowNextJs");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
