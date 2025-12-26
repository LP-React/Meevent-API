using appWeb_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace appWeb_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            // Consumir API de Categorías
            var categoriasResp = await client
                .GetFromJsonAsync<ListarCategoriasResponse>(
                    "http://localhost:5077/api/categorias/ListarCategorias"
                );
            // Subcategorías
            var subcategoriasResp = await client
                .GetFromJsonAsync<SubcategoriasResponse>("http://localhost:5077/api/SubcategoriasEvento/ListarSubCategorias");

            // Eventos
            var eventosResp = await client
                .GetFromJsonAsync<EventosResponse>("http://localhost:5077/api/eventos/getEventos?estadoEvento=publicado");
            ViewBag.EventosActivos = eventosResp?.TotalEventos ?? 0;

            ViewBag.TotalSubcategorias = subcategoriasResp?.TotalSubCategoriasEvento ?? 0;
            ViewBag.TotalCategorias = categoriasResp?.TotalCategoriasEvento ?? 0;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
