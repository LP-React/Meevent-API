using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.CategoriasEvento.DAO
{
    public class CategoriaEventoDAO : ICategoriaEventoDAO
    {
        private readonly string _cadena;

        public CategoriaEventoDAO(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("MeeventDB");
        }

        public IEnumerable<CategoriaEventoDTO> GetCategorias()
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
                        Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                    });
                }
            }

            return lista;
        }

        public IEnumerable<CategoriaEventoDTO> GetCategoriaPorId(int id_categoria_evento)
        {
            List<CategoriaEventoDTO> lista = new();

            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_categorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CategoriaEventoDTO
                    {
                        IdCategoriaEvento = dr.GetInt32(dr.GetOrdinal("id_categoria_evento")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("nombre_categoria")),
                        SlugCategoria = dr.GetString(dr.GetOrdinal("slug_categoria")),
                        Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                    });
                }
            }

            return lista;
        }

        public string InsertCategoria(CategoriaEventoCrearDTO reg)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre_categoria", reg.NombreCategoria);
                    cmd.Parameters.AddWithValue("@slug_categoria", reg.SlugCategoria);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return "Categoría registrada correctamente";
                }
            }
            catch (Exception ex)
            {
                return $"Error al crear: {ex.Message}";
            }
        }

        public string UpdateCategoria(int id_categoria_evento, CategoriaEventoEditarDTO reg)
        {
            try
            {
                var actual = GetCategoriaPorId(id_categoria_evento).FirstOrDefault();
                if (actual == null) return "No se encontró la categoría para actualizar";

                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);
                    cmd.Parameters.AddWithValue("@nombre_categoria", reg.NombreCategoria ?? actual.NombreCategoria);
                    cmd.Parameters.AddWithValue("@slug_categoria", reg.SlugCategoria ?? actual.SlugCategoria);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return "Datos actualizados correctamente";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar: {ex.Message}";
            }
        }

        public string CambiarEstado(int id_categoria_evento, bool nuevo_estado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_categorias_evento_cambiar_estado", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_categoria_evento", id_categoria_evento);
                    cmd.Parameters.AddWithValue("@estado", nuevo_estado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return nuevo_estado ? "Categoría activada" : "Categoría desactivada";
                }
            }
            catch (Exception ex)
            {
                return $"Error al cambiar estado: {ex.Message}";
            }
        }
    }
}
