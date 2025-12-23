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
    }
}
