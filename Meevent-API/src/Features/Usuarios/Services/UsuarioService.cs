
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

        public async Task<UsuarioListResponseDTO> ObtenerUsuariosAsync()
        {
            try
            {
                var usuarios = await Task.Run(() => _usuarioDAO.GetUsuarios());

                var usuariosDTO = new List<UsuarioDTO>();
                foreach (var u in usuarios)
                {
                    var dto = new UsuarioDTO
                    {
                        id_usuario = u.IdUsuario,
                        nombre_completo = u.NombreCompleto,
                        correo_electronico = u.CorreoElectronico,
                        numero_telefono = u.NumeroTelefono,
                        imagen_perfil_url = u.ImagenPerfilUrl,
                        fecha_nacimiento = u.FechaNacimiento,
                        fecha_creacion = u.FechaCreacion,
                        fecha_actualizacion = u.FechaActualizacion,
                        tipo_usuario = u.TipoUsuario,
                        cuenta_activa = u.CuentaActiva,
                        jsonCiudad = new CiudadDTO
                        {
                            IdCiudad = u.IdCiudadNavigation.IdCiudad,
                            NombreCiudad = u.IdCiudadNavigation.NombreCiudad,
                            IdPais = u.IdCiudadNavigation.IdPais,
                            jsonPais = new PaisJDTO
                            {
                                NombrePais = u.IdPaisNavigation.NombrePais,
                            }
                        }
                    };

                    if (u.TipoUsuario?.ToLower() == "artista")
                        dto.perfil = await _perfilArtistaDAO.ObtenerPorUsuarioIdAsync(u.IdUsuario);
                    else if (u.TipoUsuario?.ToLower() == "organizador")
                        dto.perfil = await _perfilOrganizadorDAO.ObtenerPorUsuarioIdAsync(u.IdUsuario);

                    usuariosDTO.Add(dto);
                }

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

                if (usuario == null) return null;

                var dto = new UsuarioDTO
                {
                    id_usuario = usuario.IdUsuario,
                    nombre_completo = usuario.NombreCompleto,
                    correo_electronico = usuario.CorreoElectronico,
                    numero_telefono = usuario.NumeroTelefono,
                    imagen_perfil_url = usuario.ImagenPerfilUrl,
                    fecha_nacimiento = usuario.FechaNacimiento,
                    tipo_usuario = usuario.TipoUsuario,
                    cuenta_activa = usuario.CuentaActiva,
                    jsonCiudad = new CiudadDTO
                    {
                        IdCiudad = usuario.IdCiudadNavigation.IdCiudad,
                        NombreCiudad = usuario.IdCiudadNavigation.NombreCiudad,
                        IdPais = usuario.IdCiudadNavigation.IdPais,
                        jsonPais = new PaisJDTO
                        {
                            NombrePais = usuario.IdPaisNavigation.NombrePais,
                        }
                    }
                };

                if (usuario.TipoUsuario?.ToLower() == "artista")
                    dto.perfil = await _perfilArtistaDAO.ObtenerPorUsuarioIdAsync(usuario.IdUsuario);
                else if (usuario.TipoUsuario?.ToLower() == "organizador")
                    dto.perfil = await _perfilOrganizadorDAO.ObtenerPorUsuarioIdAsync(usuario.IdUsuario);

                return dto;
            }
            catch { return null; }
        }

        public async Task<UsuarioDetalleDTO> ObtenerUsuarioPorCorreoAsync(string correo_electronico)
        {
            if (string.IsNullOrWhiteSpace(correo_electronico)) 
                return null;

            var usuario = await Task.Run(() => _usuarioDAO.GetUsuariosPorCorreo(correo_electronico));

            return usuario;
        }

        public async Task<string> RegistrarUsuarioAsync(UsuarioRegistroDTO registro)
        {
            try
            {
                var tiposValidos = new[] { "normal", "artista", "organizador" };
                if (!string.IsNullOrEmpty(registro.tipo_usuario) &&
                    !tiposValidos.Contains(registro.tipo_usuario.ToLower()))
                {
                    return "Tipo de usuario inválido.";
                }

                if (!await VerificarCiudadExisteAsync(registro.id_ciudad))
                {
                    return $"La ciudad con ID {registro.id_ciudad} no existe.";
                }

                if (await VerificarCorreoExistenteAsync(registro.correo_electronico))
                {
                    return "El correo electrónico ya está registrado.";
                }

                return await Task.Run(() => _usuarioDAO.InsertUsuario(registro));
            }
            catch (Exception ex)
            {
                return $"Error interno al procesar el registro.";
            }
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO login)
        {

            return new LoginResponseDTO 
            { 
                Exitoso = false, 
                Mensaje = "Servicio de login no disponible temporalmente" 
            };

            /*try
            {
                if (string.IsNullOrEmpty(login.contrasena) || string.IsNullOrEmpty(login.correo_electronico))
                {
                    return new LoginResponseDTO { Exitoso = false, Mensaje = "Correo o contraseña incorrectos" };
                }

                string token;
                try
                {
                    token = await Task.Run(() => _usuarioDAO.LoginUsuario(login));
                }
                catch (Exception)
                {
                    return new LoginResponseDTO { Exitoso = false, Mensaje = "Correo o contraseña incorrectos" };
                }

                if (string.IsNullOrEmpty(token) || token.Contains("incorrecto") || token.Contains("no existe"))
                {
                    return new LoginResponseDTO { Exitoso = false, Mensaje = "Correo o contraseña incorrectos" };
                }

                var usuarioDTO = await ObtenerUsuarioPorCorreoAsync(login.correo_electronico);

                return new LoginResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Login exitoso",
                    Token = token,
                    Usuario = usuarioDTO
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO { Exitoso = false, Mensaje = "Correo o contraseña incorrectos" };
            }*/
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
                        Mensaje = "Debe proporcionar al menos un campo para actualizar"
                    };
                }

                var usuarioExistente = await ObtenerUsuarioPorIdAsync(id_usuario);
                if (usuarioExistente == null)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Usuario no encontrado"
                    };
                }

                if (!usuarioExistente.cuenta_activa)
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "No se puede editar un usuario con cuenta desactivada"
                    };
                }

                if (!string.IsNullOrEmpty(usuario.tipo_usuario) &&
                    !new[] { "normal", "artista", "organizador" }.Contains(usuario.tipo_usuario.ToLower()))
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Tipo de usuario inválido. Debe ser: normal, artista u organizador"
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
                            Mensaje = $"El país con ID {usuario.id_pais.Value} no existe"
                        };
                    }
                }

                var resultadoStr = await Task.Run(() => _usuarioDAO.ActualizarUsuario(id_usuario, usuario));

                if (resultadoStr.Contains("correctamente"))
                {
                    return new UsuarioEditarResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = resultadoStr,
                        UsuarioActualizado = await ObtenerUsuarioPorIdAsync(id_usuario)
                    };
                }

                return new UsuarioEditarResponseDTO
                {
                    Exitoso = false,
                    Mensaje = resultadoStr
                };
            }
            catch (Exception ex)
            {
                return new UsuarioEditarResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error inesperado en el servicio: {ex.Message}"
                };
            }
        }

        public async Task<bool> VerificarCorreoExistenteAsync(string correo) 
            => await Task.Run(() => _usuarioDAO.VerificarCorreoExistente(correo));
        public async Task<bool> VerificarPaisExisteAsync(int id)
            => await Task.Run(() => _usuarioDAO.VerificarPaisExiste(id));
        public async Task<bool> VerificarCiudadExisteAsync(int id) 
            => await Task.Run(() => _usuarioDAO.VerificarCiudadExiste(id));

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
    }
}