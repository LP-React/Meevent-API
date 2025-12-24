using appWeb_Admin.Models;
using Grpc.Net.Client;
using gRpc_Meevent.Protos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
namespace appWeb_Admin.Controllers
{
    public class CategoriasEventoController : Controller
    {
        private ServiceCategoria.ServiceCategoriaClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoriasEventoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            _client = new ServiceCategoria.ServiceCategoriaClient(canal);

            var request = new Empty();
            var mensaje = await _client.GetAllAsync(request);

            List<CategoriaEventoModel> temporal = new List<CategoriaEventoModel>();
            foreach(var categoria in mensaje.Items)
            {
                temporal.Add(new CategoriaEventoModel
                {
                    IdCategoriaEvento = categoria.IdCategoriaEvento,
                    NombreCategoria = categoria.NombreCategoria,
                    EstaActivo = categoria.Estado
                });
            }
            return View(temporal);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaEventoModel reg)
        {
            if (!ModelState.IsValid)
                return View(reg);

            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(reg);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://localhost:7292/api/categorias/RegistrarCategoria",
                content
            );

            var body = await response.Content.ReadAsStringAsync();

            string? mensajeApi;

            try
            {
                var jsonDoc = JObject.Parse(body);
                mensajeApi = jsonDoc["mensaje"]?.ToString();
            }
            catch
            {
                mensajeApi = body.Replace("\"", "");
            }

            ViewBag.Mensaje = mensajeApi;
            return View(reg);

        }

    }
}
