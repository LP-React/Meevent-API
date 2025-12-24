using Grpc.Core;
using gRpc_Meevent.Protos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRpc_Meevent.Services
{
    public class SubCategoriaService : ServiceSubcategoria.ServiceSubcategoriaBase
    {
        private readonly ILogger<CategoriaService> _logger;

        public SubCategoriaService(ILogger<CategoriaService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=MeeventDB; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true";

        List<SubcategoriaEvento> Lista(string nombre = null, bool? estado = null, int? idCategoria = null)
        {
            List<SubcategoriaEvento> temporal = new List<SubcategoriaEvento>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_listar_grpc", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", string.IsNullOrWhiteSpace(nombre) ? DBNull.Value : nombre);
                cmd.Parameters.AddWithValue("@estado", (object)estado ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id_categoria", (object)idCategoria ?? DBNull.Value);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        temporal.Add(new SubcategoriaEvento
                        {
                            IdSubcategoriaEvento = dr.GetInt32(0),
                            NombreSubcategoria = dr.GetString(1),
                            CategoriaEventoId = dr.GetInt32(3),
                            NombreCategoria = dr.GetString(4),
                            Estado = dr.GetBoolean(5)
                        });
                    }
                }
            }
            return temporal;
        }

        public override Task<SubcategoriaResponse> GetAll(EmptySubCategoria request, ServerCallContext context)
        {
            SubcategoriaResponse categorias = new SubcategoriaResponse();
            categorias.Items.AddRange(Lista());
            return Task.FromResult(categorias);
        }

        public override Task<SubcategoriaResponse> GetById(SubcategoriaRequest request, ServerCallContext context)
        {
            SubcategoriaResponse subcategoria = new SubcategoriaResponse();
            if (request.IdSubcategoriaEvento > 0)
            {
                subcategoria.Items.AddRange(
                    Lista().Where(c => c.IdSubcategoriaEvento == request.IdSubcategoriaEvento).ToArray()
                );
            }
            return Task.FromResult(subcategoria);
        }
        
        public override Task<SubcategoriaResponse> GetFiltrado(SubcategoriaFiltroRequest request, ServerCallContext context)
        {
            SubcategoriaResponse response = new SubcategoriaResponse();

            var listaFiltrada = Lista(
                request.HasNombre ? request.Nombre : null,
                request.HasEstado ? request.Estado : null,
                request.HasIdCategoria ? request.IdCategoria : null
            );

            response.Items.AddRange(listaFiltrada);

            return Task.FromResult(response);
        }
    }
}
