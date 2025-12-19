using gRcp_Paises;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRcp_Meevent.Services
{
    public class PaisService : ServicioPaises.ServicioPaisesBase
    {
        private readonly ILogger<PaisService> _logger;

        public PaisService(ILogger<PaisService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=MeeventDB; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true";

        List<Pais> Lista()
        {
            List<Pais> temporal = new List<Pais>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_Listar_Paises", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Pais
                    {
                        IdPais = dr.GetInt32(0),
                        NombrePais = dr.GetString(1),
                    });
                }
            }
            return temporal;
        }

        public override Task<Paises> GetAll(Empty request, ServerCallContext context)
        {
            Paises response = new Paises();
            response.Items.AddRange(Lista());
            return Task.FromResult(response);
        }
        public override Task<Paises> GetById(PaisRequest request, ServerCallContext context)
        {
            Paises response = new Paises();

            if (request.IdPais > 0)
            {
                response.Items.AddRange(
                    Lista().Where(p => p.IdPais == request.IdPais).ToArray()
                );
            }
            else
            {
                response.Items.AddRange(Lista());
            }

            return Task.FromResult(response);
        }

    }
}
