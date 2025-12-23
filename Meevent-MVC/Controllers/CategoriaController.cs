using Microsoft.AspNetCore.Mvc;
using gRpc_Categorias;
using System.Net.Http.Json;

namespace Meevent_MVC.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ServicioCategorias.ServicioCategoriasClient _grpcClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriaController(ServicioCategorias.ServicioCategoriasClient grpcClient, IHttpClientFactory httpClientFactory)
        {
            _grpcClient = grpcClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _grpcClient.GetAllAsync(new EmptyCat());
                return View(response.Items);
            }
            catch (Exception)
            {
                ViewBag.Error = "Error al conectar con el servicio gRPC.";
                return View(new List<Categoria>());
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string nombreCategoria, string slugCategoria)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var dto = new { NombreCategoria = nombreCategoria ?? "", SlugCategoria = slugCategoria ?? "" };
            var response = await client.PostAsJsonAsync("api/CategoriasEvento/RegistrarCategoria", dto);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var response = await client.GetAsync($"api/CategoriasEvento/BuscarCategoria/{id}");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<CategoriaEventoOperacionResponseDTO>();
                if (resultado != null && resultado.Exitoso && resultado.Categoria != null)
                {
                    var model = new gRpc_Categorias.Categoria
                    {
                        IdCategoria = resultado.Categoria.IdCategoriaEvento,
                        NombreCategoria = resultado.Categoria.NombreCategoria ?? "",
                        SlugCategoria = resultado.Categoria.SlugCategoria ?? "",
                        Estado = resultado.Categoria.Estado
                    };
                    return View(model);
                }
            }
            TempData["Error"] = "La categoría no existe o está desactivada.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombreCategoria, string slugCategoria, bool estado)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var dtoUpdate = new { NombreCategoria = nombreCategoria ?? "", SlugCategoria = slugCategoria ?? "" };
            var dtoEstado = new { Estado = estado };

            await client.PatchAsJsonAsync($"api/CategoriasEvento/EditarCategoria/{id}", dtoUpdate);
            await client.PatchAsJsonAsync($"api/CategoriasEvento/ActivarEstado_Desactivar/{id}", dtoEstado);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReactivarDirecto(int id)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var response = await client.PatchAsJsonAsync($"api/CategoriasEvento/ActivarEstado_Desactivar/{id}", new { estado = true });

            if (response.IsSuccessStatusCode)
                TempData["Success"] = $"Categoría #{id} reactivada correctamente.";
            else
                TempData["Error"] = "No se pudo encontrar o activar el ID.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GestionEstado() => View();

        [HttpPost]
        public async Task<IActionResult> GestionEstado(int id, bool activar)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var response = await client.PatchAsJsonAsync($"api/CategoriasEvento/ActivarEstado_Desactivar/{id}", new { estado = activar });

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = activar ? "Activado con éxito" : "Desactivado con éxito";
                TempData["Tipo"] = "success";
            }
            else
            {
                TempData["Mensaje"] = $"Error al procesar el ID {id}.";
                TempData["Tipo"] = "danger";
            }
            return View();
        }
    }

    public class CategoriaEventoOperacionResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = "";
        public CategoriaDetalleAPI? Categoria { get; set; }
    }

    public class CategoriaDetalleAPI
    {
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; } = "";
        public string SlugCategoria { get; set; } = "";
        public bool Estado { get; set; }
    }
}