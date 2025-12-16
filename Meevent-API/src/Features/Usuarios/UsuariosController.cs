using Microsoft.AspNetCore.Mvc;
using Meevent_API.src.Features.Usuarios.Service;

namespace Meevent_API.src.Features.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult<UsuarioListResponseDTO>> Get()
        {
            var respuesta = await _usuarioService.ObtenerUsuariosAsync();

            if (!respuesta.Exitoso)
                return StatusCode(500, respuesta);

            return Ok(respuesta);
        }

        [HttpGet("Buscar{id}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { Mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }

        [HttpGet("Buscar-Por-correo/{correo}")]
        public async Task<ActionResult<UsuarioDTO>> GetPorCorreo(string correo)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorCorreoAsync(correo);

            if (usuario == null)
                return NotFound(new { Mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }

        [HttpPost("registrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO registro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _usuarioService.RegistrarUsuarioAsync(registro);

            if (resultado.Contains("ya está registrado"))
                return BadRequest(new { Exitoso = false, Mensaje = resultado });

            if (resultado.Contains("Error"))
                return StatusCode(500, new { Exitoso = false, Mensaje = resultado });

            return Ok(new { Exitoso = true, Mensaje = resultado });
        }

        [HttpPost("loginUsuario")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _usuarioService.LoginAsync(login);

            if (!respuesta.Exitoso)
                return Unauthorized(respuesta);

            return Ok(respuesta);
        }

        [HttpPut("editarUsuario")]
        public async Task<ActionResult<UsuarioEditarResponseDTO>> EditarUsuario([FromBody] UsuarioEditarDTO usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (usuario.id_usuario <= 0)
                return BadRequest(new { Mensaje = "ID de usuario inválido" });

            var respuesta = await _usuarioService.ActualizarUsuarioAsync(usuario);

            if (!respuesta.Exitoso)
            {
                if (respuesta.Mensaje.Contains("no encontrado"))
                    return NotFound(respuesta);

                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }


    }
}