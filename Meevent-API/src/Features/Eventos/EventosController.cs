using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Eventos.DAO;

namespace Meevent_API.src.Features.Eventos
{
    [Route("api/eventos")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        [HttpGet("getEventos")] public async Task<ActionResult<List<Evento>>> GetEventos()
        {
            var lista=await Task.Run(() => new EventoDAO().GetEventos());
            return Ok(lista);
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
    }
}
