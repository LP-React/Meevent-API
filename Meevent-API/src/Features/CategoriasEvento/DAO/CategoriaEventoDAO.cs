using Meevent_API.src.Features.CategoriasEvento;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.CategoriasEvento.DAO
{
    public class CategoriaEventoDAO : ICategoriaEventoDAO
    {
        private readonly string _cadena;
        private readonly IConfiguration _configuration;

        public CategoriaEventoDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        // LISTAR
        public IEnumerable<CategoriaEventoDTO> GetCategoriasEvento()
        {
            List<CategoriaEventoDTO> lista = new();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_categorias_evento_listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CategoriaEventoDTO
                    {
                        IdCategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("nombre_categoria")),
                        SlugCategoria = dr.GetString(dr.GetOrdinal("slug_categoria")),
                        IconoUrl = dr.IsDBNull(dr.GetOrdinal("icono_url"))
                                    ? ""
                                    : dr.GetString(dr.GetOrdinal("icono_url"))
                    });
                }
                dr.Close();
            }

            return lista;
        }

        // BUSCAR POR ID
        public CategoriaEventoDTO? GetCategoriaEventoPorId(int id_categoria_evento)
        {
            CategoriaEventoDTO? categoria = null;

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_categorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    categoria = new CategoriaEventoDTO
                    {
                        IdCategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("nombre_categoria")),
                        SlugCategoria = dr.GetString(dr.GetOrdinal("slug_categoria")),
                        IconoUrl = dr.IsDBNull(dr.GetOrdinal("icono_url"))
                                    ? ""
                                    : dr.GetString(dr.GetOrdinal("icono_url"))
                    };
                }
                dr.Close();
            }

            return categoria;
        }

        // CREAR
        public string CrearCategoriaEvento(CategoriaEventoDTO categoria)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre_categoria", categoria.NombreCategoria);
                    cmd.Parameters.AddWithValue("@slug_categoria", categoria.SlugCategoria);
                    cmd.Parameters.AddWithValue("@icono_url",
                        string.IsNullOrEmpty(categoria.IconoUrl)
                        ? (object)DBNull.Value
                        : categoria.IconoUrl);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    return "Categoría creada correctamente";
                }
            }
            catch (Exception ex)
            {
                return $"Error al crear categoría: {ex.Message}";
            }
        }

        // ACTUALIZAR
        public string ActualizarCategoriaEvento(int id_categoria_evento, CategoriaEventoDTO categoria)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);
                    cmd.Parameters.AddWithValue("@nombre_categoria", categoria.NombreCategoria);
                    cmd.Parameters.AddWithValue("@slug_categoria", categoria.SlugCategoria);
                    cmd.Parameters.AddWithValue("@icono_url",
                        string.IsNullOrEmpty(categoria.IconoUrl)
                        ? (object)DBNull.Value
                        : categoria.IconoUrl);

                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0
                        ? "Categoría actualizada correctamente"
                        : "No se encontró la categoría";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar categoría: {ex.Message}";
            }
        }

        // ELIMINAR
        public string EliminarCategoriaEvento(int id_categoria_evento)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);

                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0
                        ? "Categoría eliminada correctamente"
                        : "No se encontró la categoría";
                }
            }
            catch (Exception ex)
            {
                return $"Error al eliminar categoría: {ex.Message}";
            }
        }
    }
}
