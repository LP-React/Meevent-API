using Meevent_API.src.Features.Ciudades.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.Ciudades
{
    [Route("api/ciudades")]
    [ApiController]
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudadService _ciudadService;

        public CiudadesController(ICiudadService ciudadService)
        {
            _ciudadService = ciudadService;
        }

        [HttpGet("getCiudades")]
        public async Task<ActionResult<CiudadListResponseDTO>> GetByPais(int? idPais = null)
        {
            var response = await _ciudadService.GetCiudadesByPaisAsync(idPais);
            return response.Exitoso ? Ok(response) : StatusCode(500, response);
        }
    }
}
