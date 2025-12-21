using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.SubcategoriasEvento.DAO
{
    public class SubcategoriaEventoDAO : ISubcategoriaEventoDAO
    {
        private readonly string _cadena;
        private readonly IConfiguration _configuration;

        public SubcategoriaEventoDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

  
        public IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasEvento()
        {
            List<SubcategoriaEventoDTO> lista = new();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SubcategoriaEventoDTO
                    {
                        IdSubcategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                        NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                        SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                        CategoriaEventoId = dr.GetInt32(dr.GetOrdinal("categoria_evento_id")),
                        NombreCategoria = dr.IsDBNull(dr.GetOrdinal("nombre_categoria"))
                            ? ""
                            : dr.GetString(dr.GetOrdinal("nombre_categoria"))
                    });
                }
                dr.Close();
            }

            return lista;
        }

 
        public IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasPorCategoria(int categoria_evento_id)
        {
            List<SubcategoriaEventoDTO> lista = new();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_listar_por_categoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoria_evento_id", categoria_evento_id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SubcategoriaEventoDTO
                    {
                        IdSubcategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                        NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                        SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                        CategoriaEventoId = dr.GetInt32(dr.GetOrdinal("categoria_evento_id"))
                    });
                }
                dr.Close();
            }

            return lista;
        }

        public SubcategoriaEventoDTO? GetSubcategoriaEventoPorId(int id_subcategoria_evento)
        {
            SubcategoriaEventoDTO? subcategoria = null;

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    subcategoria = new SubcategoriaEventoDTO
                    {
                        IdSubcategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_subcategoria_evento")),
                        NombreSubcategoria = dr.GetString(dr.GetOrdinal("nombre_subcategoria")),
                        SlugSubcategoria = dr.GetString(dr.GetOrdinal("slug_subcategoria")),
                        CategoriaEventoId = dr.GetInt32(dr.GetOrdinal("categoria_evento_id"))
                    };
                }
                dr.Close();
            }

            return subcategoria;
        }

        public string CrearSubcategoriaEvento(SubcategoriaEventoDTO subcategoria)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre_subcategoria", subcategoria.NombreSubcategoria);
                    cmd.Parameters.AddWithValue("@slug_subcategoria", subcategoria.SlugSubcategoria);
                    cmd.Parameters.AddWithValue("@categoria_evento_id", subcategoria.CategoriaEventoId);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    return "Subcategoría creada correctamente";
                }
            }
            catch (Exception ex)
            {
                return $"Error al crear subcategoría: {ex.Message}";
            }
        }

    
        public string ActualizarSubcategoriaEvento(int id_subcategoria_evento, SubcategoriaEventoDTO subcategoria)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);
                    cmd.Parameters.AddWithValue("@nombre_subcategoria", subcategoria.NombreSubcategoria);
                    cmd.Parameters.AddWithValue("@slug_subcategoria", subcategoria.SlugSubcategoria);
                    cmd.Parameters.AddWithValue("@categoria_evento_id", subcategoria.CategoriaEventoId);

                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0
                        ? "Subcategoría actualizada correctamente"
                        : "No se encontró la subcategoría";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar subcategoría: {ex.Message}";
            }
        }

 
        public string EliminarSubcategoriaEvento(int id_subcategoria_evento)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);

                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0
                        ? "Subcategoría eliminada correctamente"
                        : "No se encontró la subcategoría";
                }
            }
            catch (Exception ex)
            {
                return $"Error al eliminar subcategoría: {ex.Message}";
            }
        }
    }
}
