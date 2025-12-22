
using Meevent_API.src.Features.Paises;
using Meevent_API.src.Features.PerfilesArtistas.DAO;
using Meevent_API.src.Features.PerfilesOrganizadores.DAO;
using Meevent_API.src.Features.Usuarios.DAO;

namespace Meevent_API.src.Features.Usuarios.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioDAO _usuarioDAO;
        private readonly IPerfilArtistaDAO _perfilArtistaDAO;
        private readonly IPerfilOrganizadorDAO _perfilOrganizadorDAO;

        public UsuarioService(
            IUsuarioDAO usuarioDAO,
            IPerfilArtistaDAO perfilArtistaDAO,
            IPerfilOrganizadorDAO perfilOrganizadorDAO)
        {
            _usuarioDAO = usuarioDAO;
            _perfilArtistaDAO = perfilArtistaDAO;
            _perfilOrganizadorDAO = perfilOrganizadorDAO;
        }

        public async Task<UsuariosListaResponseDTO> ListarUsuariosAsync()
        {
            var respuesta = new UsuariosListaResponseDTO();
            try
            {
                var lista = await _usuarioDAO.GetUsuarios();

                respuesta.Exitoso = true;
                respuesta.Mensaje = lista.Any() ? "Usuarios recuperados correctamente" : "No hay usuarios registrados";
                respuesta.Usuarios = lista;
            }
            catch (Exception ex)
            {
                respuesta.Exitoso = false;
                respuesta.Mensaje = "Error al obtener la lista: " + ex.Message;
                respuesta.Usuarios = null;
            }

            return respuesta;
        }

        public async Task<UsuarioDetalleResponseDTO> ObtenerUsuarioPorIdAsync(int id_usuario)
        {
            var respuesta = new UsuarioDetalleResponseDTO();

            try
            {
                var usuario = await _usuarioDAO.GetUsuariosPorId(id_usuario);

                if (usuario == null)
                {
                    respuesta.Exitoso = false;
                    respuesta.Mensaje = $"No se encontró un usuario con el ID {id_usuario}";
                    respuesta.Usuario = null;
                }
                else
                {
                    respuesta.Exitoso = true;
                    respuesta.Mensaje = "Usuario encontrado.";
                    respuesta.Usuario = usuario;
                }
            }
            catch (Exception ex)
            {
                respuesta.Exitoso = false;
                respuesta.Mensaje = "Error: " + ex.Message;
            }

            return respuesta;
        }

        public async Task<UsuarioDetalleDTO> ObtenerUsuarioPorCorreoAsync(string correo_electronico)
        {
            if (string.IsNullOrWhiteSpace(correo_electronico))
                return null;

            var usuario = await Task.Run(() => _usuarioDAO.GetUsuariosPorCorreo(correo_electronico));

            return usuario;
        }

        public async Task<UsuarioDetalleResponseDTO> RegistrarUsuarioAsync(UsuarioRegistroDTO reg)
        {
            var respuesta = new UsuarioDetalleResponseDTO();

            try
            {
                // Verificar si el correo ya está registrado
                bool yaExiste = await _usuarioDAO.VerificarCorreoExistenteAsync(reg.correo_electronico);
                if (yaExiste)
                {
                    respuesta.Exitoso = false;
                    respuesta.Mensaje = "El correo electrónico ya está registrado.";
                    return respuesta;
                }

                bool ciudadExiste = await _usuarioDAO.VerificarCiudadExisteAsync(reg.id_ciudad);
                if (!ciudadExiste)
                {
                    respuesta.Exitoso = ciudadExiste;
                    respuesta.Mensaje = "La ciudad que ingreso no se encuentra disponible.";
                    return respuesta;
                }


                // Hash de la contraseña antes de guardarla
                reg.contrasenia = BCrypt.Net.BCrypt.HashPassword(reg.contrasenia);

                // Insertar el nuevo usuario
                string resultadoDAO = await _usuarioDAO.InsertUsuario(reg);

                if (resultadoDAO == "OK")
                {
                    // Obtener el usuario recién creado para devolverlo en la respuesta
                    var usuarioCreado = await _usuarioDAO.GetUsuariosPorCorreo(reg.correo_electronico);

                    respuesta.Exitoso = true;
                    respuesta.Mensaje = "¡Registro exitoso!";
                    respuesta.Usuario = usuarioCreado;
                }
                else
                {
                    respuesta.Exitoso = false;
                    respuesta.Mensaje = resultadoDAO;
                }
            }
            catch (Exception ex)
            {
                respuesta.Exitoso = false;
                respuesta.Mensaje = "Error crítico en el Service: " + ex.Message;
            }

            return respuesta;
        }

        public async Task<LoginResponseDTOE> LoginAsync(LoginDTO login)
        {
            var response = new LoginResponseDTOE();

            try
            {
                var usuario = await _usuarioDAO.ObtenerUsuarioLogin(login.correo_electronico);

                // Existe el correo en la base de datos?
                if (usuario == null)
                {
                    response.Exitoso = false;
                    response.Mensaje = "El correo electrónico o contraseña es incorrecta.";
                    return response;
                }

                // Verificamos si la cuenta está activa
                if (!usuario.cuenta_activa)
                {
                    response.Exitoso = false;
                    response.Mensaje = "Tu cuenta está deshabilitada. Por favor, contacta al correo : ayuda@meevent.com";
                    return response;
                }

                // Verificamos si la contraseña ingresada coincide con el Hash de la BD
                bool esValida = BCrypt.Net.BCrypt.Verify(login.contrasenia, usuario.contrasena_hash);

                if (!esValida)
                {
                    response.Exitoso = false;
                    response.Mensaje = "Correo o Contraseña incorrecta. Inténtelo de nuevo.";
                    return response;
                }

                // Si llegamos aquí, las credenciales son correctas
                response.Exitoso = true;
                response.Mensaje = $"Bienvenido, {usuario.nombre_completo}";
                response.Usuario = usuario;
            }
            catch (Exception ex)
            {
                response.Exitoso = false;
                response.Mensaje = "Ocurrió un error inesperado durante el inicio de sesión.";
            }

            return response;
        }

        public async Task<UpdateResponseDTO> ActualizarPerfilAsync(UsuarioUpdateDTO dto)
        {
            var response = new UpdateResponseDTO();

            if (dto.id_ciudad.HasValue)
            {
                bool existeCiudad = await _usuarioDAO.VerificarCiudadExisteAsync(dto.id_ciudad.Value);
                if (!existeCiudad)
                {
                    response.Exitoso = false;
                    response.Mensaje = "La ciudad especificada no es válida.";
                    return response;
                }
            }

            string resultado = await _usuarioDAO.ActualizarUsuarioAsync(dto);

            if (resultado == "OK")
            {
                var usuario = await _usuarioDAO.GetUsuariosPorId(dto.id_usuario);

                response.Exitoso = true;
                response.Mensaje = "Perfil actualizado correctamente.";
                response.UsuarioActualizado = usuario;
            }
            else
            {
                response.Exitoso = false;
                response.Mensaje = resultado;
            }

            return response;
        }

        public async Task<bool> VerificarCorreoExistenteAsync(string correo)
        {
            try
            {
                return await _usuarioDAO.VerificarCorreoExistenteAsync(correo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la disponibilidad del correo electrónico.", ex);
            }
        }

        public async Task<bool> VerificarPaisExisteAsync(int id)
            => await Task.Run(() => _usuarioDAO.VerificarPaisExiste(id));
        public async Task<bool> CiudadExisteAsync(int id_ciudad)
        {
            try
            {
                return await _usuarioDAO.VerificarCiudadExisteAsync(id_ciudad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la ciudad.", ex);
            }
        }

        public async Task<UsuarioActivarCuentaResponseDTO> ActivarDesactivarCuentaAsync(int id, bool estado)
        {
            try
            {
                string res = await Task.Run(() => _usuarioDAO.ActivarDesactivarCuenta(id, estado));
                return new UsuarioActivarCuentaResponseDTO
                {
                    Exitoso = res.Contains("exitosa"),
                    Mensaje = res,
                    CuentaActiva = res.Contains("exitosa") ? estado : !estado
                };
            }
            catch (Exception ex)
            {
                return new UsuarioActivarCuentaResponseDTO { Exitoso = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<bool> CambiarContraseniaAsync(int id_usuario, UsuarioCambiarPasswordDTO dto)
        {
            try
            {
                return await _usuarioDAO.CambiarContraseniaAsync(id_usuario, dto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña.", ex);
            }
        }
    }
}