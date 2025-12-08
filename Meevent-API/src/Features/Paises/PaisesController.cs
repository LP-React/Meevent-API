using Meevent_API.src.Features.Paises.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.Paises
{

    [Route("api/paises")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly IPaisService _paisService;

        public PaisesController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        [HttpGet]
        public async Task<ActionResult<PaisListResponseDTO>> GetPaises()
        {
            var resultado = await _paisService.GetAllPaisesAsync();

            if (!resultado.Exitoso)
            {
                return StatusCode(500, resultado);
            }

            return Ok(resultado);
        }
    }
}
