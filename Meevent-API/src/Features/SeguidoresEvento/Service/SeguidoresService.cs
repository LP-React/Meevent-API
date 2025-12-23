using Meevent_API.src.Features.SeguidoresEvento.DAO;

namespace Meevent_API.src.Features.SeguidoresEvento.Service
{
    public class SeguidoresService : ISeguidoresService
    {
        private readonly ISeguidoresEventoDAO _seguimientoDAO;
        private readonly ILogger<SeguidoresService> _logger;

        public SeguidoresService(ISeguidoresEventoDAO seguimientoDAO, ILogger<SeguidoresService> logger)
        {
            _seguimientoDAO = seguimientoDAO;
            _logger = logger;
        }

        public async Task<EventoSeguidoListResponseDTO> GetEventosSeguidosPorUsuarioAsync(int idUsuario)
        {
            try
            {
                // 1. Llamada al DAO para obtener la data cruda
                var eventos = await _seguimientoDAO.ListarEventosSeguidosPorUsuarioAsync(idUsuario);

                // 2. Procesar estados dinámicos (Lógica de negocio)
                foreach (var evento in eventos)
                {
                    AsignarEstadoCliente(evento);
                }

                // 3. Retornar respuesta exitosa
                return new EventoSeguidoListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = eventos.Any()
                        ? $"Se encontraron {eventos.Count} eventos seguidos."
                        : "El usuario no sigue ningún evento actualmente.",
                    TotalResultados = eventos.Count,
                    Eventos = eventos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos seguidos para el usuario {IdUsuario}", idUsuario);

                return new EventoSeguidoListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error interno al procesar seguidos: {ex.Message}",
                    TotalResultados = 0,
                    Eventos = new List<EventoSeguidoDTO>()
                };
            }
        }

        private void AsignarEstadoCliente(EventoSeguidoDTO evento)
        {
            DateTime fechaInicio;
            DateTime fechaFin;

            if (!DateTime.TryParse(evento.FechaInicio, out fechaInicio) ||
                !DateTime.TryParse(evento.FechaFin, out fechaFin))
            {
                return;
            }


            var ahora = DateTime.Now;

            if (ahora < fechaInicio)
            {
                evento.EstadoEvento = "próximo";
            }
            else if (ahora >= fechaInicio && ahora <= fechaFin)
            {
                evento.EstadoEvento = "en curso";
            }
            else
            {
                evento.EstadoEvento = "finalizado";
            }
        }
    }
}
