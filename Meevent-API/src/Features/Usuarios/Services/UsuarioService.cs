using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Usuarios.DAO;

namespace Meevent_API.src.Features.Usuarios.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioDAO _usuarioDAO;

        public UsuarioService(IUsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public async Task<UsuarioListResponseDTO> ObtenerUsuariosAsync()
        {
            try
            {
                var usuarios = await Task.Run(() => _usuarioDAO.GetUsuarios());

                var usuariosDTO = usuarios.Select(u => new UsuarioDTO
                {
                    id_usuario = u.IdUsuario,
                    nombre_completo = u.NombreCompleto,
                    correo_electronico = u.CorreoElectronico,
                    numero_telefono = u.NumeroTelefono,
                    imagen_perfil_url = u.ImagenPerfilUrl,
                    fecha_nacimiento = u.FechaNacimiento,
                    tipo_usuario = u.TipoUsuario
                }).ToList();

                return new UsuarioListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Usuarios obtenidos exitosamente",
                    TotalUsuarios = usuariosDTO.Count,
                    Usuarios = usuariosDTO
                };
            }
            catch (Exception ex)
            {
                return new UsuarioListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener usuarios: {ex.Message}",
                    TotalUsuarios = 0,
                    Usuarios = Enumerable.Empty<UsuarioDTO>()
                };
            }
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int id_usuario)
        {
            try
            {
                var usuarios = await Task.Run(() => _usuarioDAO.GetUsuariosPorId(id_usuario));
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                    return null;

                return new UsuarioDTO
                {
                    id_usuario = usuario.IdUsuario,
                    nombre_completo = usuario.NombreCompleto,
                    correo_electronico = usuario.CorreoElectronico,
                    numero_telefono = usuario.NumeroTelefono,
                    imagen_perfil_url = usuario.ImagenPerfilUrl,
                    fecha_nacimiento = usuario.FechaNacimiento,
                    tipo_usuario = usuario.TipoUsuario
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorCorreoAsync(string correo_electronico)
        {
            try
            {
                var usuarios = await Task.Run(() => _usuarioDAO.GetUsuariosPorCorreo(correo_electronico));
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                    return null;

                return new UsuarioDTO
                {
                    id_usuario = usuario.IdUsuario,
                    nombre_completo = usuario.NombreCompleto,
                    correo_electronico = usuario.CorreoElectronico,
                    numero_telefono = usuario.NumeroTelefono,
                    imagen_perfil_url = usuario.ImagenPerfilUrl,
                    fecha_nacimiento = usuario.FechaNacimiento,
                    tipo_usuario = usuario.TipoUsuario
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> RegistrarUsuarioAsync(UsuarioRegistroDTO registro)
        {
            try
            {
                return await Task.Run(() => _usuarioDAO.InsertUsuario(registro));
            }
            catch (Exception ex)
            {
                return $"Error al registrar usuario: {ex.Message}";
            }
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO login)
        {
            try
            {
                var token = await Task.Run(() => _usuarioDAO.LoginUsuario(login));

                if (token.Contains("incorrecto"))
                {
                    return new LoginResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = token,
                        Token = null,
                        Usuario = null
                    };
                }

   
                var usuario = await ObtenerUsuarioPorCorreoAsync(login.correo_electronico);
                    
                return new LoginResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Login exitoso",
                    Token = token,
                    Usuario = usuario
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error en login: {ex.Message}",
                    Token = null,
                    Usuario = null
                };
            }
        }

        public async Task<UsuarioEditarResponseDTO> ActualizarUsuarioAsync(UsuarioEditarDTO usuario)
        {
            try
            {
                var usuarioExistente = await ObtenerUsuarioPorIdAsync(usuario.id_usuario);

                if (usuarioExistente == null)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Usuario no encontrado",
                        UsuarioActualizado = null
                    };
                }

                var resultado = await Task.Run(() => _usuarioDAO.ActualizarUsuario(usuario));

                if (resultado.Contains("correctamente"))
                {
                    var usuarioActualizado = await ObtenerUsuarioPorIdAsync(usuario.id_usuario);

                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = " Usuario actualizado correctamente",
                        UsuarioActualizado = usuarioActualizado
                    };
                }
                else
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = $"no se actualizo {resultado}",
                        UsuarioActualizado = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new UsuarioEditarResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $" Error al actualizar usuario: {ex.Message}",
                    UsuarioActualizado = null
                };
            }
        }
    }
}