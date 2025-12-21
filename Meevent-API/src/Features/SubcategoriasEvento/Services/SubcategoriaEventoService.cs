using Meevent_API.src.Features.SubcategoriasEvento.DAO;

namespace Meevent_API.src.Features.SubcategoriasEvento.Services
{
    public class SubcategoriaEventoService : ISubcategoriaEventoService
    {
        private readonly ISubcategoriaEventoDAO _dao;

        public SubcategoriaEventoService(ISubcategoriaEventoDAO dao)
        {
            _dao = dao;
        }

        public IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasEvento()
            => _dao.GetSubcategoriasEvento();

        public IEnumerable<SubcategoriaEventoDTO> GetSubcategoriasPorCategoria(int categoria_evento_id)
            => _dao.GetSubcategoriasPorCategoria(categoria_evento_id);

        public SubcategoriaEventoDTO? GetSubcategoriaEventoPorId(int id_subcategoria_evento)
            => _dao.GetSubcategoriaEventoPorId(id_subcategoria_evento);

        public string CrearSubcategoriaEvento(SubcategoriaEventoDTO subcategoria)
            => _dao.CrearSubcategoriaEvento(subcategoria);

        public string ActualizarSubcategoriaEvento(int id_subcategoria_evento, SubcategoriaEventoDTO subcategoria)
            => _dao.ActualizarSubcategoriaEvento(id_subcategoria_evento, subcategoria);

        public string EliminarSubcategoriaEvento(int id_subcategoria_evento)
            => _dao.EliminarSubcategoriaEvento(id_subcategoria_evento);
    }
}
