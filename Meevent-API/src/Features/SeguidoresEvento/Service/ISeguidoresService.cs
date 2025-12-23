namespace Meevent_API.src.Features.SeguidoresEvento.Service
{
    public interface ISeguidoresService
    {
        Task<EventoSeguidoListResponseDTO> GetEventosSeguidosPorUsuarioAsync(int idUsuario);
    }
}
