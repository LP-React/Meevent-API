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

        [HttpGet]
        public async Task<IActionResult> Index(string nombre, bool? estado)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            _client = new ServiceCategoria.ServiceCategoriaClient(canal);

            // Creamos la solicitud de filtro
            var request = new FiltroRequest
            {
                Nombre = nombre ?? "", 
            };

            if (estado.HasValue)
            {
                request.Estado = estado.Value;
            }

            var mensaje = await _client.GetFiltradoAsync(request);

            List<CategoriaEventoModel> temporal = new List<CategoriaEventoModel>();
            foreach (var categoria in mensaje.Items)
            {
                temporal.Add(new CategoriaEventoModel
                {
                    IdCategoriaEvento = categoria.IdCategoriaEvento,
                    NombreCategoria = categoria.NombreCategoria,
                    EstaActivo = categoria.Estado
                });
            }

            ViewBag.FiltroNombre = nombre;
            ViewBag.FiltroEstado = estado;

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
                "http://localhost:8080/api/categorias/RegistrarCategoria",
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

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, bool estado)
        {
            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                estado = estado
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync(
                $"http://localhost:8080/api/categorias/ActivarEstado_Desactivar/{id}",
                content
            );

            var body = await response.Content.ReadAsStringAsync();

            string mensaje;

            try
            {
                var jsonDoc = JObject.Parse(body);
                mensaje = jsonDoc["mensaje"]?.ToString();
            }
            catch
            {
                mensaje = "Operación realizada.";
            }

            TempData["MensajeFeedback"] = mensaje;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            var client = new ServiceCategoria.ServiceCategoriaClient(canal);

            var request = new CategoriaRequest
            {
                IdCategoriaEvento = id
            };

            var response = await client.GetByIdAsync(request);

            if (response.Items.Count == 0)
                return NotFound();

            var categoria = response.Items.First();

            var model = new CategoriaEventoModel
            {
                IdCategoriaEvento = categoria.IdCategoriaEvento,
                NombreCategoria = categoria.NombreCategoria,
                EstaActivo = categoria.Estado
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaEventoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new
            {
                nombreCategoria = model.NombreCategoria
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"http://localhost:8080/api/categorias/EditarCategoria/{model.IdCategoriaEvento}"
            )
            {
                Content = content
            };

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var jsonResp = JObject.Parse(body);
            bool exitoso = jsonResp["exitoso"]?.Value<bool>() ?? false;
            string mensaje = jsonResp["mensaje"]?.ToString();

            if (response.IsSuccessStatusCode && exitoso)
            {
                ViewBag.MensajeOk = mensaje;
                return View(model); 
            }

            ViewBag.MensajeError = mensaje ?? "No se pudo actualizar la categoría.";
            return View(model);
        }

    }
}
