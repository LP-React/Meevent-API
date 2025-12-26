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

        public async Task<bool> InsertEventoAsync(Evento reg)
        {
            using SqlConnection cn = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("usp_InsertarEvento", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@titulo_evento", SqlDbType.NVarChar, 250).Value = reg.TituloEvento;
            cmd.Parameters.Add("@slug_evento", SqlDbType.NVarChar, 250).Value = reg.SlugEvento;
            cmd.Parameters.Add("@descripcion_evento", SqlDbType.NVarChar).Value = reg.DescripcionEvento;
            cmd.Parameters.Add("@descripcion_corta", SqlDbType.NVarChar, 500).Value =
                (object?)reg.DescripcionCorta ?? DBNull.Value;

            cmd.Parameters.Add("@fecha_inicio", SqlDbType.DateTimeOffset)
                .Value = DateTimeOffset.Parse(reg.FechaInicio);

            cmd.Parameters.Add("@fecha_fin", SqlDbType.DateTimeOffset)
                .Value = DateTimeOffset.Parse(reg.FechaFin);

            cmd.Parameters.Add("@zona_horaria", SqlDbType.NVarChar, 50).Value = reg.ZonaHoraria;
            cmd.Parameters.Add("@estado_evento", SqlDbType.NVarChar, 20).Value = reg.EstadoEvento;
            cmd.Parameters.Add("@capacidad_evento", SqlDbType.Int).Value = reg.CapacidadEvento;
            cmd.Parameters.Add("@evento_gratuito", SqlDbType.Bit).Value = reg.EventoGratuito;
            cmd.Parameters.Add("@evento_online", SqlDbType.Bit).Value = reg.EventoOnline;

            cmd.Parameters.Add("@imagen_portada_url", SqlDbType.NVarChar, 500).Value =
                (object?)reg.ImagenPortadaUrl ?? DBNull.Value;

            cmd.Parameters.Add("@perfil_organizador_id", SqlDbType.Int).Value = reg.PerfilOrganizadorId;
            cmd.Parameters.Add("@subcategoria_evento_id", SqlDbType.Int).Value = reg.SubcategoriaEventoId;

            cmd.Parameters.Add("@local_id", SqlDbType.Int).Value =
                reg.EventoOnline ? DBNull.Value : reg.LocalId!;

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true; ;
        }


        public async Task<string> updateEventoAsync(Evento reg)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarEvento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_evento", reg.IdEvento);
                    cmd.Parameters.AddWithValue("@titulo_evento", reg.TituloEvento);
                    cmd.Parameters.AddWithValue("@slug_evento", reg.SlugEvento);
                    cmd.Parameters.AddWithValue("@descripcion_evento", (object)reg.DescripcionEvento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@descripcion_corta", (object)reg.DescripcionCorta ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@fecha_inicio", DateTime.Parse(reg.FechaInicio));
                    cmd.Parameters.AddWithValue("@fecha_fin", DateTime.Parse(reg.FechaFin));
                    cmd.Parameters.AddWithValue("@zona_horaria", reg.ZonaHoraria);
                    cmd.Parameters.AddWithValue("@estado_evento", reg.EstadoEvento);
                    cmd.Parameters.AddWithValue("@capacidad_evento", reg.CapacidadEvento);
                    cmd.Parameters.AddWithValue("@evento_gratuito", reg.EventoGratuito);
                    cmd.Parameters.AddWithValue("@evento_online", reg.EventoOnline);
                    cmd.Parameters.AddWithValue("@imagen_portada_url", (object)reg.ImagenPortadaUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@subcategoria_evento_id", reg.SubcategoriaEventoId);
                    cmd.Parameters.AddWithValue("@local_id", reg.LocalId);

                    await cn.OpenAsync();

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0
                        ? "Evento actualizado correctamente."
                        : "No se realizaron cambios o el evento no existe.";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar: {ex.Message}";
            }
        }

        public async Task<EventoCompletoDTO?> GetEventoPorIdAsync(int idEvento)
        {
            EventoCompletoDTO? evento = null;

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
                                evento = MapearEventoCompleto(dr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener evento por ID", ex);
                }
            }

            return evento;
        }

        public async Task<EventoCompletoDTO?> GetEventoPorSlugAsync(string slugEvento)
        {
            EventoCompletoDTO? evento = null;

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
                        evento = MapearEventoCompleto(dr);
                    }
                }
            }
            return evento;
        }

        // Otros métodos relacionados con eventos pueden ser implementados aquí

        public async Task<IEnumerable<EventoCompletoDTO>> ListarEventosCompletosAsync(
            int? idOrganizador,
            int? idSubCategoria,
            int? idLocal,
            bool? eventoGratuito,
            bool? eventoOnline,
            string? estadoEvento,
            string? fchDesde,
            string? fchHasta)
        {
            List<EventoCompletoDTO> listaEventos = new List<EventoCompletoDTO>();

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarEventos", cn))

                {
                    cmd.Parameters.AddWithValue("@idOrganizador", (object?)idOrganizador ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@idSubCategoria", (object?)idSubCategoria ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@idLocal", (object?)idLocal ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@eventoGratuito", (object?)eventoGratuito ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@eventoOnline", (object?)eventoOnline ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@estadoEvento", (object?)estadoEvento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaDesde", string.IsNullOrEmpty(fchDesde) ? DBNull.Value : DateTime.Parse(fchDesde));
                    cmd.Parameters.AddWithValue("@fechaHasta", string.IsNullOrEmpty(fchHasta) ? DBNull.Value : DateTime.Parse(fchHasta));
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

        public async Task<bool> ValidarEventosAlMismoTiempoAsync(int perfilOrganizador,int idEvento,DateTime fchInicio, DateTime fchFin)
        {
            using var cn = new SqlConnection(_cadenaConexion);
            using var cmd = new SqlCommand("sp_existe_evento_solapado", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@perfil_organizador_id", SqlDbType.Int)
                .Value = perfilOrganizador;

            cmd.Parameters.Add("@fecha_inicio", SqlDbType.DateTimeOffset)
                .Value = new DateTimeOffset(fchInicio);

            cmd.Parameters.Add("@fecha_fin", SqlDbType.DateTimeOffset)
                .Value = new DateTimeOffset(fchFin);

            cmd.Parameters.Add("@id_evento_excluir", SqlDbType.Int)
                .Value = idEvento > 0 ? idEvento : DBNull.Value;

            await cn.OpenAsync();

            int existe = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            return existe == 1;
        }

        // Método privado para mapear el SqlDataReader a EventoCompletoDTO
        private EventoCompletoDTO MapearEventoCompleto(SqlDataReader dr)
        {
            var evento = new EventoCompletoDTO
            {
                IdEvento = dr.GetInt32(dr.GetOrdinal("id_evento")),
                TituloEvento = dr.GetString(dr.GetOrdinal("titulo_evento")),
                SlugEvento = dr.GetString(dr.GetOrdinal("slug_evento")),
                DescripcionEvento = dr.GetString(dr.GetOrdinal("descripcion_evento")),
                DescripcionCorta = dr.IsDBNull(dr.GetOrdinal("descripcion_corta")) ? null : dr.GetString(dr.GetOrdinal("descripcion_corta")),

                FechaInicio = dr.GetDateTimeOffset(dr.GetOrdinal("fecha_inicio")).DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                FechaFin = dr.GetDateTimeOffset(dr.GetOrdinal("fecha_fin")).DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),

                ZonaHoraria = dr.GetString(dr.GetOrdinal("zona_horaria")),
                EstadoEvento = dr.GetString(dr.GetOrdinal("estado_evento")),
                CapacidadEvento = dr.GetInt32(dr.GetOrdinal("capacidad_evento")),
                EventoGratuito = dr.GetBoolean(dr.GetOrdinal("evento_gratuito")),
                EventoOnline = dr.GetBoolean(dr.GetOrdinal("evento_online")),
                ImagenPortadaUrl = dr.IsDBNull(dr.GetOrdinal("imagen_portada_url")) ? null : dr.GetString(dr.GetOrdinal("imagen_portada_url")),
                FechaCreacion = dr.GetDateTime(dr.GetOrdinal("fecha_creacion")),
                FechaActualizacion = dr.GetDateTime(dr.GetOrdinal("fecha_actualizacion")),

                Organizador = new OrganizadorDTO
                {
                    IdPerfilOrganizador = dr.GetInt32(dr.GetOrdinal("id_perfil_organizador")),
                    NombreOrganizador = dr.GetString(dr.GetOrdinal("nombre_organizador")),
                    DescripcionOrganizador = dr.GetString(dr.GetOrdinal("descripcion_organizador")),
                    SitioWeb = dr.IsDBNull(dr.GetOrdinal("organizador_sitio_web")) ? null : dr.GetString(dr.GetOrdinal("organizador_sitio_web")),
                    LogoUrl = dr.IsDBNull(dr.GetOrdinal("logo_url")) ? null : dr.GetString(dr.GetOrdinal("logo_url")),
                    FacebookUrl = dr.IsDBNull(dr.GetOrdinal("organizador_facebook")) ? null : dr.GetString(dr.GetOrdinal("organizador_facebook")),
                    InstagramUrl = dr.IsDBNull(dr.GetOrdinal("organizador_instagram")) ? null : dr.GetString(dr.GetOrdinal("organizador_instagram")),
                    TiktokUrl = dr.IsDBNull(dr.GetOrdinal("organizador_tiktok")) ? null : dr.GetString(dr.GetOrdinal("organizador_tiktok")),
                    TwitterUrl = dr.IsDBNull(dr.GetOrdinal("organizador_twitter")) ? null : dr.GetString(dr.GetOrdinal("organizador_twitter")),
                    DireccionOrganizador = dr.IsDBNull(dr.GetOrdinal("direccion_organizador")) ? null : dr.GetString(dr.GetOrdinal("direccion_organizador")),
                    TelefonoContacto = dr.IsDBNull(dr.GetOrdinal("organizador_telefono")) ? null : dr.GetString(dr.GetOrdinal("organizador_telefono"))
                },

                Subcategoria = new SubcategoriaEventoDTO
                {
                    IdSubcategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                    NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                    SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                    Categoria = new CategoriaEventoDTO
                    {
                        IdCategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("nombre_categoria")),
                        SlugCategoria = dr.GetString(dr.GetOrdinal("slug_categoria")),
                        IconoUrl = dr.IsDBNull(dr.GetOrdinal("categoria_icono")) ? null : dr.GetString(dr.GetOrdinal("categoria_icono"))
                    }
                }
            };

            if (!dr.IsDBNull(dr.GetOrdinal("id_local")))
            {
                evento.Ubicacion = new UbicacionDTO
                {
                    IdLocal = dr.GetInt32(dr.GetOrdinal("id_local")),
                    NombreLocal = dr.GetString(dr.GetOrdinal("nombre_local")),
                    CapacidadLocal = dr.IsDBNull(dr.GetOrdinal("capacidad_local")) ? null : dr.GetInt32(dr.GetOrdinal("capacidad_local")),
                    DireccionLocal = dr.IsDBNull(dr.GetOrdinal("direccion_local")) ? null : dr.GetString(dr.GetOrdinal("direccion_local")),

                    IdCiudad = dr.IsDBNull(dr.GetOrdinal("id_ciudad")) ? null : dr.GetInt32(dr.GetOrdinal("id_ciudad")),
                    NombreCiudad = dr.IsDBNull(dr.GetOrdinal("nombre_ciudad")) ? null : dr.GetString(dr.GetOrdinal("nombre_ciudad")),

                    IdPais = dr.IsDBNull(dr.GetOrdinal("id_pais")) ? null : dr.GetInt32(dr.GetOrdinal("id_pais")),
                    NombrePais = dr.IsDBNull(dr.GetOrdinal("nombre_pais")) ? null : dr.GetString(dr.GetOrdinal("nombre_pais")),
                    CodigoISO = dr.IsDBNull(dr.GetOrdinal("codigo_iso")) ? null : dr.GetString(dr.GetOrdinal("codigo_iso")),

                    Latitud = dr.IsDBNull(dr.GetOrdinal("latitud")) ? null : dr.GetDecimal(dr.GetOrdinal("latitud")),
                    Longitud = dr.IsDBNull(dr.GetOrdinal("longitud")) ? null : dr.GetDecimal(dr.GetOrdinal("longitud"))
                };
            }
            else
            {
                evento.Ubicacion = null;
            }
            return evento;
        }

    }
}
