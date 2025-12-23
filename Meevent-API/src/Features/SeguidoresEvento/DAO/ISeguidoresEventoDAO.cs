namespace Meevent_API.src.Features.SeguidoresEvento.DAO
{
    public interface ISeguidoresEventoDAO
    {
        Task<List<EventoSeguidoDTO>> ListarEventosSeguidosPorUsuarioAsync(int idUsuario);

        Task<EventoSeguidoDTO?> InsertarYObtenerSeguimientoAsync(int usuarioId, int eventoId);

        Task<bool> EliminarSeguimientoAsync(int usuarioId, int eventoId);
    }
}
