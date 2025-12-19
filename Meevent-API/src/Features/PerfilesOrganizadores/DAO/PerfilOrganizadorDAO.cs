using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.PerfilesOrganizador;
using Meevent_API.src.Features.PerfilesOrganizadores;
using Meevent_API.src.Features.PerfilesOrganizadores.DAO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.PerfilesOrganizador.DAO
{
    public class PerfilOrganizadorDAO : IPerfilOrganizadorDAO
    {
        private readonly string _cadena;
        private readonly IConfiguration _configuration;

        public PerfilOrganizadorDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        public IEnumerable<PerfilOrganizador> GetPerfilesOrganizador()
        {
            List<PerfilOrganizador> temporal = new List<PerfilOrganizador>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarPerfilesOrganizador", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new PerfilOrganizador
                    {
                        IdPerfilOrganizador = dr.GetInt32(dr.GetOrdinal("id_perfil_organizador")),
                        NombreOrganizador = dr.GetString(dr.GetOrdinal("nombre_organizador")),
                        DescripcionOrganizador = dr.GetString(dr.GetOrdinal("descripcion_organizador")),
                        SitioWeb = dr.IsDBNull(dr.GetOrdinal("sitio_web")) ? "" : dr.GetString(dr.GetOrdinal("sitio_web")),
                        FacebookUrl = dr.IsDBNull(dr.GetOrdinal("facebook_url")) ? "" : dr.GetString(dr.GetOrdinal("facebook_url")),
                        InstagramUrl = dr.IsDBNull(dr.GetOrdinal("instagram_url")) ? "" : dr.GetString(dr.GetOrdinal("instagram_url")),
                        TiktokUrl = dr.IsDBNull(dr.GetOrdinal("tiktok_url")) ? "" : dr.GetString(dr.GetOrdinal("tiktok_url")),
                        FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                        FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),
                        UsuarioId = dr.GetInt32(dr.GetOrdinal("usuario_id"))
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public IEnumerable<PerfilOrganizador> GetPerfilOrganizadorPorId(int id_perfil_organizador)
        {
            List<PerfilOrganizador> temporal = new List<PerfilOrganizador>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_PerfilOrganizadorPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_perfil_organizador", id_perfil_organizador);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new PerfilOrganizador
                    {
                        IdPerfilOrganizador = dr.GetInt32(dr.GetOrdinal("id_perfil_organizador")),
                        NombreOrganizador = dr.GetString(dr.GetOrdinal("nombre_organizador")),
                        DescripcionOrganizador = dr.GetString(dr.GetOrdinal("descripcion_organizador")),
                        SitioWeb = dr.IsDBNull(dr.GetOrdinal("sitio_web")) ? "" : dr.GetString(dr.GetOrdinal("sitio_web")),
                        FacebookUrl = dr.IsDBNull(dr.GetOrdinal("facebook_url")) ? "" : dr.GetString(dr.GetOrdinal("facebook_url")),
                        InstagramUrl = dr.IsDBNull(dr.GetOrdinal("instagram_url")) ? "" : dr.GetString(dr.GetOrdinal("instagram_url")),
                        TiktokUrl = dr.IsDBNull(dr.GetOrdinal("tiktok_url")) ? "" : dr.GetString(dr.GetOrdinal("tiktok_url")),
                        FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                        FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),
                        UsuarioId = dr.GetInt32(dr.GetOrdinal("usuario_id"))
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public string CrearPerfilOrganizador(PerfilOrganizadorCrearDTO perfil)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("usp_CrearPerfilOrganizador", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre_organizador", perfil.nombre_organizador);
                    cmd.Parameters.AddWithValue("@descripcion_organizador", perfil.descripcion_organizador);
                    cmd.Parameters.AddWithValue("@usuario_id", perfil.usuario_id);
                    cmd.Parameters.AddWithValue("@sitio_web",string.IsNullOrEmpty(perfil.sitio_web) ? (object)DBNull.Value : perfil.sitio_web);
                    cmd.Parameters.AddWithValue("@logo_url",string.IsNullOrEmpty(perfil.logo_url) ? (object)DBNull.Value : perfil.logo_url);
                    cmd.Parameters.AddWithValue("@facebook_url",string.IsNullOrEmpty(perfil.facebook_url) ? (object)DBNull.Value : perfil.facebook_url);
                    cmd.Parameters.AddWithValue("@instagram_url",string.IsNullOrEmpty(perfil.instagram_url) ? (object)DBNull.Value : perfil.instagram_url);
                    cmd.Parameters.AddWithValue("@tiktok_url",string.IsNullOrEmpty(perfil.tiktok_url) ? (object)DBNull.Value : perfil.tiktok_url);
                    cmd.Parameters.AddWithValue("@twitter_url",string.IsNullOrEmpty(perfil.twitter_url) ? (object)DBNull.Value : perfil.twitter_url);
                    cmd.Parameters.AddWithValue("@direccion_organizador",string.IsNullOrEmpty(perfil.direccion_organizador) ? (object)DBNull.Value : perfil.direccion_organizador);
                    cmd.Parameters.AddWithValue("@telefono_contacto", string.IsNullOrEmpty(perfil.telefono_contacto) ? (object)DBNull.Value : perfil.telefono_contacto);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    return "Perfil de organizador creado correctamente";
                }
            }
            catch (Exception ex)
            {
                return $"Error al crear perfil: {ex.Message}";
            }
        }

        public string ActualizarPerfilOrganizador(int id_perfil_organizador, PerfilOrganizadorEditarDTO perfil)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarPerfilOrganizador", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_perfil_organizador", id_perfil_organizador);
                    cmd.Parameters.AddWithValue("@nombre_organizador",string.IsNullOrEmpty(perfil.nombre_organizador) ? (object)DBNull.Value : perfil.nombre_organizador);
                    cmd.Parameters.AddWithValue("@descripcion_organizador",string.IsNullOrEmpty(perfil.descripcion_organizador) ? (object)DBNull.Value : perfil.descripcion_organizador);
                    cmd.Parameters.AddWithValue("@sitio_web",string.IsNullOrEmpty(perfil.sitio_web) ?(object)DBNull.Value : perfil.sitio_web);
                    cmd.Parameters.AddWithValue("@facebook_url", string.IsNullOrEmpty(perfil.facebook_url) ?(object)DBNull.Value : perfil.facebook_url);
                    cmd.Parameters.AddWithValue("@instagram_url",   string.IsNullOrEmpty(perfil.instagram_url) ?(object)DBNull.Value : perfil.instagram_url);
                    cmd.Parameters.AddWithValue("@tiktok_url", string.IsNullOrEmpty(perfil.tiktok_url) ?(object)DBNull.Value : perfil.tiktok_url);

                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0
                        ? "Perfil de organizador actualizado correctamente"
                        : "No se encontró el perfil para actualizar";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar perfil: {ex.Message}";
            }
        }
    }
}