using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Usuarios.DAO;
using Meevent_API.src.Features.Usuarios.Meevent_API.src.Features.Usuarios;

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
                    tipo_usuario = u.TipoUsuario,
                    id_pais = u.IdPais,
                    cuenta_activa = u.CuentaActiva 
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
                    tipo_usuario = usuario.TipoUsuario,
                    id_pais = usuario.IdPais,
                    cuenta_activa = usuario.CuentaActiva  
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
                    tipo_usuario = usuario.TipoUsuario,
                    id_pais = usuario.IdPais,
                    cuenta_activa = usuario.CuentaActiva  
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
                if (!string.IsNullOrEmpty(registro.tipo_usuario) &&
                    !new[] { "normal", "artista", "organizador" }.Contains(registro.tipo_usuario.ToLower()))
                {
                    return "Tipo de usuario inválido. Debe ser: normal, artista u organizador";
                }

                bool paisExiste = await VerificarPaisExisteAsync(registro.id_pais);
                if (!paisExiste)
                {
                    return $"El país con ID {registro.id_pais} no existe";
                }

                bool correoExiste = await VerificarCorreoExistenteAsync(registro.correo_electronico);
                if (correoExiste)
                {
                    return "El correo electrónico ya está registrado";
                }

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
        public async Task<UsuarioEditarResponseDTO> ActualizarUsuarioAsync(int id_usuario, UsuarioEditarDTO usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.nombre_completo) &&
                    string.IsNullOrEmpty(usuario.numero_telefono) &&
                    string.IsNullOrEmpty(usuario.imagen_perfil_url) &&
                    !usuario.fecha_nacimiento.HasValue &&
                    string.IsNullOrEmpty(usuario.tipo_usuario) &&
                    !usuario.email_verificado.HasValue &&
                    string.IsNullOrEmpty(usuario.contrasena) &&
                    !usuario.id_pais.HasValue)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Debe proporcionar al menos un campo para actualizar",
                        UsuarioActualizado = null
                    };
                }

                var usuarioExistente = await ObtenerUsuarioPorIdAsync(id_usuario);

                if (usuarioExistente == null)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Usuario no encontrado",
                        UsuarioActualizado = null
                    };
                }
                if (!usuarioExistente.cuenta_activa)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "No se puede editar un usuario con cuenta desactivada",
                        UsuarioActualizado = null
                    };
                }

                if (!string.IsNullOrEmpty(usuario.tipo_usuario) &&
                    !new[] { "normal", "artista", "organizador" }.Contains(usuario.tipo_usuario.ToLower()))
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Tipo de usuario inválido. Debe ser: normal, artista u organizador",
                        UsuarioActualizado = null
                    };
                }

                if (usuario.id_pais.HasValue)
                {
                    bool paisExiste = await VerificarPaisExisteAsync(usuario.id_pais.Value);
                    if (!paisExiste)
                    {
                        return new UsuarioEditarResponseDTO
                        {
                            Exitoso = false,
                            Mensaje = $"El país con ID {usuario.id_pais.Value} no existe",
                            UsuarioActualizado = null
                        };
                    }
                }

                var resultado = await Task.Run(() => _usuarioDAO.ActualizarUsuario(id_usuario, usuario));

                if (resultado.Contains("correctamente"))
                {
                    var usuarioActualizado = await ObtenerUsuarioPorIdAsync(id_usuario);

                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = resultado,
                        UsuarioActualizado = usuarioActualizado
                    };
                }
                else
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = resultado,
                        UsuarioActualizado = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new UsuarioEditarResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al actualizar usuario: {ex.Message}",
                    UsuarioActualizado = null
                };
            }
        }
        public async Task<bool> VerificarCorreoExistenteAsync(string correo_electronico)
        {
            try
            {
                return await Task.Run(() => _usuarioDAO.VerificarCorreoExistente(correo_electronico));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> VerificarPaisExisteAsync(int id_pais)
        {
            try
            {
                return await Task.Run(() => _usuarioDAO.VerificarPaisExiste(id_pais));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UsuarioActivarCuentaResponseDTO> ActivarDesactivarCuentaAsync(int id_usuario, bool cuenta_activa)
        {
            try
            {
                string resultado = await Task.Run(() => _usuarioDAO.ActivarDesactivarCuenta(id_usuario, cuenta_activa));

                if (resultado.Contains("exitosa"))
                {
                    return new UsuarioActivarCuentaResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = resultado,
                        CuentaActiva = cuenta_activa
                    };
                }
                else
                {
                    return new UsuarioActivarCuentaResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = resultado,
                        CuentaActiva = !cuenta_activa
                    };
                }
            }
            catch (Exception ex)
            {
                return new UsuarioActivarCuentaResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error: {ex.Message}",
                    CuentaActiva = false
                };
            }
        }
    }
}