using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Eventos.DAO;
using Meevent_API.src.Features.Eventos.Services;
using Meevent_API.src.Features.Paises.Services.Interfaces;
using Meevent_API.src.Features.Paises.Services;

namespace Meevent_API.src.Features.Eventos
{
    [Route("api/eventos")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }
        
        [HttpPost("insertEvento")]
        public async Task<ActionResult<EventoCompletoResponseDTO>> InsertEvento(EventoCrearDTO dto)
        {
            var response = await _eventoService.InsertEventoAsync(dto);

            if (!response.Exitoso)
                return BadRequest(response);

            return Ok(response);
        }

        // UPDATE
        [HttpPut("updateEvento/{id}")]
        public async Task<ActionResult<EventoCompletoResponseDTO>> UpdateEvento(int id, EventoActualizarDTO dto)
        {
            var response = await _eventoService.UpdateEventoAsync(id, dto);

            if (!response.Exitoso)
                return NotFound(response);

            return Ok(response);
        }

        // GET BY ID
        [HttpGet("getEvento/{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);

            if (evento == null)
                return NotFound("Evento no encontrado");

            return Ok(evento);
        }

        [HttpGet("getslug/{slug}")]
        public async Task<ActionResult<EventoCompletoResponseDTO>> GetBySlug(string slug)
        {
            var response = await _eventoService.GetEventoPorSlugAsync(slug);

            if (!response.Exitoso)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("listarEventosCompletos")]
        public async Task<ActionResult<EventosCompletosListResponseDTO>> ListarEventos(
            int? idPerfilOrganizador,
            int? idSubCategoria,
            int? idLocal,
            bool? eventoGratuito,
            bool? eventoOnline,
            string? estadoEvento,
            string? fchDesde,
            string? fchHasta)
        {
            var resultado = await _eventoService.ListarEventosCompletosAsync(
                idPerfilOrganizador,
                idSubCategoria, 
                idLocal, 
                eventoGratuito,
                eventoOnline,
                estadoEvento,
                fchDesde,
                fchHasta);

            if (!resultado.Exitoso)
            {
                return StatusCode(500, resultado);
            }

            return Ok(resultado);
        }

    }
}
