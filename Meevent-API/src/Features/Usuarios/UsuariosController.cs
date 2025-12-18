using Meevent_API.src.Features.Usuarios.Meevent_API.src.Features.Usuarios;
using Meevent_API.src.Features.Usuarios.Service;
using Microsoft.AspNetCore.Mvc;

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

            if (!string.IsNullOrEmpty(registro.tipo_usuario) &&
                !new[] { "normal", "artista", "organizador" }.Contains(registro.tipo_usuario.ToLower()))
            {
                return BadRequest(new
                {
                    Exitoso = false,
                    Mensaje = "Tipo de usuario debe ser: normal, artista u organizador"
                });
            }

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

        [HttpPatch("editarUsuario/{id}")]
        public async Task<ActionResult<UsuarioEditarResponseDTO>> EditarUsuario(int id, [FromBody] UsuarioEditarDTO usuario)
        {
            if (id <= 0)
                return BadRequest(new { Mensaje = "ID de usuario inválido" });

            if (string.IsNullOrEmpty(usuario.nombre_completo) &&
                string.IsNullOrEmpty(usuario.numero_telefono) &&
                string.IsNullOrEmpty(usuario.imagen_perfil_url) &&
                !usuario.fecha_nacimiento.HasValue &&
                string.IsNullOrEmpty(usuario.tipo_usuario) &&
                !usuario.email_verificado.HasValue &&
                !usuario.cuenta_activa.HasValue &&
                string.IsNullOrEmpty(usuario.contrasena))
            {
                return BadRequest(new
                {
                    Exitoso = false,
                    Mensaje = "Debe proporcionar al menos un campo para actualizar"
                });
            }
            if (!string.IsNullOrEmpty(usuario.contrasena))
            {
                if (usuario.contrasena.Length < 8)
                {
                    return BadRequest(new
                    {
                        Exitoso = false,
                        Mensaje = "La contraseña debe tener al menos 8 caracteres"
                    });
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(usuario.contrasena, @"^(?=.*[A-Z])(?=.*\d).+$"))
                {
                    return BadRequest(new
                    {
                        Exitoso = false,
                        Mensaje = "La contraseña debe tener al menos 1 mayúscula y 1 número"
                    });
                }
            }

            var respuesta = await _usuarioService.ActualizarUsuarioAsync(id, usuario);

            if (!respuesta.Exitoso)
            {
                if (respuesta.Mensaje.Contains("no encontrado"))
                    return NotFound(respuesta);

                if (respuesta.Mensaje.Contains("inválido"))
                    return BadRequest(respuesta);

                return StatusCode(500, respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("verificarEmail")]
        public async Task<ActionResult<VerificarEmailResponseDTO>> VerificarEmail([FromBody] VerificarEmailDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _usuarioService.VerificarCorreoExistenteAsync(request.correo_electronico);

            if (!respuesta.Exitoso)
                return StatusCode(500, respuesta);

            return Ok(respuesta);
        }

    }
}