using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Paises;
using Meevent_API.src.Features.Paises.DAO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Eventos.Services
{
    public class EventoService:IEventoService
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
        // ============================
        // OBTENER EVENTO POR ID
        // ============================
        public async Task<EventoResponseDTO?> GetEventoByIdAsync(int idEvento)
        {
            try
            {
                var evento = await _eventoDAO.GetEvento(idEvento);

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
                    PerfilOrganizadorId = evento.PerfilOrganizadorId
                }
            };
        }



        // ============================
        // ACTUALIZAR EVENTO
        // ============================
        public async Task<EventoResponseDTO> UpdateEventoAsync(int idEvento, EventoDetalleDTO dto)
        {
            try
            {
                var evento = await _eventoDAO.GetEvento(idEvento);

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

    }

}
