using appWeb_Admin.Models;
using Grpc.Net.Client;
using gRpc_Meevent.Protos.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace appWeb_Admin.Controllers
{
    public class UsuariosController : Controller
    {
        private ServiceUsuario.ServiceUsuarioClient _client;

        private ServiceUsuario.ServiceUsuarioClient CrearCliente()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");
            return new ServiceUsuario.ServiceUsuarioClient(canal);
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
            _client = CrearCliente();

            var request = new UsuarioRequest
            {
                IdUsuario = id
            };

            var response = await _client.GetByIdAsync(request);

            if (response.Items.Count == 0)
                return NotFound();

            var u = response.Items.First();

            var model = new UsuarioModel
            {
                IdUsuario = u.IdUsuario,
                NombreCompleto = u.NombreCompleto,
                CorreoElectronico = u.CorreoElectronico,
                NumeroTelefono = u.NumeroTelefono,
                CuentaActiva = u.CuentaActiva,
                TipoUsuario = u.TipoUsuario
            };

            return View(model);
        }
    
    }
}
