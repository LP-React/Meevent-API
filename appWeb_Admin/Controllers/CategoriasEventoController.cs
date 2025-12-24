using Grpc.Net.Client;
using gRpc_Meevent.Protos;
using Microsoft.AspNetCore.Mvc;
using appWeb_Admin.Models;
namespace appWeb_Admin.Controllers
{
    public class CategoriasEventoController : Controller
    {
        private ServiceCategoria.ServiceCategoriaClient _client;

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
    }
}
