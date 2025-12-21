using Meevent_API.src.Core.Entities;
using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;
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
        private readonly IConfiguration _configuration;

        public UsuarioDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        public string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa)
        {
            throw new NotImplementedException();
        }

        public string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioDetalleDTO>> GetUsuarios()
         {
            var lista = new List<UsuarioDetalleDTO>();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarUsuarios", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        // Usamos while para recorrer todas las filas devueltas
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new UsuarioDetalleDTO
                            {
                                id_usuario = dr.GetInt32(0),
                                nombre_completo = dr.GetString(1),
                                tipo_usuario = dr.GetString(2),
                                correo_electronico = dr.GetString(3),
                                numero_telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                                imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                                fecha_nacimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                                email_verificado = dr.GetBoolean(7),
                                cuenta_activa = dr.GetBoolean(8),
                                id_ciudad = dr.GetInt32(9),
                                nombre_ciudad = dr.GetString(10),
                                id_pais = dr.GetInt32(11),
                                nombre_pais = dr.GetString(12),
                                codigo_iso = dr.GetString(13),
                            });
                        }
                    }
                }
            }
            return lista;
        }
        
        /*public IEnumerable<Usuario> GetUsuariosPorId(int id_usuario)
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
                        ContrasenaHash = dr.GetString(dr.GetOrdinal("contrasena_hash")),
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
            }
            return temporal;
        }
        */
        
        public async Task<UsuarioDetalleDTO> GetUsuariosPorCorreo(string correo_electronico)
        {
            UsuarioDetalleDTO usuario = null;

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_UsuarioPorCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);

                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        usuario = new UsuarioDetalleDTO
                        {
                            id_usuario = dr.GetInt32(0),
                            nombre_completo = dr.GetString(1),
                            tipo_usuario = dr.GetString(2),
                            correo_electronico = dr.GetString(3),
                            numero_telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                            imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                            fecha_nacimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                            email_verificado = dr.GetBoolean(7),
                            cuenta_activa = dr.GetBoolean(8),
                            id_ciudad = dr.GetInt32(9),
                            nombre_ciudad = dr.GetString(10),
                            id_pais = dr.GetInt32(11),
                            nombre_pais = dr.GetString(12),
                            codigo_iso = dr.GetString(13),
                        };
                    }
                }
            }
            return usuario;
        }

        public IEnumerable<Usuario> GetUsuariosPorId(int id_usuario)
        {
            throw new NotImplementedException();
        }

        public string InsertUsuario(UsuarioRegistroDTO reg)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                if (VerificarCorreoExistente(reg.correo_electronico)) return "El correo ya está registrado";

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
                cmd.Parameters.AddWithValue("@id_ciudad", reg.id_ciudad);

                cmd.ExecuteNonQuery();
                return "Usuario registrado correctamente";
            }
        }

        public string LoginUsuario(LoginDTO login)
        {
            throw new NotImplementedException();
        }

        /*public string LoginUsuario(LoginDTO login)
        {
            var usuario = GetUsuariosPorCorreo(login.correo_electronico).FirstOrDefault();
            if (usuario == null) return "Correo o contraseña incorrectos";

            if (!BCrypt.Net.BCrypt.Verify(login.contrasena, usuario.ContrasenaHash))
                return "Correo o contraseña incorrectos";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

        /*public string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmdVerificar = new SqlCommand("SELECT cuenta_activa FROM usuarios WHERE id_usuario = @id_usuario", cn);
                cmdVerificar.Parameters.AddWithValue("@id_usuario", id_usuario);
                var resultadoVerificacion = cmdVerificar.ExecuteScalar();
                if (resultadoVerificacion == null) return "Usuario no encontrado";
                if (!(bool)resultadoVerificacion) return "No se puede editar un usuario con cuenta desactivada";

                var usuarioActual = GetUsuariosPorId(id_usuario).FirstOrDefault();
                if (usuarioActual == null) return "Usuario no encontrado";

                SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@nombre_completo", string.IsNullOrEmpty(usuario.nombre_completo) ? usuarioActual.NombreCompleto : usuario.nombre_completo);
                cmd.Parameters.AddWithValue("@numero_telefono", string.IsNullOrEmpty(usuario.numero_telefono) ? (usuarioActual.NumeroTelefono ?? (object)DBNull.Value) : usuario.numero_telefono);
                cmd.Parameters.AddWithValue("@imagen_perfil_url", string.IsNullOrEmpty(usuario.imagen_perfil_url) ? (usuarioActual.ImagenPerfilUrl ?? (object)DBNull.Value) : usuario.imagen_perfil_url);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", usuario.fecha_nacimiento ?? (usuarioActual.FechaNacimiento ?? (object)DBNull.Value));
                cmd.Parameters.AddWithValue("@email_verificado", usuario.email_verificado ?? usuarioActual.EmailVerificado);
                cmd.Parameters.AddWithValue("@tipo_usuario", string.IsNullOrEmpty(usuario.tipo_usuario) ? usuarioActual.TipoUsuario : usuario.tipo_usuario.ToLower());
                cmd.Parameters.AddWithValue("@contrasena_hash", !string.IsNullOrEmpty(usuario.contrasena) ? BCrypt.Net.BCrypt.HashPassword(usuario.contrasena) : usuarioActual.ContrasenaHash);
                cmd.Parameters.AddWithValue("@id_pais", usuario.id_pais ?? usuarioActual.IdCiudadNavigation.IdPais);
                cmd.Parameters.AddWithValue("@id_ciudad", usuario.id_ciudad ?? usuarioActual.IdCiudadNavigation.IdCiudad);

                return cmd.ExecuteNonQuery() > 0 ? "Usuario actualizado correctamente" : "No se pudo actualizar el usuario";
            }
        }*/

        /* public string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa)
         {
             using (SqlConnection cn = new SqlConnection(_cadena))
             {
                 cn.Open();
                 SqlCommand cmd = new SqlCommand("usp_EditarCuentaActiva", cn);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                 cmd.Parameters.AddWithValue("@cuenta_activa", cuenta_activa);
                 return cmd.ExecuteNonQuery() > 0 ? (cuenta_activa ? "Cuenta activada exitosamente" : "Cuenta desactivada exitosamente") : "No se pudo actualizar el estado";
             }
         }*/

        /* public bool VerificarCorreoExistente(string correo_electronico)
         {
             using (SqlConnection cn = new SqlConnection(_cadena))
             {
                 SqlCommand cmd = new SqlCommand("usp_VerificarCorreo", cn);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);
                 cn.Open();
                 return Convert.ToBoolean(cmd.ExecuteScalar());
             }
         }*/

        /*public bool VerificarPaisExiste(int id_pais)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarPaisExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pais", id_pais);
                cn.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        */
        public bool VerificarCiudadExiste(int id_ciudad)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarCiudadExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_ciudad", id_ciudad);
                cn.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool VerificarCorreoExistente(string correo_electronico)
        {
            throw new NotImplementedException();
        }

        public bool VerificarPaisExiste(int id_pais)
        {
            throw new NotImplementedException();
        }
    }
}