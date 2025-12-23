using gRpc_SubCategorias;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRpc_Meevent.Services
{
    public class SubCategoriaService : ServicioSubcategorias.ServicioSubcategoriasBase
    {
        private readonly string _cadena;

        public SubCategoriaService(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
        }

        public override Task<Subcategorias> GetAll(EmptySub request, ServerCallContext context)
        {
            var response = new Subcategorias();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        response.Items.Add(new Subcategoria
                        {
                            IdSubcategoria = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                            NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                            SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("categoria_evento_id")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                        });
                    }
                }
            }
            return Task.FromResult(response);
        }

        public override Task<Subcategorias> GetById(SubcategoriaRequest request, ServerCallContext context)
        {
            var response = new Subcategorias();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_subcategoria_evento", request.IdSubcategoria);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        response.Items.Add(new Subcategoria
                        {
                            IdSubcategoria = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                            NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                            SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("categoria_evento_id")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                        });
                    }
                }
            }
            return Task.FromResult(response);
        }
    }
}