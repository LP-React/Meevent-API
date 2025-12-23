using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Ciudades.DAO
{
    public class CiudadDAO : ICiudadDAO
    {
        private readonly string? _cadenaConexion;
        public CiudadDAO()
        {
            _cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").
                Build().GetConnectionString("MeeventDB");
        }

        public async Task<IEnumerable<CiudadDTO>> ListarCiudadesPorPaisAsync(int idPais)
        {
            var lista = new List<CiudadDTO>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                using (var cmd = new SqlCommand("usp_listar_ciudades_por_pais", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pais", idPais);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new CiudadDTO
                            {
                                IdCiudad = reader.GetInt32(0),
                                NombreCiudad = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
