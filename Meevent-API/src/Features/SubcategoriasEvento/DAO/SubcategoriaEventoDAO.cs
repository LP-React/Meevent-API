using Microsoft.Data.SqlClient;
using System.Data;

namespace Meevent_API.src.Features.SubcategoriasEvento.DAO
{
    public class SubcategoriaEventoDAO : ISubcategoriaEventoDAO
    {
        private readonly string _cadena;

        public SubcategoriaEventoDAO(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("MeeventDB");
        }
        public IEnumerable<SubcategoriaEventoDTO> GetSubcategorias()
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
                        Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                    });
                }
            }
            return lista;
        }

        public IEnumerable<SubcategoriaEventoDTO> GetSubcategoriaPorId(int id_subcategoria_evento)
        {
            List<SubcategoriaEventoDTO> lista = new();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);
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
                        Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
                    });
                }
            }
            return lista; ;
        }

        public string InsertSubcategoria(SubcategoriaEventoCrearDTO reg)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre_subcategoria", reg.NombreSubcategoria);
                    cmd.Parameters.AddWithValue("@slug_subcategoria", reg.SlugSubcategoria);
                    cmd.Parameters.AddWithValue("@categoria_evento_id", reg.CategoriaEventoId);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return "Subcategoría registrada correctamente";
                }
            }
            catch (Exception ex) { return $"Error al crear: {ex.Message}"; }
        }

        public string UpdateSubcategoria(int id_subcategoria_evento, SubcategoriaEventoEditarDTO reg)
        {
            try
            {
                var actual = GetSubcategoriaPorId(id_subcategoria_evento).FirstOrDefault();
                if (actual == null) return "No se encontró la subcategoría para actualizar";

                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);
                    cmd.Parameters.AddWithValue("@nombre_subcategoria", reg.NombreSubcategoria ?? actual.NombreSubcategoria);
                    cmd.Parameters.AddWithValue("@slug_subcategoria", reg.SlugSubcategoria ?? actual.SlugSubcategoria);
                    cmd.Parameters.AddWithValue("@categoria_evento_id", reg.CategoriaEventoId ?? actual.CategoriaEventoId);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return "Datos actualizados correctamente";
                }
            }
            catch (Exception ex) { return $"Error al actualizar: {ex.Message}"; }
        }

        public string CambiarEstado(int id_subcategoria_evento, bool nuevo_estado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_subcategorias_evento_cambiar_estado", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_subcategoria_evento", id_subcategoria_evento);
                    cmd.Parameters.AddWithValue("@estado", nuevo_estado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return nuevo_estado ? "Subcategoría activada" : "Subcategoría desactivada";
                }
            }
            catch (Exception ex) { return $"Error al cambiar estado: {ex.Message}"; }
        }
    }
}