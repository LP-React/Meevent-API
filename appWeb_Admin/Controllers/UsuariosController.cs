using appWeb_Admin.Models;
using Grpc.Net.Client;
using gRpc_Meevent.Protos.Usuario;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace appWeb_Admin.Controllers
{
    public class UsuariosController : Controller
    {
        private ServiceUsuario.ServiceUsuarioClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private ServiceUsuario.ServiceUsuarioClient CrearCliente()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            return new ServiceUsuario.ServiceUsuarioClient(canal);
        }

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string nombre, bool? estado, string tipo)
        {
            _client = CrearCliente();

            var response = await _client.GetAllAsync(new EmptyUsuario());

            IEnumerable<UsuarioModel> usuarios = response.Items.Select(u => new UsuarioModel
            {
                IdUsuario = u.IdUsuario,
                NombreCompleto = u.NombreCompleto,
                CorreoElectronico = u.CorreoElectronico,
                NumeroTelefono = u.NumeroTelefono,
                CuentaActiva = u.CuentaActiva,
                TipoUsuario = u.TipoUsuario
            });

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                usuarios = usuarios.Where(u =>
                    u.NombreCompleto.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            }

            if (estado.HasValue)
            {
                usuarios = usuarios.Where(u => u.CuentaActiva == estado.Value);
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                usuarios = usuarios.Where(u => u.TipoUsuario == tipo);
            }

            ViewBag.FiltroNombre = nombre;
            ViewBag.FiltroEstado = estado;
            ViewBag.FiltroTipo = tipo;

            return View(usuarios.ToList());
        }


        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(
                $"http://localhost:8080/api/Usuarios/buscar/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var root = JObject.Parse(json);

            var usuario = root["usuario"] as JObject;
            if (usuario == null)
                return NotFound("Usuario inválido");

            // --- Ubicación segura ---
            var ubicacion = usuario["ubicacion"] as JObject;

            // --- Perfil organizador seguro ---
            var perfilOrg = usuario["perfilOrganizador"] as JObject;

            var model = new UsuarioDetalleModel
            {
                IdUsuario = usuario["id_usuario"]?.Value<int>() ?? 0,
                NombreCompleto = usuario["nombre_completo"]?.ToString(),
                TipoUsuario = usuario["tipo_usuario"]?.ToString(),
                CorreoElectronico = usuario["correo_electronico"]?.ToString(),
                NumeroTelefono = usuario["numero_telefono"]?.ToString(),
                ImagenPerfilUrl = usuario["imagen_perfil_url"]?.ToString(),
                EmailVerificado = usuario["email_verificado"]?.Value<bool>() ?? false,
                CuentaActiva = usuario["cuenta_activa"]?.Value<bool>() ?? false,

                Ciudad = ubicacion?["nombre_ciudad"]?.ToString(),
                Pais = ubicacion?["nombre_pais"]?.ToString(),

                PerfilOrganizador = perfilOrg != null
                    ? new PerfilOrganizadorModel
                    {
                        NombreOrganizador = perfilOrg["nombre_organizador"]?.ToString(),
                        TelefonoContacto = perfilOrg["telefono_contacto"]?.ToString(),
                        SitioWeb = perfilOrg["sitio_web"]?.ToString()
                    }
                    : null
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, bool cuentaActiva)
        {
            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                cuenta_activa = cuentaActiva
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"http://localhost:8080/api/Usuarios/activarCuenta/{id}"
            )
            {
                Content = content
            };

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensajeError"] = "No se pudo cambiar el estado de la cuenta.";
                return RedirectToAction("Index");
            }

            var jsonResp = JObject.Parse(body);

            TempData["MensajeOk"] = jsonResp["mensaje"]?.ToString();

            return RedirectToAction("Index");
        }




    }
}
