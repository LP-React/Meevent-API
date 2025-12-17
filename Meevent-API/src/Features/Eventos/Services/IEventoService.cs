using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Paises;

namespace Meevent_API.src.Features.Eventos.Services
{
    public interface IEventoService
    {
        Task<EventoListResponseDTO> GetAllEventosAsync();
        Task<EventoResponseDTO?> GetEventoPorSlugAsync(string slug);
        Task<EventoResponseDTO?> GetEventoByIdAsync(int idEvento);
        Task<EventoResponseDTO> InsertEventoAsync(EventoDetalleDTO eventoDto);
        Task<EventoResponseDTO> UpdateEventoAsync(int idEvento, EventoDetalleDTO eventoDto);
    }
}
