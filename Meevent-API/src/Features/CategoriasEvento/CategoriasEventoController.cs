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

     
        [HttpGet("ListarCategorias")]
        public async Task<IActionResult> Get()
        {
            var response = await _service.ObtenerCategoriasAsync();
            return Ok(response);
        }

     
        [HttpGet("BuscarCategoria/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var categoria = await _service.ObtenerCategoriaPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    Exitoso = false,
                    Mensaje = "Categoría no encontrada"
                });
            }

            return Ok(new CategoriaEventoOperacionResponseDTO
            {
                Exitoso = true,
                Mensaje = "Categoría encontrada",
                Categoria = new CategoriaEventoDetalleDTO
                {
                    IdCategoriaEvento = categoria.IdCategoriaEvento,
                    NombreCategoria = categoria.NombreCategoria,
                    SlugCategoria = categoria.SlugCategoria,
                    Estado = categoria.Estado
                }
            });
        }

        [HttpPost("RegistrarCategoria")]
        public async Task<IActionResult> Post([FromBody] CategoriaEventoCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mensaje = await _service.RegistrarCategoriaAsync(dto);

            return Ok(new
            {
                Exitoso = true,
                Mensaje = mensaje
            });
        }

   
        [HttpPatch("EditarCategoria/{id}")]
        public async Task<IActionResult> Patch(
            int id,
            [FromBody] CategoriaEventoEditarDTO dto)
        {
            var response = await _service.ActualizarCategoriaAsync(id, dto);

            if (!response.Exitoso)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPatch("ActivarEstado_Desactivar/{id}")]
        public async Task<IActionResult> PatchEstado(
            int id,
            [FromBody] CategoriaCambiarEstadoDTO dto)
        {
            var response = await _service.ActivarDesactivarCategoriaAsync(id, dto.Estado);

            if (!response.Exitoso)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
