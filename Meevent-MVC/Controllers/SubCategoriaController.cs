using Microsoft.AspNetCore.Mvc;
using gRpc_SubCategorias;
using gRpc_Categorias;
using System.Net.Http.Json;

namespace Meevent_MVC.Controllers
{
    public class SubCategoriaController : Controller
    {
        private readonly ServicioSubcategorias.ServicioSubcategoriasClient _subClient;
        private readonly ServicioCategorias.ServicioCategoriasClient _catClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public SubCategoriaController(
            ServicioSubcategorias.ServicioSubcategoriasClient subClient,
            ServicioCategorias.ServicioCategoriasClient catClient,
            IHttpClientFactory httpClientFactory)
        {
            _subClient = subClient;
            _catClient = catClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _subClient.GetAllAsync(new EmptySub());
                return View(response.Items);
            }
            catch (Exception)
            {
                ViewBag.Error = "Error al conectar con el servidor gRPC.";
                return View(new List<Subcategoria>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var catResponse = await _catClient.GetAllAsync(new EmptyCat());
            ViewBag.Categorias = catResponse.Items;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombreSubcategoria, string slugSubcategoria, int categoriaEventoId)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var dto = new 
            { 
                NombreSubcategoria = nombreSubcategoria ?? "", 
                SlugSubcategoria = slugSubcategoria ?? "", 
                CategoriaEventoId = categoriaEventoId 
            };

            var response = await client.PostAsJsonAsync("api/SubcategoriasEvento/RegistrarSubCategoria", dto);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

            var catResponse = await _catClient.GetAllAsync(new EmptyCat());
            ViewBag.Categorias = catResponse.Items;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var response = await client.GetAsync($"api/SubcategoriasEvento/BuscarSubCategorias/{id}");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<SubcategoriaOperacionResponseDTO>();
                
                if (resultado != null && resultado.Exitoso && resultado.Subcategoria != null)
                {
                    var catResponse = await _catClient.GetAllAsync(new EmptyCat());
                    ViewBag.Categorias = catResponse.Items;

                    var model = new gRpc_SubCategorias.Subcategoria
                    {
                        IdSubcategoria = resultado.Subcategoria.IdSubcategoriaEvento,
                        NombreSubcategoria = resultado.Subcategoria.NombreSubcategoria ?? "",
                        SlugSubcategoria = resultado.Subcategoria.SlugSubcategoria ?? "",
                        IdCategoria = resultado.Subcategoria.CategoriaEventoId,
                        Estado = resultado.Subcategoria.Estado
                    };
                    return View(model);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nombreSubcategoria, string slugSubcategoria, int categoriaEventoId)
        {
            if (id <= 0) return RedirectToAction(nameof(Index));

            var client = _httpClientFactory.CreateClient("MeeventApi");

            var dtoUpdate = new
            {
                NombreSubcategoria = nombreSubcategoria ?? "",
                SlugSubcategoria = slugSubcategoria ?? "",
                CategoriaEventoId = categoriaEventoId,
                Estado = true 
            };

            await client.PatchAsJsonAsync($"api/SubcategoriasEvento/EditarSubCategoria/{id}", dtoUpdate);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GestionEstado()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GestionEstado(int id, bool activar)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            var response = await client.PatchAsJsonAsync($"api/SubcategoriasEvento/ActivarEstado_Desctivar/{id}", new { estado = activar });

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = activar ? "Subcategoría ACTIVADA" : "Subcategoría DESACTIVADA";
                TempData["Tipo"] = "success";
            }
            else
            {
                TempData["Mensaje"] = $"Error con el ID {id}";
                TempData["Tipo"] = "danger";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReactivarDirecto(int id)
        {
            var client = _httpClientFactory.CreateClient("MeeventApi");
            await client.PatchAsJsonAsync($"api/SubcategoriasEvento/ActivarEstado_Desctivar/{id}", new { estado = true });
            return RedirectToAction(nameof(Index));
        }
    }

    public class SubcategoriaOperacionResponseDTO
    {
        public bool Exitoso { get; set; }
        public SubcategoriaDetalleAPI? Subcategoria { get; set; }
    }

    public class SubcategoriaDetalleAPI
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; } = "";
        public string SlugSubcategoria { get; set; } = "";
        public int CategoriaEventoId { get; set; }
        public bool Estado { get; set; }
    }
}