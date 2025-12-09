using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Eventos.DAO
{
    public interface IEventoDAO
    {
        IEnumerable<Evento> GetEventos();
        Evento GetEvento(int id);
        string insertEvento(Evento reg);
        string updateEvento(Evento reg);
        string deleteEvento(int id);

    }
}
