using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Paises;
using Meevent_API.src.Features.Paises.DAO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Eventos.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoDAO _eventoDAO;

        public EventoService(IEventoDAO eventoDAO)
        {
            _eventoDAO = eventoDAO;
        }
        public async Task<EventoResponseDTO> GetEventoPorSlugAsync(string slug)
        {
            try
            {
                // 1. Obtener entidad desde DAO
                var evento = await _eventoDAO.GetEventoPorSlugAsync(slug);

                if (evento == null)
                {
                    return new EventoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Evento no encontrado",
                        Evento = null
                    };
                }

                // 2. Mapear a EventoDetalleDTO
                var eventoDTO = new EventoDetalleDTO
                {
                    TituloEvento = evento.TituloEvento,
                    DescripcionEvento = evento.DescripcionEvento,
                    DescripcionCorta = evento.DescripcionCorta,
                    FechaInicio = evento.FechaInicio,
                    FechaFin = evento.FechaFin,
                    EventoGratuito = evento.EventoGratuito,
                    EventoOnline = evento.EventoOnline,
                    CapacidadEvento = evento.CapacidadEvento,
                    SubcategoriaEventoId = evento.SubcategoriaEventoId,
                    LocalId = evento.LocalId
                };

                // 3. Retornar respuesta
                return new EventoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento obtenido correctamente",
                    Evento = eventoDTO
                };
            }
            catch (Exception ex)
            {
                return new EventoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener evento: {ex.Message}",
                    Evento = null
                };
            }
        }







        public async Task<EventoListResponseDTO> ListarEventosAsync()
        {
            try
            {
                var eventos = await _eventoDAO.ListarEventosAsync();

                return new EventoListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Eventos obtenidos correctamente",
                    Total_Eventos = eventos.Count,
                    Eventos = eventos
                };
            }
            catch (Exception ex)
            {
                return new EventoListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener eventos: {ex.Message}",
                    Total_Eventos = 0,
                    Eventos = new List<EventoListadoDTO>()
                };
            }
        }

        // ============================
        // OBTENER EVENTO POR ID
        // ============================
        public async Task<EventoResponseDTO?> GetEventoByIdAsync(int idEvento)
        {
            try
            {
                var evento = await _eventoDAO.GetEventoPorIdAsync(idEvento);

                if (evento == null)
                {
                    return new EventoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Evento no encontrado",
                        Evento = null
                    };
                }

                return new EventoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento obtenido correctamente",
                    Evento = MapToDetalleDTO(evento)
                };
            }
            catch (Exception ex)
            {
                return new EventoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener evento: {ex.Message}",
                    Evento = null
                };
            }
        }



        // ============================
        // INSERTAR EVENTO
        // ============================
        public async Task<EventoResponseDTO> InsertEventoAsync(EventoDetalleDTO dto)
        {
            var evento = new Evento
            {
                TituloEvento = dto.TituloEvento,
                SlugEvento = dto.SlugEvento,
                DescripcionEvento = dto.DescripcionEvento,
                DescripcionCorta = dto.DescripcionCorta,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                EventoGratuito = dto.EventoGratuito,
                EventoOnline = dto.EventoOnline,
                CapacidadEvento = dto.CapacidadEvento,
                SubcategoriaEventoId = dto.SubcategoriaEventoId,
                LocalId = dto.LocalId,
                PerfilOrganizadorId = dto.PerfilOrganizadorId

            };

            string mensaje = await _eventoDAO.insertEventoAsync(evento);

            return new EventoResponseDTO
            {
                Exitoso = true,
                Mensaje = mensaje,
                Evento = new EventoDetalleDTO
                {
                    TituloEvento = evento.TituloEvento,
                    SlugEvento = evento.SlugEvento,
                    DescripcionEvento = evento.DescripcionEvento,
                    DescripcionCorta = evento.DescripcionCorta,
                    FechaInicio = evento.FechaInicio,
                    FechaFin = evento.FechaFin,
                    EventoGratuito = evento.EventoGratuito,
                    EventoOnline = evento.EventoOnline,
                    CapacidadEvento = evento.CapacidadEvento,
                    SubcategoriaEventoId = evento.SubcategoriaEventoId,
                    LocalId = evento.LocalId,
                }
            };
        }



        // ============================
        // ACTUALIZAR EVENTO
        // ============================
        public async Task<EventoResponseDTO> UpdateEventoAsync(int idEvento, EventoActualizarDTO dto)
        {
            try
            {
                var evento = await _eventoDAO.GetEventoPorIdAsync(idEvento);

                if (evento == null)
                {
                    return new EventoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Evento no encontrado",
                        Evento = null
                    };
                }

                evento.TituloEvento = dto.TituloEvento;
                evento.SlugEvento = dto.SlugEvento;
                evento.DescripcionEvento = dto.DescripcionEvento;
                evento.DescripcionCorta = dto.DescripcionCorta;

                evento.FechaInicio = dto.FechaInicio;
                evento.FechaFin = dto.FechaFin;
                evento.EventoGratuito = dto.EventoGratuito;
                evento.EventoOnline = dto.EventoOnline;
                evento.CapacidadEvento = dto.CapacidadEvento;
                evento.SubcategoriaEventoId = dto.SubcategoriaEventoId;
                evento.LocalId = dto.LocalId;

                await _eventoDAO.updateEventoAsync(evento);

                return new EventoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento actualizado correctamente",
                    Evento = MapToDetalleDTO(evento)
                };
            }
            catch (Exception ex)
            {
                return new EventoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al actualizar evento: {ex.Message}",
                    Evento = null
                };
            }
        }



        private EventoDetalleDTO MapToDetalleDTO(Evento evento)
        {
            return new EventoDetalleDTO
            {
                TituloEvento = evento.TituloEvento,
                SlugEvento = evento.SlugEvento,
                DescripcionEvento = evento.DescripcionEvento,
                DescripcionCorta = evento.DescripcionCorta,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                EventoGratuito = evento.EventoGratuito,
                EventoOnline = evento.EventoOnline,
                CapacidadEvento = evento.CapacidadEvento,
                SubcategoriaEventoId = evento.SubcategoriaEventoId,
                LocalId = evento.LocalId
            };
        }

        public async Task<EventoCompletoListResponseDTO> ListarEventosCompletosAsync(
            int? idOrganizador,
            int? idSubCategoria,
            int? idLocal,
            bool? eventoGratuito,
            bool? eventoOnline,
            string? estadoEvento,
            string? fchDesde,
            string? fchHasta)
        {
            try
            {
                var eventos = await _eventoDAO.ListarEventosCompletosAsync(
                    idOrganizador, idSubCategoria, idLocal,
                    eventoGratuito, eventoOnline, estadoEvento,
                    fchDesde, fchHasta);

                var listaEventos = eventos.ToList();

                foreach (var evento in listaEventos)
                {
                    AsignarEstados(evento);
                }

                return new EventoCompletoListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = listaEventos.Any()
                        ? $"Se encontraron {listaEventos.Count} evento(s)"
                        : "No hay eventos disponibles",
                    TotalEventos = listaEventos.Count,
                    Eventos = listaEventos
                };
            }
            catch (Exception ex)
            {
                return new EventoCompletoListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al listar eventos: {ex.Message}",
                    TotalEventos = 0,
                    Eventos = new List<EventoCompletoDTO>()
                };
            }
        }



        private void AsignarEstados(EventoCompletoDTO evento)
        {
            // ESTADO PARA ADMINISTRADOR
            // Estados posibles: borrador, publicado, cancelado, finalizado, eliminado

            evento.EstadoEventoAdmin = evento.EstadoEvento switch
            {
                "borrador" => "borrador",
                "publicado" => "publicado",
                "cancelado" => "eliminado",
                "finalizado" => "finalizado",
                _ => "borrador"
            };

            // Parsear las fechas del evento (vienen como string)
            DateTime fechaInicio;
            DateTime fechaFin;

            try
            {
                // Intentar parsear las fechas
                // Formato esperado: "yyyy-MM-dd HH:mm:ss" o "yyyy-MM-ddTHH:mm:ss"
                if (!DateTime.TryParse(evento.FechaInicio, out fechaInicio))
                {
                    evento.EstadoEventoCliente = "próximo"; // Default si hay error
                    return;
                }

                if (!DateTime.TryParse(evento.FechaFin, out fechaFin))
                {
                    evento.EstadoEventoCliente = "próximo"; // Default si hay error
                    return;
                }
            }
            catch
            {
                evento.EstadoEventoCliente = "próximo";
                return;
            }

            var ahora = DateTime.Now;

            // Si el evento no está publicado, no mostramos estado al cliente
            if (evento.EstadoEvento != "publicado")
            {
                evento.EstadoEventoCliente = "no disponible";
                return;
            }

            // Lógica para determinar estado del cliente
            if (ahora < fechaInicio)
            {
                // El evento aún no ha comenzado
                // Verificar si está agotado
                if (evento.Estadisticas != null &&
                    evento.Estadisticas.EntradasVendidas >= evento.CapacidadEvento)
                {
                    evento.EstadoEventoCliente = "agotado";
                }
                else
                {
                    evento.EstadoEventoCliente = "próximo";
                }
            }
            else if (ahora >= fechaInicio && ahora <= fechaFin)
            {
                // El evento está en curso
                evento.EstadoEventoCliente = "en curso";
            }
            else // ahora > fechaFin 
            {
                // El evento ya finalizó
                evento.EstadoEventoCliente = "finalizado";
            }
        }
    }
}
