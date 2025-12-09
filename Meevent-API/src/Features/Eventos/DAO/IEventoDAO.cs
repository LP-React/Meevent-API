using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Eventos.DAO
{
    public interface IEventoDAO
    {
        Task<IEnumerable<Evento>> GetAllAsync();
        Evento GetEvento(int id);
        string insertEvento(Evento reg);
        string updateEvento(Evento reg);
        string deleteEvento(int id);

    }
}
