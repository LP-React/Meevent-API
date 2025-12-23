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

        public async Task<SeguimientoResponseDTO> SeguirEventoAsync(int usuarioId, int eventoId)
        {
            try
            {
                var evento = await _seguimientoDAO.InsertarYObtenerSeguimientoAsync(usuarioId, eventoId);

                if (evento == null)
                {
                    return new SeguimientoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "El usuario ya sigue este evento o el evento no está disponible."
                    };
                }

                AsignarEstadoCliente(evento);

                return new SeguimientoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento seguido correctamente.",
                    Evento = evento 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al seguir evento");
                return new SeguimientoResponseDTO { Exitoso = false, Mensaje = ex.Message };
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

        public async Task<BaseResponseDTO> DejarDeSeguirEventoAsync(int usuarioId, int eventoId)
        {
            try
            {
                bool eliminado = await _seguimientoDAO.EliminarSeguimientoAsync(usuarioId, eventoId);

                if (eliminado)
                {
                    return new BaseResponseDTO { Exitoso = true, Mensaje = "Ya no sigues este evento." };
                }
                else
                {
                    return new BaseResponseDTO { Exitoso = false, Mensaje = "No estabas siguiendo este evento." };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dejar de seguir evento");
                return new BaseResponseDTO { Exitoso = false, Mensaje = "Error: " + ex.Message };
            }
        
        }
    }
}
