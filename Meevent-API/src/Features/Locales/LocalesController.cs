using Meevent_API.src.Features.Locales.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.Locales
{
    [Route("api/locales")]
    [ApiController]
    public class LocalesController : ControllerBase
    {
        private readonly ILocalService _localService;
        private readonly ILogger<LocalesController> _logger;

        public LocalesController(ILocalService localService, ILogger<LocalesController> logger)
        {
            _localService = localService;
            _logger = logger;
        }

        [HttpGet("getLocales")]
        public async Task<ActionResult<LocalListResponseDTO>> GetLocalesByCiudad(int idCiudad)
        {
            _logger.LogInformation("Consultando locales para la ciudad con ID: {Id}", idCiudad);

            var response = await _localService.GetLocalesByCiudadAsync(idCiudad);

            if (!response.Exitoso)
            {
                // Si hubo un error interno en el servidor
                return StatusCode(500, response);
            }

            if (response.TotalLocales == 0)
            {
                // Si la consulta fue exitosa pero no trajo resultados
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
