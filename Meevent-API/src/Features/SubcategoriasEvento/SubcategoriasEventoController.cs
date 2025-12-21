using Meevent_API.src.Features.SubcategoriasEvento.Services;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.SubcategoriasEvento
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoriasEventoController : ControllerBase
    {
        private readonly ISubcategoriaEventoService _service;

        public SubcategoriasEventoController(ISubcategoriaEventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
            => Ok(_service.GetSubcategoriasEvento());

        [HttpGet("categoria/{categoriaId}")]
        public IActionResult GetPorCategoria(int categoriaId)
            => Ok(_service.GetSubcategoriasPorCategoria(categoriaId));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
            => Ok(_service.GetSubcategoriaEventoPorId(id));

        [HttpPost]
        public IActionResult Post([FromBody] SubcategoriaEventoDTO dto)
            => Ok(_service.CrearSubcategoriaEvento(dto));

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SubcategoriaEventoDTO dto)
            => Ok(_service.ActualizarSubcategoriaEvento(id, dto));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
            => Ok(_service.EliminarSubcategoriaEvento(id));
    }
}
