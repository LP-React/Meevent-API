using Meevent_API.src.Features.CategoriasEvento.DAO;

namespace Meevent_API.src.Features.CategoriasEvento.Services
{
    public class CategoriaEventoService : ICategoriaEventoService
    {

        private readonly ICategoriaEventoDAO _dao;

        public CategoriaEventoService(ICategoriaEventoDAO dao)
        {
            _dao = dao;
        }

        public IEnumerable<CategoriaEventoDTO> GetCategoriasEvento()
        {
            return _dao.GetCategoriasEvento();
        }

        public CategoriaEventoDTO? GetCategoriaEventoPorId(int id_categoria_evento)
        {
            return _dao.GetCategoriaEventoPorId(id_categoria_evento);
        }

        public string CrearCategoriaEvento(CategoriaEventoDTO categoria)
        {
            return _dao.CrearCategoriaEvento(categoria);
        }

        public string ActualizarCategoriaEvento(int id_categoria_evento, CategoriaEventoDTO categoria)
        {
            return _dao.ActualizarCategoriaEvento(id_categoria_evento, categoria);
        }

        public string EliminarCategoriaEvento(int id_categoria_evento)
        {
            return _dao.EliminarCategoriaEvento(id_categoria_evento);
        }


    } 
}
