using Meevent_API.src.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
namespace Meevent_API.src.Features.Eventos.DAO
{
    public class EventoDAO : IEventoDAO
    {
        private readonly string? _cadenaConexion;
        public EventoDAO()
        {
            _cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").
                Build().GetConnectionString("MeeventDB");
        }
        public async Task<List<EventoListadoDTO>> ListarEventosAsync()
        {
            var eventos = new List<EventoListadoDTO>();

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand("usp_ListarEventos", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        eventos.Add(new EventoListadoDTO
                        {
                            IdEvento = dr.GetInt32(dr.GetOrdinal("id_evento")),
                            TituloEvento = dr.GetString(dr.GetOrdinal("titulo_evento")),
                            SlugEvento = dr.GetString(dr.GetOrdinal("slug_evento")),
                            DescripcionCorta = dr.IsDBNull(dr.GetOrdinal("descripcion_corta"))
                                ? null
                                : dr.GetString(dr.GetOrdinal("descripcion_corta")),

                            FechaInicio = dr.GetDateTimeOffset(5).ToString("yyyy-MM-dd HH:mm:ss"),
                            FechaFin = dr.GetDateTimeOffset(6).ToString("yyyy-MM-dd HH:mm:ss"),

                            ZonaHoraria = dr["zona_horaria"].ToString()!,
                            EstadoEvento = dr["estado_evento"].ToString()!,
                            EventoGratuito = Convert.ToBoolean(dr["evento_gratuito"]),
                            EventoOnline = Convert.ToBoolean(dr["evento_online"]),

                            ImagenPortadaUrl = dr["imagen_portada_url"] == DBNull.Value
                                ? null
                                : dr["imagen_portada_url"].ToString(),
                            NombreOrganizador = dr["nombre_organizador"].ToString()!,
                            NombreCategoria = dr["nombre_categoria"].ToString()!,
                            NombreSubcategoria = dr["nombre_subcategoria"].ToString()!,
                            NombreLocal = dr["nombre_local"] == DBNull.Value
                                ? null
                                : dr["nombre_local"].ToString()
                        });
                    }
                }
            }

