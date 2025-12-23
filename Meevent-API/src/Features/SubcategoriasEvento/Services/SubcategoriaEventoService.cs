using Meevent_API.src.Features.SubcategoriasEvento.DAO;

namespace Meevent_API.src.Features.SubcategoriasEvento.Services
{
    public class SubcategoriaEventoService : ISubcategoriaEventoService
    {
        private readonly ISubcategoriaEventoDAO _dao;

        public SubcategoriaEventoService(ISubcategoriaEventoDAO dao)
        {
            _dao = dao;
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
            return await Task.Run(() => _dao.InsertSubcategoria(registro));
        }

        public async Task<SubcategoriaEventoOperacionResponseDTO> ActualizarSubcategoriaAsync(int id_subcategoria_evento, SubcategoriaEventoEditarDTO subcategoria)
        {
            string resultado = await Task.Run(() => _dao.UpdateSubcategoria(id_subcategoria_evento, subcategoria));

            var subActualizada = await ObtenerSubcategoriaPorIdAsync(id_subcategoria_evento);

            return new SubcategoriaEventoOperacionResponseDTO
            {
                Exitoso = !resultado.Contains("Error") && subActualizada != null,
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
    }
    }
