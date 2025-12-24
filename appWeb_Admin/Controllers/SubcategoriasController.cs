using appWeb_Admin.Models;
using Grpc.Net.Client;
using gRpc_Meevent.Protos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace appWeb_Admin.Controllers
{
    public class SubcategoriasController : Controller
    {
        private ServiceSubcategoria.ServiceSubcategoriaClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        public SubcategoriasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task CargarCategoriasActivas()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7292/api/categorias/ListarCategorias");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();

                // 1. Parseamos el cuerpo como un objeto dinámico o JObject
                var jsonDoc = JObject.Parse(body);

                // 2. Extraemos específicamente el arreglo "categoriasEvento"
                var categoriasJson = jsonDoc["categoriasEvento"]?.ToString();

                if (!string.IsNullOrEmpty(categoriasJson))
                {
                    var lista = JsonConvert.DeserializeObject<List<CategoriaEventoModel>>(categoriasJson);
                    ViewBag.Categorias = lista;
                }
                else
                {
                    ViewBag.Categorias = new List<CategoriaEventoModel>();
                }
            }
            else
            {
                ViewBag.Categorias = new List<CategoriaEventoModel>();
                ViewBag.MensajeError = "No se pudieron cargar las categorías desde la API.";
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string nombre, bool? estado, int? idCategoria)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");

            var clientCat = new ServiceCategoria.ServiceCategoriaClient(canal);
            var resCategorias = await clientCat.GetAllAsync(new Empty());
            ViewBag.Categorias = resCategorias.Items;

            _client = new ServiceSubcategoria.ServiceSubcategoriaClient(canal);
            var request = new SubcategoriaFiltroRequest();

            if (!string.IsNullOrEmpty(nombre)) request.Nombre = nombre;
            if (estado.HasValue) request.Estado = estado.Value;
            if (idCategoria.HasValue) request.IdCategoria = idCategoria.Value;

            var mensaje = await _client.GetFiltradoAsync(request);

            List<SubcategoriaModel> temporal = new List<SubcategoriaModel>();
            foreach (var sub in mensaje.Items)
            {
                temporal.Add(new SubcategoriaModel
                {
                    IdSubcategoriaEvento = sub.IdSubcategoriaEvento,
                    NombreSubcategoria = sub.NombreSubcategoria,
                    SlugSubcategoria = sub.SlugSubcategoria,
                    IdCategoriaEvento = sub.CategoriaEventoId,
                    NombreCategoria = sub.NombreCategoria,
                    EstaActivo = sub.Estado
                });
            }

            ViewBag.FiltroNombre = nombre;
            ViewBag.FiltroEstado = estado;
            ViewBag.FiltroCategoria = idCategoria;

            return View(temporal);
        }

        // GET: SubcategoriasEvento/Crear
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarCategoriasActivas();
            return View();
        }

        // POST: SubcategoriasEvento/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(SubcategoriaModel reg)
        {
            // Limpieza de validación (campos que no vienen del form)
            ModelState.Remove("SlugSubcategoria");
            ModelState.Remove("NombreCategoria");

            if (!ModelState.IsValid)
            {
                await CargarCategoriasActivas();
                return View(reg);
            }

            var dto = new
            {
                nombreSubcategoria = reg.NombreSubcategoria,
                categoriaEventoId = reg.IdCategoriaEvento
            };

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7292/api/SubcategoriasEvento/RegistrarSubCategoria", content);
            var body = await response.Content.ReadAsStringAsync();

            // Parseamos la respuesta siempre para saber qué dijo la API
            var jsonDoc = JObject.Parse(body);
            string mensajeApi = jsonDoc["mensaje"]?.ToString() ?? "Sin respuesta del servidor";

            // Un registro es realmente exitoso si el status es 200/201 Y el mensaje no empieza con "Error"
            if (response.IsSuccessStatusCode && !mensajeApi.StartsWith("Error"))
            {
                TempData["MensajeOk"] = mensajeApi; // "Subcategoría registrada correctamente"
                return RedirectToAction("Index");
            }
            else
            {
                // Si el status falló O el mensaje dice "Error: ..."
                ViewBag.MensajeError = mensajeApi;
                await CargarCategoriasActivas();
                return View(reg);
            }
        }

        // GET: SubcategoriasEvento/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            var clientSub = new ServiceSubcategoria.ServiceSubcategoriaClient(canal);
            var subResponse = await clientSub.GetByIdAsync(new SubcategoriaRequest { IdSubcategoriaEvento = id });

            if (subResponse.Items.Count == 0) return NotFound();

            await CargarCategoriasActivas();

            var sub = subResponse.Items.First();
            var model = new SubcategoriaModel
            {
                IdSubcategoriaEvento = sub.IdSubcategoriaEvento,
                NombreSubcategoria = sub.NombreSubcategoria,
                IdCategoriaEvento = sub.CategoriaEventoId,
                EstaActivo = sub.Estado
            };

            return View(model);
        }

        // POST: SubcategoriasEvento/Editar
        [HttpPost]
        public async Task<IActionResult> Editar(SubcategoriaModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCategoriasActivas();
                return View(model);
            }

            var dto = new
            {
                nombreSubcategoria = model.NombreSubcategoria,
                categoriaEventoId = model.IdCategoriaEvento,
                estado = model.EstaActivo
            };

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync(
                $"https://localhost:7292/api/SubcategoriasEvento/EditarSubCategoria/{model.IdSubcategoriaEvento}",
                content);

            var body = await response.Content.ReadAsStringAsync();
            var resJson = JObject.Parse(body);

            bool exitoso = resJson["exitoso"]?.Value<bool>() ?? false;
            string mensaje = resJson["mensaje"]?.ToString() ?? "Error al actualizar.";

            if (response.IsSuccessStatusCode && exitoso)
            {
                TempData["MensajeOk"] = mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.MensajeError = mensaje;
                await CargarCategoriasActivas();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, bool estado)
        {
            var client = _httpClientFactory.CreateClient();
            var dto = new { estado = estado };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"https://localhost:7292/api/SubcategoriasEvento/ActivarEstado_Desctivar/{id}", content);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeOk"] = "Estado actualizado correctamente.";
            }
            else
            {
                try
                {
                    var jsonDoc = JObject.Parse(body);
                    TempData["MensajeError"] = jsonDoc["mensaje"]?.ToString() ?? "No se pudo cambiar el estado.";
                }
                catch
                {
                    TempData["MensajeError"] = "Error de comunicación con la API.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}
