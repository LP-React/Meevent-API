using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Paises;
using Meevent_API.src.Features.Paises.DAO;

namespace Meevent_API.src.Features.Eventos.Services
{
    public class EventoService:IEventoService
    {
        private readonly IEventoDAO _eventoDAO;

        public EventoService(IEventoDAO eventoDAO)
        {
            _eventoDAO = eventoDAO;
        }
          public async Task<EventoListResponseDTO> GetAllEventosAsync()
        {
            try
            {
                // 1. Obtener datos del DAO
                var eventos = await _eventoDAO.GetAllAsync();

                // 2. Convertir a DTO (mapeo manual)
                var eventoDTO = eventos.Select(p => new EventoDTO
                {
                    TituloEvento = p.TituloEvento,
                    DescripcionCorta = p.DescripcionCorta,
                    EventoGratuito = p.EventoGratuito,
                    EventoOnline = p.EventoOnline,
                    SubcategoriaEventoId = p.SubcategoriaEventoId,
                    LocalId = p.LocalId
                }).ToList();

                // 3. Retornar respuesta estructurada
                return new EventoListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Eventos obtenidos correctamente",
                    Total_Eventos = eventoDTO.Count,
                    Eventos = eventoDTO
                };
            }
            catch (Exception ex)
            {
                // 4. Manejo de errores centralizado
                return new EventoListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener países: {ex.Message}",
                    Total_Eventos = 0,
                    Eventos = new List<EventoDTO>()
                };
            }
        }


    }

}
