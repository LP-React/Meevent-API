namespace Meevent_API.src.Features.SubcategoriasEvento.DAO
{
    public interface ISubcategoriaEventoDAO
    {
        IEnumerable<SubcategoriaEventoDTO> GetSubcategorias();
        IEnumerable<SubcategoriaEventoDTO> GetSubcategoriaPorId(int id_subcategoria_evento);
        string InsertSubcategoria(SubcategoriaEventoCrearDTO reg);
        string UpdateSubcategoria(int id_subcategoria_evento, SubcategoriaEventoEditarDTO reg);
        string CambiarEstado(int id_subcategoria_evento, bool nuevo_estado);
    }
}
 