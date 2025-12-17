using Meevent_API.src.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
namespace Meevent_API.src.Features.Eventos.DAO
{
    public class EventoDAO : IEventoDAO
    {
        private readonly string? _cadena;
        public EventoDAO(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("MeeventDB");
        }
        public async Task<IEnumerable<Evento>> GetAllAsync()
        {
            List<Evento> listaEventos = new List<Evento>();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarEventos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            listaEventos.Add(new Evento
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
                            });
                        }
                    }
                }
            }
            return listaEventos;
        }
        public string insertEvento(Evento reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_InsertarEvento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@titulo_evento", reg.TituloEvento);
                    cmd.Parameters.AddWithValue("@slug_evento", reg.SlugEvento);
                    cmd.Parameters.AddWithValue("@descripcion_evento", reg.DescripcionEvento);
                    cmd.Parameters.AddWithValue("@descripcion_corta", reg.DescripcionCorta ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fecha_inicio", reg.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", reg.FechaFin);
                    cmd.Parameters.AddWithValue("@zona_horaria", reg.ZonaHoraria);
                    cmd.Parameters.AddWithValue("@estado_evento", reg.EstadoEvento);
                    cmd.Parameters.AddWithValue("@capacidad_evento", reg.CapacidadEvento);
                    cmd.Parameters.AddWithValue("@evento_gratuito", reg.EventoGratuito);
                    cmd.Parameters.AddWithValue("@evento_online", reg.EventoOnline);
                    cmd.Parameters.AddWithValue("@imagen_portada_url", reg.ImagenPortadaUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@perfil_organizador_id", reg.PerfilOrganizadorId);
                    cmd.Parameters.AddWithValue("@subcategoria_evento_id", reg.SubcategoriaEventoId);
                    cmd.Parameters.AddWithValue("@local_id", reg.LocalId);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se insertó correctamente el evento. Filas afectadas: {i}";
                }
                catch (Exception ex) { mensaje  = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }
        public string updateEvento(Evento reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarEvento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_evento", reg.IdEvento);
                    cmd.Parameters.AddWithValue("@titulo_evento", reg.TituloEvento);
                    cmd.Parameters.AddWithValue("@slug_evento", reg.SlugEvento);
                    cmd.Parameters.AddWithValue("@descripcion_evento", reg.DescripcionEvento);
                    cmd.Parameters.AddWithValue("@descripcion_corta", reg.DescripcionCorta ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fecha_inicio", reg.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", reg.FechaFin);
                    cmd.Parameters.AddWithValue("@zona_horaria", reg.ZonaHoraria);
                    cmd.Parameters.AddWithValue("@estado_evento", reg.EstadoEvento);
                    cmd.Parameters.AddWithValue("@capacidad_evento", reg.CapacidadEvento);
                    cmd.Parameters.AddWithValue("@evento_gratuito", reg.EventoGratuito);
                    cmd.Parameters.AddWithValue("@evento_online", reg.EventoOnline);
                    cmd.Parameters.AddWithValue("@imagen_portada_url", reg.ImagenPortadaUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@perfil_organizador_id", reg.PerfilOrganizadorId);
                    cmd.Parameters.AddWithValue("@subcategoria_evento_id", reg.SubcategoriaEventoId);
                    cmd.Parameters.AddWithValue("@local_id", reg.LocalId);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} eventos";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }
        public Evento GetEvento(int id)
        {
            return GetAllAsync().Result.FirstOrDefault(e => e.IdEvento == id);
        }
        public string deleteEvento(int id)
        {
            throw new NotImplementedException();
        }

        public Evento GetEventoPorSlug(string slugEvento)
        {
            Evento? evento = null;

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerEventoPorSlug", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@slug_evento", slugEvento);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
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
            }

            return evento;
        }
    }
}
