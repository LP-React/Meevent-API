using Grpc.Core;
using gRpc_Meevent.Protos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRpc_Meevent.Services
{
    public class CategoriaService : ServiceCategoria.ServiceCategoriaBase
    {
        private readonly ILogger<CategoriaService> _logger;

        public CategoriaService(ILogger<CategoriaService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=MeeventDB; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true";
    
        List<CategoriaEvento> Lista()
        {
            List<CategoriaEvento> temporal = new List<CategoriaEvento>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarCategoriasEvento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new CategoriaEvento
                    {
                        IdCategoriaEvento = dr.GetInt32(0),
                        NombreCategoria = dr.GetString(1),
                        Estado = dr.GetBoolean(4),

                    });
                }
            }
            return temporal;
        }
    
        public override Task<CategoriaResponse> GetAll(Empty request, ServerCallContext context)
        {
            CategoriaResponse response = new CategoriaResponse();
            response.Items.AddRange(Lista());
            return Task.FromResult(response);
        }

        public override Task<CategoriaResponse> GetById(CategoriaRequest request, ServerCallContext context)
        {
            CategoriaResponse categoria = new CategoriaResponse();

            if(request.IdCategoriaEvento > 0)
            {
                categoria.Items.AddRange(
                    Lista().Where(c => c.IdCategoriaEvento == request.IdCategoriaEvento).ToArray()
                );
            }
            else
            {
                categoria.Items.AddRange(Lista());
            }

            return Task.FromResult(categoria);
        }
    
    }
}
