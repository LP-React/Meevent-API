    namespace Meevent_API.src.Features.SubcategoriasEvento.Services
    {
        public interface ISubcategoriaEventoService
        {
            Task<SubcategoriaEventoListResponseDTO> ObtenerSubcategoriasAsync();

            Task<SubcategoriaEventoDTO> ObtenerSubcategoriaPorIdAsync(int id_subcategoria_evento);

            Task<string> RegistrarSubcategoriaAsync(SubcategoriaEventoCrearDTO registro);

            Task<SubcategoriaEventoOperacionResponseDTO> ActualizarSubcategoriaAsync(int id_subcategoria_evento, SubcategoriaEventoEditarDTO subcategoria);

            Task<SubcategoriaCambiarEstadoResponseDTO> ActivarDesactivarSubcategoriaAsync(int id_subcategoria_evento, bool estado);
        }
    }
