namespace Meevent_API.src.Features.CategoriasEvento.DAO
{
    public interface ICategoriaEventoDAO
    {
        IEnumerable<CategoriaEventoDTO> GetCategorias();
        IEnumerable<CategoriaEventoDTO> GetCategoriaPorId(int id_categoria_evento);
        string InsertCategoria(CategoriaEventoCrearDTO reg);
        string UpdateCategoria(int id_categoria_evento, CategoriaEventoEditarDTO reg);
        string CambiarEstado(int id_categoria_evento, bool nuevo_estado);
    }
}
