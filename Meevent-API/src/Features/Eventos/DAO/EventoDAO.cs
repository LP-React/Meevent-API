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
                    cmd.Parameters.AddWithValue("@idOrganizador", idOrganizador);
                    cmd.Parameters.AddWithValue("@idSubCategoria", idSubCategoria);
                    cmd.Parameters.AddWithValue("@idLocal", idLocal);
                    cmd.Parameters.AddWithValue("@eventoGratuito", eventoGratuito);
                    cmd.Parameters.AddWithValue("@eventoOnline", eventoOnline);
                    cmd.Parameters.AddWithValue("@estadoEvento", estadoEvento);
                    cmd.Parameters.AddWithValue("@fechaDesde", fchDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fchHasta);
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
            return new EventoCompletoDTO
            {
                // Datos básicos del evento
                IdEvento = dr.GetInt32(0),
                TituloEvento = dr.GetString(1),
                SlugEvento = dr.GetString(2),
                DescripcionEvento = dr.GetString(3),
                DescripcionCorta = dr.IsDBNull(4) ? null : dr.GetString(4),
                FechaInicio = dr.GetDateTimeOffset(5).DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                FechaFin = dr.GetDateTimeOffset(6).DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
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

                Ubicacion = new UbicacionDTO
                {
                    // Local
                    IdLocal = dr.GetInt32(33),
                    NombreLocal = dr.GetString(34),
                    CapacidadLocal = dr.GetInt32(35),
                    DireccionLocal = dr.IsDBNull(36) ? string.Empty : dr.GetString(36),
                    // Ciudad
                    IdCiudad = dr.GetInt32(37),
                    NombreCiudad = dr.GetString(38),
                    // Pais
                    IdPais = dr.GetInt32(39),
                    NombrePais = dr.GetString(40),
                    CodigoISO = dr.GetString(41),

                    Latitud = dr.GetDecimal(42),
                    Longitud = dr.GetDecimal(43)
                }
            };
        }

    }
}
