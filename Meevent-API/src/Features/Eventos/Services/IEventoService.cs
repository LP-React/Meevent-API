using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Paises;

namespace Meevent_API.src.Features.Eventos.Services
{
    public interface IEventoService
    {
        Task<EventoListResponseDTO> GetAllEventosAsync();
        Evento? GetEventoPorSlug(string slug);


    }
}
