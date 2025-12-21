    using Meevent_API.src.Core.Entities;
    using Meevent_API.src.Features.Paises;

    namespace Meevent_API.src.Features.Eventos.Services
    {
        public interface IEventoService
        {
            Task<EventoListResponseDTO> ListarEventosAsync();
            Task<EventoResponseDTO?> GetEventoPorSlugAsync(string slug);
            Task<EventoResponseDTO?> GetEventoByIdAsync(int idEvento);
            Task<EventoResponseDTO> InsertEventoAsync(EventoDetalleDTO eventoDto);
            Task<EventoResponseDTO> UpdateEventoAsync(int idEvento, EventoActualizarDTO eventoDto);

            // Otros métodos relacionados con eventos pueden ser añadidos aquí
            Task<EventosCompletosListResponseDTO> ListarEventosCompletosAsync(
                int? idOrganizador,
                int? idSubCategoria,
                int? idLocal,
                bool? eventoGratuito,
                bool? eventoOnline,
                string? estadoEvento,
                string? fchDesde,
                string? fchHasta);
    }
}
