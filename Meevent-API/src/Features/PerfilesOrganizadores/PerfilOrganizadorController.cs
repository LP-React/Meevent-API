using Meevent_API.src.Features.PerfilesOrganizadores.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.src.Features.PerfilesOrganizadores;

[Route("api/[controller]")]
[ApiController]
public class PerfilesOrganizadorController : ControllerBase
{
    private readonly IPerfilOrganizadorService _perfilOrganizadorService;

    public PerfilesOrganizadorController(IPerfilOrganizadorService perfilOrganizadorService)
    {
        _perfilOrganizadorService = perfilOrganizadorService;
    }

    [HttpGet("ListarPerfiles")]
    public async Task<ActionResult<PerfilOrganizadorListResponseDTO>> ListarPerfiles()
    {
        var respuesta = await _perfilOrganizadorService.ListarPerfilesOrganizadorAsync();

        if (!respuesta.Exitoso)
            return StatusCode(500, respuesta);

        return Ok(respuesta);
    }

    [HttpGet("BuscarPerfil/{id}")]
    public async Task<ActionResult<PerfilOrganizadorDetalleDTO>> BuscarPerfil(int id)
    {
        var perfil = await _perfilOrganizadorService.ObtenerPerfilOrganizadorPorIdAsync(id);

        if (perfil == null)
            return NotFound(new { Mensaje = "Perfil de organizador no encontrado" });

        return Ok(perfil);
    }

    [HttpPost("CrearPerfil")]
    public async Task<IActionResult> CrearPerfil([FromBody] PerfilOrganizadorCrearDTO perfil)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _perfilOrganizadorService.CrearPerfilOrganizadorAsync(perfil);

        if (resultado.Contains("Error"))
            return StatusCode(500, new { Exitoso = false, Mensaje = resultado });

        return Ok(new { Exitoso = true, Mensaje = resultado });
    }

    [HttpPatch("EditarPerfil/{id}")]
    public async Task<IActionResult> ActualizarPerfilParcial(int id, [FromBody] PerfilOrganizadorEditarDTO perfil)
    {
        if (id <= 0)
            return BadRequest(new { Mensaje = "ID de perfil inválido" });
        if (string.IsNullOrEmpty(perfil.nombre_organizador) &&
            string.IsNullOrEmpty(perfil.descripcion_organizador) &&
            string.IsNullOrEmpty(perfil.sitio_web) &&
            string.IsNullOrEmpty(perfil.facebook_url) &&
            string.IsNullOrEmpty(perfil.instagram_url) &&
            string.IsNullOrEmpty(perfil.tiktok_url))
        {
            return BadRequest(new
            {
                Exitoso = false,
                Mensaje = "Debe proporcionar al menos un campo para actualizar"
            });
        }
        var resultado = await _perfilOrganizadorService.ActualizarPerfilOrganizadorAsync(id, perfil);

        if (resultado.Contains("No se encontró"))
            return NotFound(new { Exitoso = false, Mensaje = resultado });

        if (resultado.Contains("Error"))
            return StatusCode(500, new { Exitoso = false, Mensaje = resultado });

        return Ok(new { Exitoso = true, Mensaje = resultado });
    }
}