using Meevent_API.src.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.PerfilesArtistas.DAO
{
    public class PerfilArtistaDAO : IPerfilArtistaDAO
    {
        private readonly string _cadena;
        private readonly IConfiguration _configuration;

        public PerfilArtistaDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        public async Task<PerfilArtista> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                var query = "SELECT * FROM perfiles_artista WHERE usuario_id = @userId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = usuarioId;

                    await conn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (dr != null && await dr.ReadAsync())
                        {
                            return new PerfilArtista
                            {
                                IdPerfilArtista = dr.GetInt32(dr.GetOrdinal("id_perfil_artista")),
                                NombreArtistico = dr["nombre_artistico"].ToString() ?? "",
                                BiografiaArtista = dr["biografia_artista"].ToString() ?? "",
                                GeneroMusical = dr["genero_musical"].ToString() ?? "",
                                SitioWeb = dr["sitio_web"] as string,
                                FacebookUrl = dr["facebook_url"] as string,
                                InstagramUrl = dr["instagram_url"] as string,
                                TiktokUrl = dr["tiktok_url"] as string,
                                FechaCreacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_creacion"))),
                                FechaActualizacion = DateOnly.FromDateTime(dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion"))),
                                UsuarioId = dr.GetInt32(dr.GetOrdinal("usuario_id"))
                            };
                        }
                    }
                }
            } 
            return null; 
        }
    }
}