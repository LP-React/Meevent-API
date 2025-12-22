using Meevent_API.src.Features.Usuarios.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> GetAllUsuariosAsync()
        {
            var resultado = await _usuarioService.ListarUsuariosAsync();

            if (!resultado.Exitoso)
            {
                return StatusCode(500, resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> GetByIdUsuario(int id)
        {
            var resultado = await _usuarioService.ObtenerUsuarioPorIdAsync(id);

            if (!resultado.Exitoso)
            {
                return NotFound(resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("buscar-por-correo/{correo}")]
        public async Task<IActionResult> GetPorCorreo(string correo)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorCorreoAsync(correo);

            if (usuario == null)
            {
                return NotFound(new { 
                        Mensaje = "El usuario no existe." 
                    });
            }

            return Ok(usuario);
        }

        [HttpPost("registrarUsuario")]
        public async Task<IActionResult> Registrar(UsuarioRegistroDTO registro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _usuarioService.RegistrarUsuarioAsync(registro);

            if (resultado.Exitoso)
            {
                return Ok(resultado);
            }

            return BadRequest(resultado);
        }


        [HttpPost("loginUsuario")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { exitoso = false, mensaje = "Datos de entrada inválidos" });
            }

            var resultado = await _usuarioService.LoginAsync(login);

            if (resultado.Exitoso)
            {
                return Ok(resultado);
            }
            else
            {
                return Unauthorized(resultado);
            }
        }

        [HttpPatch("editarUsuario/")]
        public async Task<IActionResult> ActualizarPerfil(int id, UsuarioUpdateDTO dto)
        {
            dto.id_usuario = id;

            if (dto.id_usuario <= 0)
            {
                return BadRequest(new { exitoso = false, mensaje = "ID de usuario no válido." });
            }

            var resultado = await _usuarioService.ActualizarPerfilAsync(dto);

            if (resultado.Exitoso)
            {
                return Ok(resultado);
            }

            return BadRequest(resultado);
        }

        [HttpGet("verificarEmail/{correo}")]
        public async Task<ActionResult<bool>> VerificarEmail(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return BadRequest("El correo electrónico es requerido");

            if (!new EmailAddressAttribute().IsValid(correo))
                return BadRequest("Formato de correo inválido");

            try
            {
                bool existe = await _usuarioService.VerificarCorreoExistenteAsync(correo);
                return Ok(existe);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar correo: {ex.Message}");
            }
        }

        [HttpPatch("activarCuenta/{id}")]
        public async Task<IActionResult> ActivarCuenta(int id, [FromBody] UsuarioActivarCuentaDTO estado)
        {
            if (id <= 0)
                return BadRequest(new { Mensaje = "ID de usuario inválido" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _usuarioService.ActivarDesactivarCuentaAsync(id, estado.cuenta_activa);

            if (respuesta.Exitoso)
            {
                return Ok(respuesta);
            }
            else
            {
                if (respuesta.Mensaje.Contains("no encontrado"))
                    return NotFound(respuesta);

                return BadRequest(respuesta);
            }
        }
    
        [HttpPost("cambiar-contrasenia/{id}")]
        public async Task<IActionResult> CambiarContrasenia(int id, UsuarioCambiarPasswordDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { Mensaje = "ID de usuario inválido" });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var exito = await _usuarioService.CambiarContraseniaAsync(id, dto);
            if (exito)
            {
                return Ok(new { Mensaje = "Contraseña cambiada exitosamente." });
            }
            else
            {
                return BadRequest(new { Mensaje = "Error al cambiar la contraseña." });
            }
        }
    }
}