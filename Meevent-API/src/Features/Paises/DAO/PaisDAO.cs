using Meevent_API.src.Core.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.Paises.DAO
{
    public class PaisDAO : IPaisDAO
    {
        private readonly string _cadenaConexion;

        public PaisDAO(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Pais>> GetAllAsync()
        {
            List<Pais> listaPaises = new List<Pais>();

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Listar_Paises", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cn.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            listaPaises.Add(new Pais
                            {
                                IdPais = dr.GetInt32(0),
                                NombrePais = dr.GetString(1),
                            });
                        }
                    }
                }
            }

            return listaPaises;
        }
    
    }
}
