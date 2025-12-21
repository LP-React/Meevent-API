using Meevent_API.src.Core.Entities;
using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Usuarios;
using Meevent_API.src.Features.Usuarios.Meevent_API.src.Features.Usuarios;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Meevent_API.src.Features.Usuarios.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {
        private readonly string _cadena;

        public UsuarioDAO(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        public IEnumerable<Usuario> GetUsuarios()
        {
            List<Usuario> temporal = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Usuario
                    {
                        IdUsuario = dr.GetInt32(dr.GetOrdinal("id_usuario")),
                        NombreCompleto = dr.GetString(dr.GetOrdinal("nombre_completo")),
                        CorreoElectronico = dr.GetString(dr.GetOrdinal("correo_electronico")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null: DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento"))),
                        FechaCreacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_creacion"))),
                        FechaActualizacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion"))),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario")),
                        IdPaisNavigation = new Pais
                        {
                            NombrePais = dr.GetString(dr.GetOrdinal("nombre_pais"))
                        },
                        IdCiudadNavigation = new Ciudad
                        {
                            IdCiudad = dr.GetInt32(dr.GetOrdinal("id_ciudad")),
                            NombreCiudad = dr.GetString(dr.GetOrdinal("nombre_ciudad")),
                            IdPais = dr.GetInt32(dr.GetOrdinal("id_pais"))
                        }
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public IEnumerable<Usuario> GetUsuariosPorId(int id_usuario)
        {
            List<Usuario> temporal = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_UsuarioPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Usuario
                    {
                        IdUsuario = dr.GetInt32(dr.GetOrdinal("id_usuario")),
                        NombreCompleto = dr.GetString(dr.GetOrdinal("nombre_completo")),
                        CorreoElectronico = dr.GetString(dr.GetOrdinal("correo_electronico")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null : DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento"))),
                        FechaCreacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_creacion"))),
                        FechaActualizacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion"))),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario")),
                        IdPaisNavigation = new Pais
                        {
                            NombrePais = dr.GetString(dr.GetOrdinal("nombre_pais"))
                        },
                        IdCiudadNavigation = new Ciudad
                        {
                            IdCiudad = dr.GetInt32(dr.GetOrdinal("id_ciudad")),
                            NombreCiudad = dr.GetString(dr.GetOrdinal("nombre_ciudad")),
                            IdPais = dr.GetInt32(dr.GetOrdinal("id_pais"))
                        }
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public IEnumerable<Usuario> GetUsuariosPorCorreo(string correo_electronico)
        {
            List<Usuario> temporal = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_UsuarioPorCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Usuario
                    {
                        IdUsuario = dr.GetInt32(dr.GetOrdinal("id_usuario")),
                        NombreCompleto = dr.GetString(dr.GetOrdinal("nombre_completo")),
                        CorreoElectronico = dr.GetString(dr.GetOrdinal("correo_electronico")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null : DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento"))),
                        FechaCreacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_creacion"))),
                        FechaActualizacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion"))),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario")),
                        IdPaisNavigation = new Pais
                        {
                            NombrePais = dr.GetString(dr.GetOrdinal("nombre_pais"))
                        },
                        IdCiudadNavigation = new Ciudad
                        {
                            IdCiudad = dr.GetInt32(dr.GetOrdinal("id_ciudad")),
                            NombreCiudad = dr.GetString(dr.GetOrdinal("nombre_ciudad")),
                            IdPais = dr.GetInt32(dr.GetOrdinal("id_pais"))
                        }
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public string InsertUsuario(UsuarioRegistroDTO reg)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();

                bool correoExiste = VerificarCorreoExistente(reg.correo_electronico);
                if (correoExiste)
                    return "El correo ya está registrado";

                string hash = BCrypt.Net.BCrypt.HashPassword(reg.contrasena);

                SqlCommand cmd = new SqlCommand("usp_CrearUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre_completo", reg.nombre_completo);
                cmd.Parameters.AddWithValue("@correo_electronico", reg.correo_electronico);
                cmd.Parameters.AddWithValue("@contrasena_hash", hash);
                cmd.Parameters.AddWithValue("@numero_telefono", reg.numero_telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@imagen_perfil_url", reg.imagen_perfil_url ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", reg.fecha_nacimiento ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email_verificado", false);
                cmd.Parameters.AddWithValue("@cuenta_activa", true);
                cmd.Parameters.AddWithValue("@tipo_usuario", string.IsNullOrEmpty(reg.tipo_usuario) ? "normal" : reg.tipo_usuario);
                cmd.Parameters.AddWithValue("@id_pais", reg.id_pais);
                cmd.Parameters.AddWithValue("@id_ciudad", reg.id_ciudad);
                cmd.ExecuteNonQuery();

                return "Usuario registrado correctamente";
            }
        }

        public string LoginUsuario(LoginDTO login)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("usp_UsuarioPorCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", login.correo_electronico);

                SqlDataReader dr = cmd.ExecuteReader();

                Usuario usuario = null;

                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = dr.GetInt32(dr.GetOrdinal("id_usuario")),
                        NombreCompleto = dr.GetString(dr.GetOrdinal("nombre_completo")),
                        CorreoElectronico = dr.GetString(dr.GetOrdinal("correo_electronico")),
                        ContrasenaHash = dr.GetString(dr.GetOrdinal("contrasena_hash")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario"))
                    };
                }
                dr.Close();

                if (usuario == null)
                    return "Correo o contraseña incorrectos";

                bool valido = BCrypt.Net.BCrypt.Verify(login.contrasena, usuario.ContrasenaHash);
                if (!valido)
                    return "Correo o contraseña incorrectos";

                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["Jwt:Key"])
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                    new Claim(ClaimTypes.Role, usuario.TipoUsuario)
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(300),
                    signingCredentials: creds
                );

                string jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
        }


        public string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();

                SqlCommand cmdVerificar = new SqlCommand("SELECT cuenta_activa FROM usuarios WHERE id_usuario = @id_usuario", cn);
                cmdVerificar.Parameters.AddWithValue("@id_usuario", id_usuario);

                var resultadoVerificacion = cmdVerificar.ExecuteScalar();
                if (resultadoVerificacion == null)
                    return "Usuario no encontrado";
                bool cuentaActivaActual = (bool)resultadoVerificacion;
                if (!cuentaActivaActual)
                    return "No se puede editar un usuario con cuenta desactivada";
                var usuarioActual = GetUsuariosPorId(id_usuario).FirstOrDefault();
                if (usuarioActual == null)
                    return "Usuario no encontrado";
                if (!string.IsNullOrEmpty(usuario.tipo_usuario) &&
                    !new[] { "normal", "artista", "organizador" }.Contains(usuario.tipo_usuario.ToLower()))
                {
                    return "Tipo de usuario inválido. Debe ser: normal, artista u organizador";
                }

                string tipoUsuario = !string.IsNullOrEmpty(usuario.tipo_usuario)
                    ? usuario.tipo_usuario.ToLower()
                    : usuarioActual.TipoUsuario;

                bool? emailVerificado = usuario.email_verificado ?? usuarioActual.EmailVerificado;
                string contrasenaHash = usuarioActual.ContrasenaHash;
                if (!string.IsNullOrEmpty(usuario.contrasena))
                {
                    contrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.contrasena);
                }

                SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@nombre_completo", string.IsNullOrEmpty(usuario.nombre_completo) ? (object)DBNull.Value : usuario.nombre_completo);
                cmd.Parameters.AddWithValue("@numero_telefono", string.IsNullOrEmpty(usuario.numero_telefono) ? (object)DBNull.Value : usuario.numero_telefono);
                cmd.Parameters.AddWithValue("@imagen_perfil_url", string.IsNullOrEmpty(usuario.imagen_perfil_url) ? (object)DBNull.Value : usuario.imagen_perfil_url);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", usuario.fecha_nacimiento ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email_verificado", emailVerificado ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@tipo_usuario", string.IsNullOrEmpty(usuario.tipo_usuario) ? (object)DBNull.Value : tipoUsuario);
                cmd.Parameters.AddWithValue("@contrasena_hash", string.IsNullOrEmpty(usuario.contrasena) ? (object)DBNull.Value : contrasenaHash);
                cmd.Parameters.AddWithValue("@id_pais", usuario.id_pais.HasValue ? usuario.id_pais.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id_ciudad", usuario.id_ciudad.HasValue ? usuario.id_ciudad.Value : (object)DBNull.Value);

                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return "Usuario actualizado correctamente";
                else
                    return "No se pudo actualizar el usuario";
            }
        }

        public bool VerificarCorreoExistente(string correo_electronico)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);

                cn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToBoolean(result);
                }

                return false;
            }
        }

        public bool VerificarPaisExiste(int id_pais)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarPaisExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pais", id_pais);

                cn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToBoolean(result);
                }

                return false;
            }
        }
        public bool VerificarCiudadExiste(int id_ciudad)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarCiudadExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_ciudad", id_ciudad);

                cn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToBoolean(result);
                }

                return false;
            }
        }

        public string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    cn.Open();
                    SqlCommand cmdVerificar = new SqlCommand(
                        "SELECT COUNT(1) FROM usuarios WHERE id_usuario = @id_usuario",
                        cn);
                    cmdVerificar.Parameters.AddWithValue("@id_usuario", id_usuario);

                    int existe = (int)cmdVerificar.ExecuteScalar();

                    if (existe == 0)
                    {
                        return $"Usuario con ID {id_usuario} no encontrado";
                    }
                    SqlCommand cmd = new SqlCommand("usp_EditarCuentaActiva", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                    cmd.Parameters.AddWithValue("@cuenta_activa", cuenta_activa);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        string estado = cuenta_activa ? "activada" : "desactivada";
                        return $"Cuenta {estado} exitosamente";
                    }

                    return "No se pudo actualizar el estado de la cuenta";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}