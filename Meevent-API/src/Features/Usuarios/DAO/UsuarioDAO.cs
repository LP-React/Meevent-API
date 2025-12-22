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
                            var usuario = new UsuarioDetalleDTO
                            {
                                id_usuario = dr.GetInt32(0),
                                nombre_completo = dr.GetString(1),
                                tipo_usuario = dr.GetString(2),
                                correo_electronico = dr.GetString(3),
                                numero_telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                                imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                                fecha_nacimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                                email_verificado = dr.GetBoolean(7),

                                Ubicacion = new UbicacionDTO
                                {
                                    id_ciudad = dr.GetInt32(9),
                                    nombre_ciudad = dr.GetString(10),
                                    id_pais = dr.GetInt32(11),
                                    nombre_pais = dr.GetString(12),
                                    codigo_iso = dr.GetString(13)
                                }
                            };

                            if (usuario.tipo_usuario == "artista" && !dr.IsDBNull(14))
                            {
                                usuario.PerfilArtista = new PerfilArtistaDTO
                                {
                                    id_perfil_artista = dr.GetInt32(14),
                                    nombre_artistico = dr.IsDBNull(15) ? null : dr.GetString(15),
                                    biografia_artista = dr.IsDBNull(16) ? null : dr.GetString(16),
                                    genero_musical = dr.IsDBNull(17) ? null : dr.GetString(17),
                                    sitio_web = dr.IsDBNull(18) ? null : dr.GetString(18),
                                    facebook_url = dr.IsDBNull(19) ? null : dr.GetString(19),
                                    instagram_url = dr.IsDBNull(20) ? null : dr.GetString(20),
                                    tiktok_url = dr.IsDBNull(21) ? null : dr.GetString(21)
                                };
                            }

                            if (usuario.tipo_usuario == "organizador" && !dr.IsDBNull(22))
                            {
                                usuario.PerfilOrganizador = new PerfilOrganizadorDTO
                                {
                                    id_perfil_organizador = dr.GetInt32(22),
                                    nombre_organizador = dr.GetString(23),
                                    descripcion_organizador = dr.IsDBNull(24) ? null : dr.GetString(24),
                                    direccion_organizador = dr.IsDBNull(25) ? null : dr.GetString(25),
                                    telefono_contacto = dr.IsDBNull(26) ? null : dr.GetString(26),
                                    sitio_web = dr.IsDBNull(27) ? null : dr.GetString(27),
                                    logo_url = dr.IsDBNull(28) ? null : dr.GetString(28),
                                    facebook_url = dr.IsDBNull(29) ? null : dr.GetString(29),
                                    instagram_url = dr.IsDBNull(30) ? null : dr.GetString(30),
                                    tiktok_url = dr.IsDBNull(31) ? null : dr.GetString(31),
                                    twitter_url = dr.IsDBNull(32) ? null : dr.GetString(32)
                                };
                            }

                            lista.Add(usuario);
                        }
                    }
                }
            }
            return lista;
        }

        public async Task<UsuarioDetalleDTO> GetUsuariosPorCorreo(string correo_electronico)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_UsuarioPorCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);
                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        var usuario = new UsuarioDetalleDTO
                        {
                            // Tabla Usuarios (0-8)
                            id_usuario = dr.GetInt32(0),
                            nombre_completo = dr.GetString(1),
                            tipo_usuario = dr.GetString(2),
                            correo_electronico = dr.GetString(3),
                            numero_telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                            imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                            fecha_nacimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                            email_verificado = dr.GetBoolean(7),
                            // cuenta_activa es el índice 8

                            // Tabla Ubicación (9-13)
                            Ubicacion = new UbicacionDTO
                            {
                                id_ciudad = dr.GetInt32(9),
                                nombre_ciudad = dr.GetString(10),
                                id_pais = dr.GetInt32(11),
                                nombre_pais = dr.GetString(12),
                                codigo_iso = dr.GetString(13)
                            }
                        };

                        // Perfil Artista (14-21)
                        if (usuario.tipo_usuario == "artista" && !dr.IsDBNull(14))
                        {
                            usuario.PerfilArtista = new PerfilArtistaDTO
                            {
                                id_perfil_artista = dr.GetInt32(14),
                                nombre_artistico = dr.IsDBNull(15) ? null : dr.GetString(15),
                                biografia_artista = dr.IsDBNull(16) ? null : dr.GetString(16),
                                genero_musical = dr.IsDBNull(17) ? null : dr.GetString(17),
                                sitio_web = dr.IsDBNull(18) ? null : dr.GetString(18),
                                facebook_url = dr.IsDBNull(19) ? null : dr.GetString(19),
                                instagram_url = dr.IsDBNull(20) ? null : dr.GetString(20),
                                tiktok_url = dr.IsDBNull(21) ? null : dr.GetString(21)
                            };
                        }

                        // Perfil Organizador (22-32)
                        if (usuario.tipo_usuario == "organizador" && !dr.IsDBNull(22))
                        {
                            usuario.PerfilOrganizador = new PerfilOrganizadorDTO
                            {
                                id_perfil_organizador = dr.GetInt32(22),
                                nombre_organizador = dr.GetString(23),
                                descripcion_organizador = dr.IsDBNull(24) ? null : dr.GetString(24),
                                direccion_organizador = dr.IsDBNull(25) ? null : dr.GetString(25),
                                telefono_contacto = dr.IsDBNull(26) ? null : dr.GetString(26),
                                sitio_web = dr.IsDBNull(27) ? null : dr.GetString(27),
                                logo_url = dr.IsDBNull(28) ? null : dr.GetString(28),
                                facebook_url = dr.IsDBNull(29) ? null : dr.GetString(29),
                                instagram_url = dr.IsDBNull(30) ? null : dr.GetString(30),
                                tiktok_url = dr.IsDBNull(31) ? null : dr.GetString(31),
                                twitter_url = dr.IsDBNull(32) ? null : dr.GetString(32)
                            };
                        }

                        return usuario;
                    }
                }
            }
            return null;
        }

        public async Task<UsuarioDetalleDTO?> GetUsuariosPorId(int id_usuario)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_UsuarioPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        var usuario = new UsuarioDetalleDTO
                        {
                            id_usuario = dr.GetInt32(0),
                            nombre_completo = dr.GetString(1),
                            tipo_usuario = dr.GetString(2),
                            correo_electronico = dr.GetString(3),
                            numero_telefono = dr.IsDBNull(4) ? null : dr.GetString(4),
                            imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                            fecha_nacimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                            email_verificado = dr.GetBoolean(7),

                            Ubicacion = new UbicacionDTO
                            {
                                id_ciudad = dr.GetInt32(9),
                                nombre_ciudad = dr.GetString(10),
                                id_pais = dr.GetInt32(11),
                                nombre_pais = dr.GetString(12),
                                codigo_iso = dr.GetString(13)
                            }
                        };

                        if (usuario.tipo_usuario == "artista" && !dr.IsDBNull(14))
                        {
                            usuario.PerfilArtista = new PerfilArtistaDTO
                            {
                                id_perfil_artista = dr.GetInt32(14),
                                nombre_artistico = dr.IsDBNull(15) ? null : dr.GetString(15),
                                biografia_artista = dr.IsDBNull(16) ? null : dr.GetString(16),
                                genero_musical = dr.IsDBNull(17) ? null : dr.GetString(17),
                                sitio_web = dr.IsDBNull(18) ? null : dr.GetString(18),
                                facebook_url = dr.IsDBNull(19) ? null : dr.GetString(19),
                                instagram_url = dr.IsDBNull(20) ? null : dr.GetString(20),
                                tiktok_url = dr.IsDBNull(21) ? null : dr.GetString(21)
                            };
                        }

                        // Perfil Organizador (Índices 22-32)
                        if (usuario.tipo_usuario == "organizador" && !dr.IsDBNull(22))
                        {
                            usuario.PerfilOrganizador = new PerfilOrganizadorDTO
                            {
                                id_perfil_organizador = dr.GetInt32(22),
                                nombre_organizador = dr.GetString(23),
                                descripcion_organizador = dr.IsDBNull(24) ? null : dr.GetString(24),
                                direccion_organizador = dr.IsDBNull(25) ? null : dr.GetString(25),
                                telefono_contacto = dr.IsDBNull(26) ? null : dr.GetString(26),
                                logo_url = dr.IsDBNull(28) ? null : dr.GetString(28),
                                facebook_url = dr.IsDBNull(29) ? null : dr.GetString(29),
                                instagram_url = dr.IsDBNull(30) ? null : dr.GetString(30),
                                tiktok_url = dr.IsDBNull(31) ? null : dr.GetString(31),
                                twitter_url = dr.IsDBNull(32) ? null : dr.GetString(32)
                            };
                        }

                        return usuario;
                    }
                }
            }
            return null;
        }

        public async Task<string> InsertUsuario(UsuarioRegistroDTO reg)
        {
            string resultado = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarUsuarioCompleto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Datos base del Usuario
                        cmd.Parameters.AddWithValue("@nombre_completo", reg.nombre_completo);
                        cmd.Parameters.AddWithValue("@correo_electronico", reg.correo_electronico);
                        cmd.Parameters.AddWithValue("@contrasena_hash", reg.contrasenia);
                        cmd.Parameters.AddWithValue("@tipo_usuario", (object)reg.tipo_usuario ?? "normal");
                        cmd.Parameters.AddWithValue("@id_ciudad", reg.id_ciudad);
                        cmd.Parameters.AddWithValue("@numero_telefono", (object)reg.numero_telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@imagen_perfil_url", (object)reg.imagen_perfil_url ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fecha_nacimiento", (object)reg.fecha_nacimiento ?? DBNull.Value);

                        // Datos específicos de Artista
                        cmd.Parameters.AddWithValue("@nombre_artistico", (object)reg.nombre_artistico ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@biografia_artista", (object)reg.biografia_artista ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@genero_musical", (object)reg.genero_musical ?? DBNull.Value);

                        // Datos específicos de Organizador
                        cmd.Parameters.AddWithValue("@nombre_organizador", (object)reg.nombre_organizador ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@descripcion_organizador", (object)reg.descripcion_organizador ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono_contacto", (object)reg.telefono_contacto ?? DBNull.Value);

                        await cn.OpenAsync();
                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                        // Importante: Si insertó en dos tablas, filasAfectadas será > 1
                        if (filasAfectadas > 0)
                        {
                            resultado = "OK";
                        }
                        else
                        {
                            resultado = "Error: No se pudo insertar el registro.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = "Excepción: " + ex.Message;
            }

            return resultado;
        }

        public async Task<string> ActualizarUsuarioAsync(UsuarioUpdateDTO dto)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarUsuarioCompleto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_usuario", dto.id_usuario);

                    cmd.Parameters.AddWithValue("@nombre_completo", (object)dto.nombre_completo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id_ciudad", (object)dto.id_ciudad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@numero_telefono", (object)dto.numero_telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@imagen_perfil_url", (object)dto.imagen_perfil_url ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@nombre_artistico", (object)dto.nombre_artistico ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@biografia_artista", (object)dto.biografia_artista ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@genero_musical", (object)dto.genero_musical ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@nombre_organizador", (object)dto.nombre_organizador ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@descripcion_organizador", (object)dto.descripcion_organizador ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@telefono_contacto", (object)dto.telefono_contacto ?? DBNull.Value);

                    await cn.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "No se encontró el usuario o no se realizaron cambios.";
                    }
                }
            }
            catch (SqlException ex)
            {
                // Captura errores específicos de SQL (como violaciones de FK si cambian la ciudad a una inválida)
                return "Error de Base de Datos: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<UsuarioLoginResponseDTO?> ObtenerUsuarioLogin(string correo)
        {
            UsuarioLoginResponseDTO? user = null;

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Obtener_usuario_login", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", correo);

                    await cn.OpenAsync();
                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            user = new UsuarioLoginResponseDTO
                            {
                                id_usuario = dr.GetInt32(0),
                                nombre_completo = dr.GetString(1),
                                correo_electronico = dr.GetString(2),
                                contrasena_hash = dr.GetString(3),
                                tipo_usuario = dr.GetString(4),
                                imagen_perfil_url = dr.IsDBNull(5) ? null : dr.GetString(5),
                                cuenta_activa = dr.GetBoolean(25)
                            };

                            // --- MAPEO DE PERFIL ARTISTA (Inicia en índice 6) ---
                            if (user.tipo_usuario == "artista" && !dr.IsDBNull(6))
                            {
                                user.PerfilArtista = new PerfilArtistaDTO
                                {
                                    id_perfil_artista = dr.GetInt32(6),
                                    nombre_artistico = dr.GetString(7),
                                    biografia_artista = dr.IsDBNull(8) ? null : dr.GetString(8),
                                    genero_musical = dr.IsDBNull(9) ? null : dr.GetString(9),
                                    sitio_web = dr.IsDBNull(10) ? null : dr.GetString(10),
                                    facebook_url = dr.IsDBNull(11) ? null : dr.GetString(11),
                                    instagram_url = dr.IsDBNull(12) ? null : dr.GetString(12),
                                    tiktok_url = dr.IsDBNull(13) ? null : dr.GetString(13)
                                };
                            }

                            // --- MAPEO DE PERFIL ORGANIZADOR (Inicia en índice 14) ---
                            else if (user.tipo_usuario == "organizador" && !dr.IsDBNull(14))
                            {
                                user.PerfilOrganizador = new PerfilOrganizadorDTO
                                {
                                    id_perfil_organizador = dr.GetInt32(14),
                                    nombre_organizador = dr.GetString(15),
                                    descripcion_organizador = dr.IsDBNull(16) ? null : dr.GetString(16),
                                    direccion_organizador = dr.IsDBNull(17) ? null : dr.GetString(17),
                                    telefono_contacto = dr.IsDBNull(18) ? null : dr.GetString(18),
                                    sitio_web = dr.IsDBNull(19) ? null : dr.GetString(19),
                                    logo_url = dr.IsDBNull(20) ? null : dr.GetString(20),
                                    facebook_url = dr.IsDBNull(21) ? null : dr.GetString(21),
                                    instagram_url = dr.IsDBNull(22) ? null : dr.GetString(22),
                                    tiktok_url = dr.IsDBNull(23) ? null : dr.GetString(23),
                                    twitter_url = dr.IsDBNull(24) ? null : dr.GetString(24)
                                };
                            }
                        }
                    }
                }
            }
            return user;
        }

        public string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa)
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
        }

        public async Task<bool> VerificarCorreoExistenteAsync(string correo_electronico)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarCorreo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo_electronico", correo_electronico);
                await cn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();
                return Convert.ToBoolean(resultado);
            }
        }

        public bool VerificarPaisExiste(int id_pais)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarPaisExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pais", id_pais);
                cn.OpenAsync();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public async Task<bool> VerificarCiudadExisteAsync(int id_ciudad)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_VerificarCiudadExiste", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_ciudad", id_ciudad);

                await cn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();
                return resultado != null && Convert.ToBoolean(resultado);
            }
        }

        public async Task<bool> CambiarContraseniaAsync(int id_usuario, UsuarioCambiarPasswordDTO dto)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_Cambiar_contrasenia_usuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@contrasenia", dto.contrasenia);

                await cn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();
                return resultado != null && Convert.ToBoolean(resultado);
            }
        }
    }
}