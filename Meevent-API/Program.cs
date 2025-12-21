using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Eventos.Services;
﻿using gRcp_Paises;
using Meevent_API.src.Features.Paises.Services;
using Meevent_API.src.Features.Paises.Services.Interfaces;
using Meevent_API.src.Features.Usuarios.DAO;
using Meevent_API.src.Features.Usuarios.Service;

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
// parte de FRANCO
builder.Services.AddScoped<IEventoDAO, EventoDAO>();
// parte de ELTON


// ---- Registrar Services ----
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// parte de FRANCO
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
