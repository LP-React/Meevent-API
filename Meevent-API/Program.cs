using Meevent_API.src.Features.Paises.DAO;
using Meevent_API.src.Features.Paises.Services;
using Meevent_API.src.Features.Paises.Services.Interfaces;
using Meevent_API.src.Features.Usuarios.DAO;
using Meevent_API.src.Features.Usuarios.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ===== INYECCIÓN DE DEPENDENCIAS =====
// ---- Registrar DAOs ----
builder.Services.AddScoped<IPaisDAO, PaisDAO>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();
// parte de FRANCO
// parte de ELTON


// ---- Registrar Services ----
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// parte de FRANCO
// parte de ELTON


// ====================================


var app = builder.Build();

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
