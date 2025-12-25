using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Eventos.DAO
{
    public interface IEventoDAO
    {
        Task<EventoCompletoDTO?> GetEventoPorIdAsync(int idEvento);
        Task<EventoCompletoDTO?> GetEventoPorSlugAsync(string slugEvento);
        Task<string> insertEventoAsync(Evento reg);
        Task<string> updateEventoAsync(Evento reg);

        Task<IEnumerable<EventoCompletoDTO>> ListarEventosCompletosAsync(
            int? idOrganizador,
            int? idSubCategoria,
            int? idLocal,
            bool? eventoGratuito,
            bool? eventoOnline,
            string? estadoEvento,
            string? fchDesde,
            string? fchHasta);

        Task<bool> ValidarEventosAlMismoTiempoAsync(
            int perfilOrganizador,
            int idEvento,
            DateTime fchInicio,
            DateTime fchFin
        );
    }
}
