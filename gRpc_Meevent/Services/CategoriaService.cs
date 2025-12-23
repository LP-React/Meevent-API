using gRpc_Categorias;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRpc_Meevent.Services
{
    public class CategoriaService : ServicioCategorias.ServicioCategoriasBase
    {
        private readonly string _cadena;

        public CategoriaService(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
        }

        public override Task<Categorias> GetAll(EmptyCat request, ServerCallContext context)
        {
            var response = new Categorias();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_categorias_evento_listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        response.Items.Add(new Categoria
                        {
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                            NombreCategoria = dr.IsDBNull(dr.GetOrdinal("nombre_categoria")) ? "" : dr.GetString(dr.GetOrdinal("nombre_categoria")),
                            SlugCategoria = dr.IsDBNull(dr.GetOrdinal("slug_categoria")) ? "" : dr.GetString(dr.GetOrdinal("slug_categoria")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                        });
                    }
                }
            }
            return Task.FromResult(response);
        }

        public override Task<Categorias> GetById(CategoriaRequest request, ServerCallContext context)
        {
            var response = new Categorias();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_categorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_categoria_evento", request.IdCategoria);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        response.Items.Add(new Categoria
                        {
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                            NombreCategoria = dr.IsDBNull(dr.GetOrdinal("nombre_categoria")) ? "" : dr.GetString(dr.GetOrdinal("nombre_categoria")),
                            SlugCategoria = dr.IsDBNull(dr.GetOrdinal("slug_categoria")) ? "" : dr.GetString(dr.GetOrdinal("slug_categoria")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                        });
                    }
                }
            }
            return Task.FromResult(response);
        }
    }
}