using gRpc_Meevent.Protos;
using Meevent_API.src.Features.SubcategoriasEvento.DAO;

namespace Meevent_API.src.Features.SubcategoriasEvento.Services
{
    public class SubcategoriaEventoService : ISubcategoriaEventoService
    {
        private readonly ISubcategoriaEventoDAO _dao;
        private ServiceSubcategoria.ServiceSubcategoriaClient _client;
        public SubcategoriaEventoService(ISubcategoriaEventoDAO dao, ServiceSubcategoria.ServiceSubcategoriaClient client)
        {
            _dao = dao;
            _client = client;
        }

        public async Task<SubcategoriaEventoListResponseDTO> ObtenerSubcategoriasAsync()
        {
            var lista = await Task.Run(() => _dao.GetSubcategorias());
            return new SubcategoriaEventoListResponseDTO
            {
                Exitoso = true,
                Mensaje = "Listado obtenido correctamente",
                TotalSubCategoriasEvento = lista.Count(),
                SubcategoriaEventos = lista
            };
        }

        public async Task<SubcategoriaEventoDTO> ObtenerSubcategoriaPorIdAsync(int id_subcategoria_evento)
        {
            var lista = await Task.Run(() => _dao.GetSubcategoriaPorId(id_subcategoria_evento));
            return lista.FirstOrDefault(); 
        }

        public async Task<string> RegistrarSubcategoriaAsync(SubcategoriaEventoCrearDTO registro)
        {
            var respuestaGrpc = await _client.GetAllAsync(new EmptySubCategoria());
            
            bool nombreExiste = respuestaGrpc.Items.Any(c =>
                c.NombreSubcategoria.Trim().Equals(registro.NombreSubcategoria.Trim(), StringComparison.OrdinalIgnoreCase)
            );
            
            if (nombreExiste)
            {
                return "Error: El nombre de la subcategoría ya está registrado.";
            }

            registro.SlugSubcategoria = GenerarSlug(registro.NombreSubcategoria);
            return await Task.Run(() => _dao.InsertSubcategoria(registro));
        }

        public async Task<SubcategoriaEventoOperacionResponseDTO> ActualizarSubcategoriaAsync(int id_subcategoria_evento, SubcategoriaEventoEditarDTO subcategoria)
        {
            var todasLasSub = await _client.GetAllAsync(new EmptySubCategoria());

            bool nombreYaExiste = todasLasSub.Items.Any(s =>
                s.IdSubcategoriaEvento != id_subcategoria_evento &&
                s.NombreSubcategoria.Trim().Equals(subcategoria.NombreSubcategoria.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            if (nombreYaExiste)
            {
                return new SubcategoriaEventoOperacionResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "Error: Ya existe otra subcategoría con ese nombre."
                };
            }

            subcategoria.SlugSubcategoria = GenerarSlug(subcategoria.NombreSubcategoria);

            string resultado = await Task.Run(() => _dao.UpdateSubcategoria(id_subcategoria_evento, subcategoria));

            var subActualizada = await ObtenerSubcategoriaPorIdAsync(id_subcategoria_evento);

            bool fueExitoso = !resultado.Contains("Error") &&
                              !resultado.Contains("No se encontró") &&
                              subActualizada != null;

            return new SubcategoriaEventoOperacionResponseDTO
            {
                Exitoso = fueExitoso,
                Mensaje = resultado,
                Subcategoria = subActualizada != null ? new SubcategoriaEventoDetalleDTO
                {
                    IdSubcategoriaEvento = subActualizada.IdSubcategoriaEvento,
                    NombreSubcategoria = subActualizada.NombreSubcategoria,
                    SlugSubcategoria = subActualizada.SlugSubcategoria,
                    CategoriaEventoId = subActualizada.CategoriaEventoId,
                    Estado = subActualizada.Estado
                } : null
            };
        }

        public async Task<SubcategoriaCambiarEstadoResponseDTO> ActivarDesactivarSubcategoriaAsync(int id_subcategoria_evento, bool estado)
        {
            string resultado = await Task.Run(() => _dao.CambiarEstado(id_subcategoria_evento, estado));

            return new SubcategoriaCambiarEstadoResponseDTO
            {
                Exitoso = !resultado.Contains("Error"),
                Mensaje = resultado,
                Estado = estado
            };
        }

        private string GenerarSlug(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return string.Empty;
            string slug = nombre.ToLowerInvariant().Trim();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim();

            return slug;
        }
    }
}
