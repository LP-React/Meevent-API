using Meevent_API.src.Features.Paises.DAO;
using Meevent_API.src.Features.Paises.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ===== INYECCIÓN DE DEPENDENCIAS =====
// ---- Registrar DAOs ----
builder.Services.AddScoped<IPaisDAO, PaisDAO>();
// parte de FRANCO
// parte de ELTON


// ---- Registrar Services ----
builder.Services.AddScoped<IPaisService, PaisService>();
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
