using Meevent_API.src.Core.Entities;
using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Usuarios;
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
                        ContrasenaHash = dr.GetString(dr.GetOrdinal("contrasena_hash")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null : dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento")),
                        FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                        FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario"))
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
                        ContrasenaHash = dr.GetString(dr.GetOrdinal("contrasena_hash")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null : dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento")),
                        FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                        FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario"))
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
                        ContrasenaHash = dr.GetString(dr.GetOrdinal("contrasena_hash")),
                        NumeroTelefono = dr.IsDBNull(dr.GetOrdinal("numero_telefono")) ? null : dr.GetString(dr.GetOrdinal("numero_telefono")),
                        ImagenPerfilUrl = dr.IsDBNull(dr.GetOrdinal("imagen_perfil_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_perfil_url")),
                        FechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fecha_nacimiento")) ? null : dr.GetDateTime(dr.GetOrdinal("fecha_nacimiento")),
                        FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                        FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),
                        EmailVerificado = dr.GetBoolean(dr.GetOrdinal("email_verificado")),
                        CuentaActiva = dr.GetBoolean(dr.GetOrdinal("cuenta_activa")),
                        TipoUsuario = dr.GetString(dr.GetOrdinal("tipo_usuario"))
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

                SqlCommand validar = new SqlCommand(
                    "SELECT COUNT(*) FROM usuarios WHERE correo_electronico=@correo", cn);
                validar.Parameters.AddWithValue("@correo", reg.correo_electronico);

                int existe = (int)validar.ExecuteScalar();
                if (existe > 0)
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
                cmd.Parameters.AddWithValue("@tipo_usuario", "normal");

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


        public string ActualizarUsuario(UsuarioEditarDTO usuario)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                var usuarioActual = GetUsuariosPorId(usuario.id_usuario).FirstOrDefault();

                if (usuarioActual == null)
                    return "Usuario no encontrado";
                string contrasenaHash = usuarioActual.ContrasenaHash;

                if (!string.IsNullOrEmpty(usuario.contrasena))
                {
                    contrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.contrasena);
                }

                if (!string.IsNullOrEmpty(usuario.tipo_usuario) &&
                    !new[] { "normal", "artista", "organizador" }.Contains(usuario.tipo_usuario.ToLower()))
                {
                    return "Tipo de usuario inválido. Debe ser: normal, artista u organizador";
                }

                string tipoUsuario = !string.IsNullOrEmpty(usuario.tipo_usuario)
                    ? usuario.tipo_usuario.ToLower()
                    : usuarioActual.TipoUsuario;

                bool emailVerificado = usuario.email_verificado ?? usuarioActual.EmailVerificado;
                bool cuentaActiva = usuario.cuenta_activa ?? usuarioActual.CuentaActiva;

                SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_usuario", usuario.id_usuario);
                cmd.Parameters.AddWithValue("@nombre_completo", usuario.nombre_completo);
                cmd.Parameters.AddWithValue("@contrasena_hash", contrasenaHash);
                cmd.Parameters.AddWithValue("@numero_telefono", usuario.numero_telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@imagen_perfil_url", usuario.imagen_perfil_url ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", usuario.fecha_nacimiento ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email_verificado", emailVerificado);
                cmd.Parameters.AddWithValue("@cuenta_activa", cuentaActiva);
                cmd.Parameters.AddWithValue("@tipo_usuario", tipoUsuario);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    return "Usuario actualizado correctamente";
                else
                    return "No se pudo actualizar el usuario";
            }
        }

    }
}