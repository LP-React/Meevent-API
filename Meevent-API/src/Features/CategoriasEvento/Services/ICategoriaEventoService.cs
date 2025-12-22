namespace Meevent_API.src.Features.CategoriasEvento.Services
{
    public interface ICategoriaEventoService
    {
        Task<CategoriaEventoListResponseDTO> ObtenerCategoriasAsync();

        Task<CategoriaEventoDTO> ObtenerCategoriaPorIdAsync(int id_categoria_evento);

        Task<string> RegistrarCategoriaAsync(CategoriaEventoCrearDTO registro);

        Task<CategoriaEventoOperacionResponseDTO> ActualizarCategoriaAsync(
            int id_categoria_evento,
            CategoriaEventoEditarDTO categoria
        );

        Task<CategoriaCambiarEstadoResponseDTO> ActivarDesactivarCategoriaAsync(
            int id_categoria_evento,
            bool estado
        );
    }
}
