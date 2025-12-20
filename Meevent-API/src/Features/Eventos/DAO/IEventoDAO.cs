using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Eventos.DAO
{
    public interface IEventoDAO
    {
        Task<List<EventoListadoDTO>> ListarEventosAsync();
        Task<Evento?> GetEventoPorIdAsync(int idEvento);
        Task<Evento?> GetEventoPorSlugAsync(string slugEvento);
        Task<string> insertEventoAsync(Evento reg);
        Task<string> updateEventoAsync(Evento reg);
        Task<string> deleteEventoAsync(int id);

        // Otros métodos relacionados con eventos pueden ser añadidos aquí
        Task<IEnumerable<EventoCompletoDTO>> ListarEventosCompletosAsync(int? idOrganizador,int? idSubCategoria, int? idLocal);

    }
}
