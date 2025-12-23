using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Locales.DAO
{
    public class LocalDAO : ILocalDAO
    {
        private readonly string? _cadenaConexion;
        public LocalDAO()
        {
            _cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").
                Build().GetConnectionString("MeeventDB");
        }

        public async Task<IEnumerable<LocalDTO>> ListarLocalesPorCiudadAsync(int idCiudad)
        {
            var lista = new List<LocalDTO>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                using (var cmd = new SqlCommand("usp_listar_locales_por_ciudades", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_ciudad", idCiudad);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new LocalDTO
                            {
                                IdLocal = reader.GetInt32(0),
                                NombreLocal = reader.GetString(1),
                                CapacidadLocal = reader.GetInt32(2),
                                DireccionLocal = reader.GetString(3),
                                CiudadId = reader.GetInt32(4),
                                SlugLocal = reader.GetString(5),
                                Latitud = reader.GetDecimal(6),
                                Longitud = reader.GetDecimal(7)
                            });
                        }
                    }
                }
            }
            return lista;
        }
    
    }
}
