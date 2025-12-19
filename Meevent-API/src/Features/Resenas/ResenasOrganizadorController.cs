using Meevent_API.src.Features.Resenas.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.Resenas
{
    [Route("api/organizador")]
    [ApiController]
    public class ResenasOrganizadorController : ControllerBase
    {
        private readonly IResenasOrganizadorService _service;

        public ResenasOrganizadorController(IResenasOrganizadorService service)
        {
            _service = service;
        }

        // GET: api/resenas-organizador/organizador/{id}
        [HttpGet("organizador/{perfilOrganizadorId}")]
        public async Task<IActionResult> GetByOrganizador(int perfilOrganizadorId)
        {
            var result = await _service.GetAllByOrganizadorAsync(perfilOrganizadorId);

            return result.Exitoso
                ? Ok(result)
                : BadRequest(result);
        }

        // GET: api/resenas-organizador/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return result.Exitoso
                ? Ok(result)
                : NotFound(result);
        }

        // POST: api/resenas-organizador
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearResenaOrganizadorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);

            return result.Exitoso
                ? Ok(result)
                : BadRequest(result);
        }

        // PUT: api/resenas-organizador/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Update(int id, int usuarioId, ActualizarResenaOrganizadorDTO dto)
        {
            var result = await _service.UpdateAsync(id, usuarioId, dto);
            return result.Exitoso ? Ok(result) : BadRequest(result);
        }

        // POST: api/resenas-organizador/utilidad/incrementar/{id}
        [HttpPost("utilidad/incrementar/{idResenaOrganizador}")]
        public async Task<IActionResult> IncrementarUtilidad(int idResenaOrganizador)
        {
            var result = await _service.IncrementarUtilidadAsync(idResenaOrganizador);

            return result.Exitoso
                ? Ok(result)
                : BadRequest(result);
        }

        // POST: api/resenas-organizador/utilidad/incrementar/{id}
        [HttpPost("utilidad/descrementar/{idResenaOrganizador}")]
        public async Task<IActionResult> DecrementarUtilidad(int idResenaOrganizador)
        {
            var result = await _service.DecrementarUtilidadAsync(idResenaOrganizador);

            return result.Exitoso
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
