namespace Meevent_API.src.Features.CategoriasEvento.DAO
{
    public interface ICategoriaEventoDAO
    {
        IEnumerable<CategoriaEventoDTO> GetCategoriasEvento();
        CategoriaEventoDTO? GetCategoriaEventoPorId(int id_categoria_evento);
        string CrearCategoriaEvento(CategoriaEventoDTO categoria);
        string ActualizarCategoriaEvento(int id_categoria_evento, CategoriaEventoDTO categoria);
        string EliminarCategoriaEvento(int id_categoria_evento);
    }

}
