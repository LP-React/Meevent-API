namespace Meevent_API.src.Features.SubcategoriasEvento.DAO
{
    public interface ISubcategoriaEventoDAO
    {
        IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasEvento();
        IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasPorCategoria(int categoria_evento_id);
        SubcategoriaEventoDTO? GetSubcategoriaEventoPorId(int id_subcategoria_evento);
        string CrearSubcategoriaEvento(SubcategoriaEventoDTO subcategoria);
        string ActualizarSubcategoriaEvento(int id_subcategoria_evento, SubcategoriaEventoDTO subcategoria);
        string EliminarSubcategoriaEvento(int id_subcategoria_evento);
    }
}
