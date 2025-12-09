using Meevent_API.src.Features.Paises;

namespace Meevent_API.src.Features.Eventos.Services
{
    public interface IEventoService
    {
        Task<EventoListResponseDTO> GetAllEventosAsync();

    }
}
