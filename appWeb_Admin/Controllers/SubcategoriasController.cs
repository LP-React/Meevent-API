using appWeb_Admin.Models;
using Grpc.Net.Client;
using gRpc_Meevent.Protos;
using Microsoft.AspNetCore.Mvc;

namespace appWeb_Admin.Controllers
{
    public class SubcategoriasController : Controller
    {
        private ServiceSubcategoria.ServiceSubcategoriaClient _client;

        [HttpGet]
        public async Task<IActionResult> Index(string nombre, bool? estado, int? idCategoria)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7111");

            // 1. Obtener Categorías para el combo
            var clientCat = new ServiceCategoria.ServiceCategoriaClient(canal);
            var resCategorias = await clientCat.GetAllAsync(new Empty());
            ViewBag.Categorias = resCategorias.Items; // Pasamos la lista a la vista

            // 2. Obtener Subcategorías (tu código actual)
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
    }
}
