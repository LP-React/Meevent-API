using Meevent_API.src.Features.SeguidoresEvento.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.SeguidoresEvento
{
    [Route("api/seguidores-evento")]
    [ApiController]
    public class SeguidoresEventoController : ControllerBase
    {
        private readonly ISeguidoresService _seguimientoService;
        private readonly ILogger<SeguidoresEventoController> _logger;

        public SeguidoresEventoController(ISeguidoresService seguimientoService, ILogger<SeguidoresEventoController> logger)
        {
            _seguimientoService = seguimientoService;
            _logger = logger;
        }

        [HttpGet("getEventosSeguidos")]
        public async Task<ActionResult<EventoSeguidoListResponseDTO>> GetEventosSeguidos(int idUsuario)
        {
            _logger.LogInformation("Cargando eventos seguidos para el usuario ID: {Id}", idUsuario);

            var response = await _seguimientoService.GetEventosSeguidosPorUsuarioAsync(idUsuario);

            if (!response.Exitoso)
            {
                return StatusCode(500, response);
            }

            if (response.TotalResultados == 0)
            {
                return Ok(response);
            }

            return Ok(response);
        }

        [HttpPost("seguir-evento")]
        public async Task<ActionResult<SeguimientoResponseDTO>> PostSeguimiento(int idUsuario,int idEvento)
        {

            var resultado = await _seguimientoService.SeguirEventoAsync(idUsuario, idEvento);

            if (!resultado.Exitoso)
            {
                return BadRequest(resultado);
            }
            return CreatedAtAction(nameof(PostSeguimiento), new { id = resultado.Evento?.IdSeguidorEvento }, resultado);
        }

        [HttpDelete("dejar-de-seguir")]
        public async Task<ActionResult<BaseResponseDTO>> DeleteSeguimiento([FromQuery] int usuarioId, [FromQuery] int eventoId)
        {
            var resultado = await _seguimientoService.DejarDeSeguirEventoAsync(usuarioId, eventoId);

            if (!resultado.Exitoso)
            {
                return NotFound(resultado); // 404 si no existía el seguimiento
            }

            return Ok(resultado);
        }
    }
}
