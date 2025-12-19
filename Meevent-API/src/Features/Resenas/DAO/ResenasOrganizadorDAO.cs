using Meevent_API.src.Core.Entities;
using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Resenas.DAO
{
    public class ResenasOrganizadorDAO : IResenasOrganizadorDAO
    {
        private readonly string? _cadenaConexion;

        public ResenasOrganizadorDAO(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("MeeventDB");
        }

        public async Task<IEnumerable<ResenaOrganizador>> GetAllByOrganizadorAsync(int perfilOrganizadorId)
        {
            List<ResenaOrganizador> listaResenas = new List<ResenaOrganizador>();

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Listar_Resenas_Organizador", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PerfilOrganizadorId", perfilOrganizadorId);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            listaResenas.Add(new ResenaOrganizador
                            {
                                IdResenaOrganizador = dr.GetInt32(0),
                                CalificacionResena = dr.GetInt32(1),
                                ComentarioResena = dr.GetString(2),
                                FechaCreacion = dr.GetDateTime(3),
                                ContadorUtilidad = dr.GetInt32(4),
                                CompradorVerificado = dr.GetBoolean(5),
                                PerfilOrganizadorId = dr.GetInt32(6),
                                UsuarioId = dr.GetInt32(7),
                                NombreCompleto = dr.GetString(8),
                                ImagenPerfilUrl = dr.IsDBNull(9) ? null : dr.GetString(9)
                            });
                        }
                    }
                }
            }

            return listaResenas;
        }

        public async Task<ResenaOrganizador> GetByIdAsync(int idResenaOrganizador)
        {
            ResenaOrganizador resena = null;

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Obtener_Resena_Organizador_Por_Id", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdResenaOrganizador", idResenaOrganizador);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            resena = new ResenaOrganizador
                            {
                                IdResenaOrganizador = dr.GetInt32(0),
                                CalificacionResena = dr.GetInt32(1),
                                ComentarioResena = dr.GetString(2),
                                FechaCreacion = dr.GetDateTime(3),
                                ContadorUtilidad = dr.GetInt32(4),
                                CompradorVerificado = dr.GetBoolean(5),
                                PerfilOrganizadorId = dr.GetInt32(6),
                                UsuarioId = dr.GetInt32(7),
                                NombreCompleto = dr.GetString(8),
                                ImagenPerfilUrl = dr.IsDBNull(9) ? null : dr.GetString(9)
                            };
                        }
                    }
                }
            }

            return resena;
        }

        public async Task<int> InsertAsync(ResenaOrganizador resena)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Insertar_Resena_Organizador", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CalificacionResena", resena.CalificacionResena);
                    cmd.Parameters.AddWithValue("@ComentarioResena", resena.ComentarioResena);
                    cmd.Parameters.AddWithValue("@CompradorVerificado", resena.CompradorVerificado);
                    cmd.Parameters.AddWithValue("@PerfilOrganizadorId", resena.PerfilOrganizadorId);
                    cmd.Parameters.AddWithValue("@UsuarioId", resena.UsuarioId);

                    await cn.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<int> UpdateAsync(int idResenaOrganizador,int idUsuario, int calificacion, string comentario)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Actualizar_Resena_Organizador", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdResenaOrganizador", idResenaOrganizador);
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuario);
                    cmd.Parameters.AddWithValue("@CalificacionResena", calificacion);
                    cmd.Parameters.AddWithValue("@ComentarioResena", comentario);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            return dr.GetInt32(0);
                        }
                    }
                }
            }

            return 0;
        }
        
        public async Task<int> IncrementarUtilidadAsync(int idResenaOrganizador)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Incrementar_Utilidad_Resena_Organizador", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdResenaOrganizador", idResenaOrganizador);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            return dr.GetInt32(0);
                        }
                    }
                }
            }

            return 0;
        }

        public async Task<int> DecrementarUtilidadAsync(int idResenaOrganizador)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Decrementar_Utilidad_Resena_Organizador", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdResenaOrganizador", idResenaOrganizador);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            return dr.GetInt32(0);
                        }
                    }
                }
            }

            return 0;
        }

        public async Task<ResenaOrganizador> VerificarResenaExistenteAsync(int perfilOrganizadorId, int usuarioId)
        {
            ResenaOrganizador resena = null;

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Verificar_Resena_Existente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PerfilOrganizadorId", perfilOrganizadorId);
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            resena = new ResenaOrganizador
                            {
                                IdResenaOrganizador = dr.GetInt32(0),
                                CalificacionResena = dr.GetInt32(1),
                                ComentarioResena = dr.GetString(2),
                                FechaCreacion = dr.GetDateTime(3)
                            };
                        }
                    }
                }
            }

            return resena;
        }
    
    }
}
