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

        public async Task<CategoriaEventoListResponseDTO> ObtenerCategoriasAsync()
        {
            var lista = await Task.Run(() => _dao.GetCategorias());

            return new CategoriaEventoListResponseDTO
            {
                Exitoso = true,
                Mensaje = "Listado obtenido correctamente",
                TotalCategoriasEvento = lista.Count(),
                CategoriasEvento = lista
            };
        }

        public async Task<CategoriaEventoDTO> ObtenerCategoriaPorIdAsync(int id_categoria_evento)
        {
            var lista = await Task.Run(() => _dao.GetCategoriaPorId(id_categoria_evento));
            return lista.FirstOrDefault();
        }

        public async Task<string> RegistrarCategoriaAsync(CategoriaEventoCrearDTO registro)
        {
            return await Task.Run(() => _dao.InsertCategoria(registro));
        }

        public async Task<CategoriaEventoOperacionResponseDTO> ActualizarCategoriaAsync(
            int id_categoria_evento,
            CategoriaEventoEditarDTO categoria)
        {
            string resultado = await Task.Run(() =>
                _dao.UpdateCategoria(id_categoria_evento, categoria));

            var categoriaActualizada = await ObtenerCategoriaPorIdAsync(id_categoria_evento);

            return new CategoriaEventoOperacionResponseDTO
            {
                Exitoso = !resultado.Contains("Error") && categoriaActualizada != null,
                Mensaje = resultado,
                Categoria = categoriaActualizada != null
                    ? new CategoriaEventoDetalleDTO
                    {
                        IdCategoriaEvento = categoriaActualizada.IdCategoriaEvento,
                        NombreCategoria = categoriaActualizada.NombreCategoria,
                        SlugCategoria = categoriaActualizada.SlugCategoria,
                        Estado = categoriaActualizada.Estado
                    }
                    : null
            };
        }

        public async Task<CategoriaCambiarEstadoResponseDTO> ActivarDesactivarCategoriaAsync(
            int id_categoria_evento,
            bool estado)
        {
            string resultado = await Task.Run(() =>
                _dao.CambiarEstado(id_categoria_evento, estado));

            return new CategoriaCambiarEstadoResponseDTO
            {
                Exitoso = !resultado.Contains("Error"),
                Mensaje = resultado,
                Estado = estado
            };
        }
    }
}
