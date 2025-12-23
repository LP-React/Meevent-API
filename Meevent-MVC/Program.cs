using gRpc_SubCategorias;
using gRpc_Categorias;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// ---------------------------------------------------------
// 1. CONFIGURACIÓN CLIENTES gRPC (Puerto 7185)
// ---------------------------------------------------------
var gRpcAddress = "https://localhost:7185";

builder.Services.AddGrpcClient<ServicioCategorias.ServicioCategoriasClient>(o =>
{
    o.Address = new Uri(gRpcAddress);
})
.ConfigurePrimaryHttpMessageHandler(() => {
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

builder.Services.AddGrpcClient<ServicioSubcategorias.ServicioSubcategoriasClient>(o =>
{
    o.Address = new Uri(gRpcAddress);
})
.ConfigurePrimaryHttpMessageHandler(() => {
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

// ---------------------------------------------------------
// 2. CONFIGURACIÓN API REST (Puerto 7292)
// ---------------------------------------------------------
builder.Services.AddHttpClient("MeeventApi", client =>
{
    // Usamos el puerto 7292 detectado en tus logs para la API REST
    client.BaseAddress = new Uri("https://localhost:7292/");

    // Forzamos HTTP/1.1 para evitar conflictos de protocolo con gRPC
    client.DefaultRequestVersion = System.Net.HttpVersion.Version11;
    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

var app = builder.Build();

// Configuración del pipeline de HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categoria}/{action=Index}/{id?}");

app.Run();