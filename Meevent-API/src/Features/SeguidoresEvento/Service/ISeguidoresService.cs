namespace Meevent_API.src.Features.SeguidoresEvento.Service
{
    public interface ISeguidoresService
    {
        Task<EventoSeguidoListResponseDTO> GetEventosSeguidosPorUsuarioAsync(int idUsuario);

        Task<SeguimientoResponseDTO> SeguirEventoAsync(int usuarioId, int eventoId);

        Task<BaseResponseDTO> DejarDeSeguirEventoAsync(int usuarioId, int eventoId);
    }
}
