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
        public async Task<ActionResult<UsuarioListResponseDTO>> GetAllUsuariosAsync()
        {
            var resultado = await _usuarioService.ListarUsuariosAsync();

            if (!resultado.Exitoso)
            {
                return StatusCode(500, resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<UsuarioDetalleResponseDTO>> GetByIdUsuario(int id)
        {
            var resultado = await _usuarioService.ObtenerUsuarioPorIdAsync(id);

            if (!resultado.Exitoso)
            {
                return NotFound(resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("buscar-por-correo/{correo}")]
        public async Task<ActionResult<UsuarioDetalleDTO>> GetPorCorreo(string correo)
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

        /*[HttpPost("registrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO registro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _usuarioService.RegistrarUsuarioAsync(registro);

            if (resultado.Contains("correctamente"))
            {
                return Ok(new
                {
                    Exitoso = true,
                    Mensaje = resultado
                });
            }

            return BadRequest(new
            {
                Exitoso = false,
                Mensaje = resultado
            });
        }
        */
        
        /*[HttpPost("loginUsuario")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _usuarioService.LoginAsync(login);

            if (!respuesta.Exitoso)
                return Unauthorized(respuesta);

            return Ok(respuesta);
        }*/

        /*[HttpPatch("editarUsuario/{id}")]
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
                string.IsNullOrEmpty(usuario.contrasena) &&
                !usuario.id_pais.HasValue)
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

                if (respuesta.Mensaje.Contains("inválido") || respuesta.Mensaje.Contains("no existe"))
                    return BadRequest(respuesta);

                return StatusCode(500, respuesta);
            }

            return Ok(respuesta);
        }
        */

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

        [HttpGet("verificarPais/{idPais}")]
        public async Task<ActionResult<bool>> VerificarPais(int idPais)
        {
            if (idPais <= 0)
                return BadRequest("El ID de país debe ser un número positivo");

            try
            {
                bool existe = await _usuarioService.VerificarPaisExisteAsync(idPais);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar país: {ex.Message}");
            }
        }

        [HttpGet("verificarCiudad/{idCiudad}")]
        public async Task<ActionResult<bool>> VerificarCiudad(int idCiudad)
        {
            if (idCiudad <= 0)
                return BadRequest("El ID de ciudad debe ser un número positivo");
            try
            {
                bool existe = await _usuarioService.VerificarCiudadExisteAsync(idCiudad);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar ciudad: {ex.Message}");
            }
        }

        /*[HttpPatch("activarCuenta/{id}")]
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
        */
    }
}