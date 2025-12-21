using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Paises;
using Meevent_API.src.Features.Paises.DAO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Meevent_API.src.Features.Eventos.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoDAO _eventoDAO;

        public EventoService(IEventoDAO eventoDAO)
        {
            _eventoDAO = eventoDAO;
        }
        public async Task<EventoCompletoResponseDTO> GetEventoPorSlugAsync(string slug)
        {
            try
            {
                var evento = await _eventoDAO.GetEventoPorSlugAsync(slug);

                if (evento == null)
                {
                    return new EventoCompletoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "El evento solicitado no existe o el slug es incorrecto.",
                        Evento = null
                    };
                }

                return new EventoCompletoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento obtenido exitosamente.",
                    Evento = evento
                };
            }
            catch (Exception ex)
            {
                return new EventoCompletoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error interno al procesar la solicitud: {ex.Message}",
                    Evento = null
                };
            }
        }

        // OBTENER EVENTO POR ID
        public async Task<EventoCompletoResponseDTO?> GetEventoByIdAsync(int idEvento)
        {
            try
            {
                var evento = await _eventoDAO.GetEventoPorIdAsync(idEvento);

                if (evento == null)
                {
                    return new EventoCompletoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Evento no encontrado",
                        Evento = null
                    };
                }

                return new EventoCompletoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Evento obtenido correctamente",
                    Evento = evento
                };
            }
            catch (Exception ex)
            {
                return new EventoCompletoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener evento: {ex.Message}",
                    Evento = null
                };
            }
        }

        // INSERTAR NUEVO EVENTO
        public async Task<EventoCompletoResponseDTO> InsertEventoAsync(EventoCrearDTO dto)
        {
            try
            {
                if (dto.FechaFin <= dto.FechaInicio)
                {
                    return new EventoCompletoResponseDTO { 
                        Exitoso = false,
                        Mensaje = "La fecha de finalización debe ser posterior a la de inicio." 
                    };
                }

                if(dto.FechaInicio < DateTime.Now)
                {
                    return new EventoCompletoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "La fecha de inicio no puede ser posterior a la fecha actual."
                    };
                }

                    string slugGenerado = GenerarSlug(dto.TituloEvento);

                var entidad = new Evento
                {
                    TituloEvento = dto.TituloEvento.Trim(),
                    SlugEvento = slugGenerado,
                    DescripcionEvento = dto.DescripcionEvento,
                    DescripcionCorta = dto.DescripcionCorta,
                    FechaInicio = dto.FechaInicio.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    FechaFin = dto.FechaFin.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    ZonaHoraria = dto.ZonaHoraria,
                    CapacidadEvento = dto.CapacidadEvento,
                    EventoGratuito = dto.EventoGratuito,
                    EventoOnline = dto.EventoOnline,
                    ImagenPortadaUrl = dto.ImagenPortadaUrl,
                    PerfilOrganizadorId = dto.PerfilOrganizadorId,
                    SubcategoriaEventoId = dto.SubcategoriaEventoId,
                    LocalId = dto.LocalId,
                    EstadoEvento = dto.EstadoEvento,
                };

                string mensajeResultado = await _eventoDAO.insertEventoAsync(entidad);

                return new EventoCompletoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = mensajeResultado,
                    Evento = new EventoCompletoDTO
                    {
                        TituloEvento = dto.TituloEvento,
                        SlugEvento = slugGenerado
                    }
                };
            }
            catch (Exception ex)
            {
                return new EventoCompletoResponseDTO { 
                    Exitoso = false, Mensaje = "Ocurrió un error interno: " + ex.Message 
                };
            }
        }

        // ACTUALIZAR EVENTO
        public async Task<EventoCompletoResponseDTO> UpdateEventoAsync(int idEvento, EventoActualizarDTO dto)
        {
            try
            {
                if (dto.FechaFin <= dto.FechaInicio)
                {
                    return new EventoCompletoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "La fecha de finalización debe ser posterior a la de inicio."
                    };
                }

                if (dto.FechaInicio <= DateTime.Now)
                {
                    return new EventoCompletoResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "La fecha de inicio no puede ser posterior a la fecha actual."
                    };
                }

                string slugNuevo = GenerarSlug(dto.TituloEvento);

                var entidad = new Evento
                {
                    IdEvento = idEvento,
                    TituloEvento = dto.TituloEvento,
                    SlugEvento = slugNuevo,
                    DescripcionEvento = dto.DescripcionEvento,
                    DescripcionCorta = dto.DescripcionCorta,
                    FechaInicio = dto.FechaInicio.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    FechaFin = dto.FechaFin.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    ZonaHoraria = dto.ZonaHoraria,
                    EstadoEvento = dto.EstadoEvento,
                    CapacidadEvento = dto.CapacidadEvento,
                    EventoGratuito = dto.EventoGratuito,
                    EventoOnline = dto.EventoOnline,
                    ImagenPortadaUrl = dto.ImagenPortadaUrl,
                    SubcategoriaEventoId = dto.SubcategoriaEventoId,
                    LocalId = dto.LocalId
                };

                string resultado = await _eventoDAO.updateEventoAsync(entidad);

                return new EventoCompletoResponseDTO
                {
                    Exitoso = true,
                    Mensaje = resultado,
                    Evento = new EventoCompletoDTO
                    {
                        IdEvento = idEvento,
                        SlugEvento = slugNuevo
                    }
                };
            }
            catch (Exception ex)
            {
                return new EventoCompletoResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "Ocurrió un error interno: " + ex.Message
                };
            }
        }

        public async Task<EventosCompletosListResponseDTO> ListarEventosCompletosAsync(
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

                return new EventosCompletosListResponseDTO
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
                return new EventosCompletosListResponseDTO
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
                evento.EstadoEventoCliente = "próximo";
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

        private string GenerarSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
                return string.Empty;

            // Convertir a minúsculas y quitar espacios en los extremos
            value = value.ToLowerInvariant().Trim();

            // Normalizar: Esto separa las letras de sus acentos (á -> a + ´)
            string normalizedString = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizedString)
            {
                // Solo conservamos lo que NO sea un acento/marca
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // Volvemos a normalizar y limpiamos con Regex
            string result = sb.ToString().Normalize(NormalizationForm.FormC);

            // Cambiar espacios por guiones y quitar caracteres raros (deja solo letras, números y guiones)
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
            result = Regex.Replace(result, @"\s+", "-").Trim('-');

            // Evitar guiones dobles (--)
            result = Regex.Replace(result, @"-{2,}", "-");

            return result;
        }
    }
}
