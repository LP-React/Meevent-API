using Meevent_API.src.Features.CategoriasEvento.Services;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.CategoriasEvento
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasEventoController : ControllerBase
    {
        private readonly ICategoriaEventoService _service;

        public CategoriasEventoController(ICategoriaEventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _service.GetCategoriasEvento();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = _service.GetCategoriaEventoPorId(id);
            return Ok(data);
        }

        
        [HttpPost]
        public IActionResult Post([FromBody] CategoriaEventoDTO dto)
        {
            var mensaje = _service.CrearCategoriaEvento(dto);
            return Ok(mensaje);
        }

        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoriaEventoDTO dto)
        {
            var mensaje = _service.ActualizarCategoriaEvento(id, dto);
            return Ok(mensaje);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var mensaje = _service.EliminarCategoriaEvento(id);
            return Ok(mensaje);
        }
    }
}