            return eventos;
        }

        public async Task<string> insertEventoAsync(Evento reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    using SqlCommand cmd = new SqlCommand("usp_InsertarEvento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@titulo_evento", reg.TituloEvento);
                    cmd.Parameters.AddWithValue("@slug_evento", reg.SlugEvento);
                    cmd.Parameters.AddWithValue("@descripcion_evento", reg.DescripcionEvento);
                    cmd.Parameters.AddWithValue("@descripcion_corta", reg.DescripcionCorta ?? (object)DBNull.Value);
                    cmd.Parameters.Add("@fecha_inicio", SqlDbType.DateTimeOffset).Value = DateTimeOffset.Parse(reg.FechaInicio);
                    cmd.Parameters.Add("@fecha_fin", SqlDbType.DateTimeOffset).Value = DateTimeOffset.Parse(reg.FechaFin);
                    cmd.Parameters.AddWithValue("@zona_horaria", reg.ZonaHoraria);
                    cmd.Parameters.AddWithValue("@estado_evento", reg.EstadoEvento);
                    cmd.Parameters.AddWithValue("@capacidad_evento", reg.CapacidadEvento);
                    cmd.Parameters.AddWithValue("@evento_gratuito", reg.EventoGratuito);
                    cmd.Parameters.AddWithValue("@evento_online", reg.EventoOnline);
                    cmd.Parameters.AddWithValue("@imagen_portada_url", reg.ImagenPortadaUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@perfil_organizador_id", reg.PerfilOrganizadorId);
                    cmd.Parameters.AddWithValue("@subcategoria_evento_id", reg.SubcategoriaEventoId);
                    cmd.Parameters.AddWithValue("@local_id", reg.LocalId);

                    await cn.OpenAsync();

                    int i = await cmd.ExecuteNonQueryAsync();

                    mensaje = $"Se insertó correctamente el evento. Filas afectadas: {i}";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }

            return mensaje;
        }

        public async Task<string> updateEventoAsync(Evento reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    using SqlCommand cmd = new SqlCommand("usp_ActualizarEvento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros para actualizar (sin PerfilOrganizadorId)
                    cmd.Parameters.AddWithValue("@id_evento", reg.IdEvento);
                    cmd.Parameters.AddWithValue("@titulo_evento", reg.TituloEvento);
                    cmd.Parameters.AddWithValue("@slug_evento", reg.SlugEvento);
                    cmd.Parameters.AddWithValue("@descripcion_evento", reg.DescripcionEvento);
                    cmd.Parameters.AddWithValue("@descripcion_corta", reg.DescripcionCorta ?? (object)DBNull.Value);
                    cmd.Parameters.Add("@fecha_inicio", SqlDbType.DateTimeOffset).Value = DateTimeOffset.Parse(reg.FechaInicio);
                    cmd.Parameters.Add("@fecha_fin", SqlDbType.DateTimeOffset).Value = DateTimeOffset.Parse(reg.FechaFin);
                    cmd.Parameters.AddWithValue("@zona_horaria", reg.ZonaHoraria);
                    cmd.Parameters.AddWithValue("@estado_evento", reg.EstadoEvento);
                    cmd.Parameters.AddWithValue("@capacidad_evento", reg.CapacidadEvento);
                    cmd.Parameters.AddWithValue("@evento_gratuito", reg.EventoGratuito);
                    cmd.Parameters.AddWithValue("@evento_online", reg.EventoOnline);
                    cmd.Parameters.AddWithValue("@imagen_portada_url", reg.ImagenPortadaUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@subcategoria_evento_id", reg.SubcategoriaEventoId);
                    cmd.Parameters.AddWithValue("@local_id", reg.LocalId);

                    await cn.OpenAsync();
                    int i = await cmd.ExecuteNonQueryAsync();

                    mensaje = $"Se ha actualizado {i} evento(s)";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }

            return mensaje;
        }



        public async Task<Evento?> GetEventoPorIdAsync(int idEvento)
        {
            Evento? evento = null;

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    await cn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("usp_ObtenerEventoPorId", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_evento", idEvento);

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            if (await dr.ReadAsync())
                            {
                                evento = new Evento
                                {
                                    IdEvento = Convert.ToInt32(dr["id_evento"]),
                                    TituloEvento = dr["titulo_evento"].ToString()!,
                                    SlugEvento = dr["slug_evento"].ToString()!,
                                    DescripcionEvento = dr["descripcion_evento"].ToString()!,
                                    DescripcionCorta = dr["descripcion_corta"] == DBNull.Value
                                        ? null
                                        : dr["descripcion_corta"].ToString(),
                                    FechaInicio = dr.GetDateTimeOffset(dr.GetOrdinal("fecha_inicio")).ToString("yyyy-MM-dd HH:mm:ss"),
                                    FechaFin = dr.GetDateTimeOffset(dr.GetOrdinal("fecha_fin")).ToString("yyyy-MM-dd HH:mm:ss"),
                                    ZonaHoraria = dr["zona_horaria"].ToString()!,
                                    EstadoEvento = dr["estado_evento"].ToString()!,
                                    CapacidadEvento = Convert.ToInt32(dr["capacidad_evento"]),
                                    EventoGratuito = Convert.ToBoolean(dr["evento_gratuito"]),
                                    EventoOnline = Convert.ToBoolean(dr["evento_online"]),
                                    ImagenPortadaUrl = dr["imagen_portada_url"] == DBNull.Value
                                        ? null
                                        : dr["imagen_portada_url"].ToString(),
                                    PerfilOrganizadorId = Convert.ToInt32(dr["perfil_organizador_id"]),
                                    SubcategoriaEventoId = Convert.ToInt32(dr["subcategoria_evento_id"]),
                                    LocalId = Convert.ToInt32(dr["local_id"])
                                };
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            return evento;
        }



        public async Task<Evento?> GetEventoPorSlugAsync(string slugEvento)
        {
            Evento? evento = null;

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand("usp_ObtenerEventoPorSlug", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@slug_evento", slugEvento);

                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await dr.ReadAsync())
                    {
                        evento = new Evento
                        {
                            IdEvento = dr.GetInt32(0),
                            TituloEvento = dr.GetString(1),
                            SlugEvento = dr.GetString(2),
                            DescripcionEvento = dr.GetString(3),
                            DescripcionCorta = dr.IsDBNull(4) ? null : dr.GetString(4),
                            FechaInicio = dr.GetDateTimeOffset(5).ToString("yyyy-MM-dd HH:mm:ss"),
                            FechaFin = dr.GetDateTimeOffset(6).ToString("yyyy-MM-dd HH:mm:ss"),
                            ZonaHoraria = dr.GetString(7),
                            EstadoEvento = dr.GetString(8),
                            CapacidadEvento = dr.GetInt32(9),
                            EventoGratuito = dr.GetBoolean(10),
                            EventoOnline = dr.GetBoolean(11),
                            ImagenPortadaUrl = dr.IsDBNull(12) ? null : dr.GetString(12),
                            FechaCreacion = dr.GetDateTime(13).ToString("yyyy-MM-ddTHH:mm:ss"),
                            FechaActualizacion = dr.GetDateTime(14).ToString("yyyy-MM-ddTHH:mm:ss"),
                            PerfilOrganizadorId = dr.GetInt32(15),
                            SubcategoriaEventoId = dr.GetInt32(16),
                            LocalId = dr.GetInt32(17)
                        };
                    }
                }
            }
            return evento;
        }

        public Task<string> deleteEventoAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Otros métodos relacionados con eventos pueden ser implementados aquí

        public async Task<IEnumerable<EventoCompletoDTO>> ListarEventosCompletosAsync(int? idOrganizador,int? idSubCategoria)
        {
            List<EventoCompletoDTO> listaEventos = new List<EventoCompletoDTO>();

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarEventos", cn))

                {
                    cmd.Parameters.AddWithValue("@idOrganizador", idOrganizador);
                    cmd.Parameters.AddWithValue("@idSubCategoria", idSubCategoria);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            listaEventos.Add(MapearEventoCompleto(dr));
                        }
                    }
                }
            }

            return listaEventos;
        }

        // Método privado para mapear el SqlDataReader a EventoCompletoDTO
        private EventoCompletoDTO MapearEventoCompleto(SqlDataReader dr)
        {
            return new EventoCompletoDTO
            {
                // Datos básicos del evento
                IdEvento = dr.GetInt32(0),
                TituloEvento = dr.GetString(1),
                SlugEvento = dr.GetString(2),
                DescripcionEvento = dr.GetString(3),
                DescripcionCorta = dr.IsDBNull(4) ? null : dr.GetString(4),
                FechaInicio = dr.GetDateTimeOffset(5).DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                FechaFin = dr.GetDateTimeOffset(6).DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ZonaHoraria = dr.GetString(7),
                EstadoEvento = dr.GetString(8),
                CapacidadEvento = dr.GetInt32(9),
                EventoGratuito = dr.GetBoolean(10),
                EventoOnline = dr.GetBoolean(11),
                ImagenPortadaUrl = dr.IsDBNull(12) ? null : dr.GetString(12),
                FechaCreacion = dr.GetDateTime(13),
                FechaActualizacion = dr.GetDateTime(14),

                // Organizador completo
                Organizador = new OrganizadorDTO
                {
                    IdPerfilOrganizador = dr.GetInt32(15),
                    NombreOrganizador = dr.GetString(16),
                    DescripcionOrganizador = dr.GetString(17),
                    SitioWeb = dr.IsDBNull(18) ? null : dr.GetString(18),
                    LogoUrl = dr.IsDBNull(19) ? null : dr.GetString(19),
                    FacebookUrl = dr.IsDBNull(20) ? null : dr.GetString(20),
                    InstagramUrl = dr.IsDBNull(21) ? null : dr.GetString(21),
                    TiktokUrl = dr.IsDBNull(22) ? null : dr.GetString(22),
                    TwitterUrl = dr.IsDBNull(23) ? null : dr.GetString(23),
                    DireccionOrganizador = dr.IsDBNull(24) ? null : dr.GetString(24),
                    TelefonoContacto = dr.IsDBNull(25) ? null : dr.GetString(25)
                },

                // Subcategoría con categoría anidada
                Subcategoria = new SubcategoriaEventoDTO
                {
                    IdSubcategoriaEvento = dr.GetInt32(26),
                    NombreSubcategoria = dr.GetString(27),
                    SlugSubcategoria = dr.GetString(28),
                    Categoria = new CategoriaEventoDTO
                    {
                        IdCategoriaEvento = dr.GetInt32(29),
                        NombreCategoria = dr.GetString(30),
                        SlugCategoria = dr.GetString(31),
                        IconoUrl = dr.IsDBNull(32) ? null : dr.GetString(32)
                    }
                },

                // Local con ciudad y país anidados
                Local = new LocalDTO
                {
                    IdLocal = dr.GetInt32(33),
                    NombreLocal = dr.GetString(34),
                    CapacidadLocal = dr.GetInt32(35),
                    DireccionLocal = dr.GetString(36),
                    Ciudad = new CiudadDTO
                    {
                        IdCiudad = dr.GetInt32(37),
                        NombreCiudad = dr.GetString(38),
                        Pais = new PaisDTO
                        {
                            IdPais = dr.GetInt32(39),
                            NombrePais = dr.GetString(40),
                            CodigoISO = dr.GetString(41)
                        }
                    }
                }
            };
        }

    }
}
