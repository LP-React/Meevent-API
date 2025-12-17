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
        private readonly IEventoService _eventoDAO;

        public EventosController(IEventoService eventoService)
        {
            _eventoDAO = eventoService;
        }

        [HttpGet("getEventos")] public async Task<ActionResult<EventoListResponseDTO>> GetEventos()
        {
            var resultado = await _eventoDAO.GetAllEventosAsync();

            if (!resultado.Exitoso)
            {
                return StatusCode(500, resultado);
            }

            return Ok(resultado);
        }
        [HttpPost("insertEvento")] public async Task<ActionResult<string>> insertEvento(Evento reg)
        {
            var mensaje = await Task.Run(() => new EventoDAO().insertEvento(reg));
            return Ok(mensaje);
        }
        [HttpPut("updateEvento")] public async Task<ActionResult<string>> updateEvento(Evento reg)
        {
            var mensaje = await Task.Run(() => new EventoDAO().updateEvento(reg));
            return Ok(mensaje);
        }
        [HttpGet("getEvento/{id}")] public async Task<ActionResult<List<Evento>>> getEvento(int id)
        {
            var lista = await Task.Run(() => new EventoDAO().GetEvento(id));
            return Ok(lista);
        }
        [HttpGet("getslug/{slug}")]
        public IActionResult GetBySlug(string slug)
        {
            var evento = _eventoDAO.GetEventoPorSlug(slug);

            if (evento == null)
                return NotFound();

            return Ok(evento);
        }

    }
}
