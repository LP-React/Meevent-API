namespace Meevent_API.src.Features.SeguidoresEvento.DAO
{
    public interface ISeguidoresEventoDAO
    {
        Task<List<EventoSeguidoDTO>> ListarEventosSeguidosPorUsuarioAsync(int idUsuario);
    }
}
