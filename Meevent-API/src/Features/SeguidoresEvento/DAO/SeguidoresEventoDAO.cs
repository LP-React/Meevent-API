using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.SeguidoresEvento.DAO
{
    public class SeguidoresEventoDAO : ISeguidoresEventoDAO
    {
        private readonly string? _cadenaConexion;
        public SeguidoresEventoDAO()
        {
            _cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").
                Build().GetConnectionString("MeeventDB");
        }

        public async Task<List<EventoSeguidoDTO>> ListarEventosSeguidosPorUsuarioAsync(int idUsuario)
        {
            var lista = new List<EventoSeguidoDTO>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                using (var cmd = new SqlCommand("usp_listar_eventos_seguidos_por_usuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new EventoSeguidoDTO
                            {
                                // --- DATOS DE SEGUIMIENTO ---
                                IdSeguidorEvento = reader.GetInt32(0),
                                FechaSeguimiento = reader.GetDateTime(1),
                                UsuarioId = reader.GetInt32(2),

                                // --- DATOS DEL EVENTO ---
                                IdEvento = reader.GetInt32(3),
                                TituloEvento = reader.GetString(4),
                                SlugEvento = reader.GetString(5),
                                DescripcionEvento = reader.IsDBNull(6) ? null : reader.GetString(6),
                                DescripcionCorta = reader.IsDBNull(7) ? null : reader.GetString(7),
                                FechaInicio = reader.GetDateTimeOffset(8).DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                FechaFin = reader.GetDateTimeOffset(9).DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                ZonaHoraria = reader.GetString(10),
                                EstadoEvento = reader.GetString(11),
                                CapacidadEvento = reader.GetInt32(12),
                                EventoGratuito = reader.GetBoolean(13),
                                EventoOnline = reader.GetBoolean(14),
                                ImagenPortadaUrl = reader.IsDBNull(15) ? null : reader.GetString(15),

                                // --- DATOS DEL ORGANIZADOR ---
                                IdPerfilOrganizador = reader.GetInt32(18),
                                NombreOrganizador = reader.GetString(19),
                                // 20: desc_org, 21: sitio_web
                                LogoUrl = reader.IsDBNull(22) ? null : reader.GetString(22),
                                // 23-28: Redes sociales y contacto organizador

                                // --- DATOS DE SUB-CATEGORIA ---
                                IdSubcategoria = reader.GetInt32(29),
                                NombreSubcategoria = reader.GetString(30),
                                // 31: slug_sub

                                // --- DATOS DE CATEGORIA ---
                                IdCategoria = reader.GetInt32(32),
                                NombreCategoria = reader.GetString(33),
                                // 34: slug_cat
                                CategoriaIcono = reader.IsDBNull(35) ? null : reader.GetString(35),

                                // --- DATOS DEL LOCAL ---
                                IdLocal = reader.GetInt32(36),
                                NombreLocal = reader.GetString(37),
                                DireccionLocal = reader.GetString(39),

                                // --- DATOS CIUDAD / PAÍS ---
                                NombreCiudad = reader.GetString(41),
                                NombrePais = reader.GetString(43),

                                // --- COORDENADAS ---
                                Latitud = reader.GetDecimal(45),
                                Longitud = reader.GetDecimal(46)
                            });
                        }
                    }
                }
            }
            return lista;
        }

    }
}
