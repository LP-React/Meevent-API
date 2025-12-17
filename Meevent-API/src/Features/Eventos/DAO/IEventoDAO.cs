using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Eventos.DAO
{
    public interface IEventoDAO
    {
        Task<IEnumerable<Evento>> GetAllAsync();
        Task<Evento?> GetEvento(int idEvento);
        Task<Evento> GetEventoPorSlugAsync(string slugEvento);
        Task<string> insertEventoAsync(Evento reg);
        Task<string> updateEventoAsync(Evento reg);
        Task<string> deleteEventoAsync(int id);

    }
}
