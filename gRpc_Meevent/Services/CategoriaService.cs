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

        List<CategoriaEvento> Lista(string nombre = null, bool? estado = null)
        {
            List<CategoriaEvento> temporal = new List<CategoriaEvento>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_ListarCategoriasEventoFiltrado_grpc", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", string.IsNullOrWhiteSpace(nombre) ? DBNull.Value : nombre);
                cmd.Parameters.AddWithValue("@Estado", (object)estado ?? DBNull.Value);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new CategoriaEvento
                    {
                        IdCategoriaEvento = dr.GetInt32(0),
                        NombreCategoria = dr.GetString(1),
                        Estado = dr.GetBoolean(3)
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

        public override Task<CategoriaResponse> GetFiltrado(FiltroRequest request, ServerCallContext context)
        {
            CategoriaResponse response = new CategoriaResponse();

            var listaFiltrada = Lista(request.Nombre, request.HasEstado ? request.Estado : null);

            response.Items.AddRange(listaFiltrada);

            return Task.FromResult(response);
        }
    }
}
