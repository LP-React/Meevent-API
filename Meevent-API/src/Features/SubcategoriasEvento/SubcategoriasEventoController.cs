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

        [HttpGet("ListarSubCategorias")]
        public async Task<IActionResult> Get()
        {
            var response = await _service.ObtenerSubcategoriasAsync();
            return Ok(response);
        }

        [HttpGet("BuscarSubCategorias/{id}")]
        public async Task<ActionResult<SubcategoriaEventoOperacionResponseDTO>> Get(int id)
        {
            var subcategoria = await _service.ObtenerSubcategoriaPorIdAsync(id);

            if (subcategoria == null)
            {
                return NotFound(new { Exitoso = false, Mensaje = "Subcategoría no encontrada" });
            }

            return Ok(new SubcategoriaEventoOperacionResponseDTO
            {
                Exitoso = true,
                Mensaje = "Subcategoría encontrada",
                Subcategoria = new SubcategoriaEventoDetalleDTO
                {
                    IdSubcategoriaEvento = subcategoria.IdSubcategoriaEvento,
                    NombreSubcategoria = subcategoria.NombreSubcategoria,
                    SlugSubcategoria = subcategoria.SlugSubcategoria,
                    CategoriaEventoId = subcategoria.CategoriaEventoId,
                    Estado = subcategoria.Estado
                }
            });
        }

        [HttpPost("RegistrarSubCategoria")]
        public async Task<IActionResult> Post([FromBody] SubcategoriaEventoCrearDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mensaje = await _service.RegistrarSubcategoriaAsync(dto);
            return Ok(new { Exitoso = true, Mensaje = mensaje });
        }

        [HttpPatch("EditarSubCategoria/{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] SubcategoriaEventoEditarDTO dto)
        {
            var response = await _service.ActualizarSubcategoriaAsync(id, dto);

            if (!response.Exitoso) return NotFound(response);

            return Ok(response);
        }

        [HttpPatch("ActivarEstado_Desctivar/{id}")]
        public async Task<IActionResult> PatchEstado(int id, [FromBody] SubcategoriaCambiarEstadoDTO dto)
        {
            var response = await _service.ActivarDesactivarSubcategoriaAsync(id, dto.Estado);

            if (!response.Exitoso) return BadRequest(response);

            return Ok(response);
        }
    }
}