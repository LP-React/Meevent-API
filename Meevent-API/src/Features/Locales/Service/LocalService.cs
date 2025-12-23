using Meevent_API.src.Features.Locales.DAO;

namespace Meevent_API.src.Features.Locales.Service
{
    public class LocalService : ILocalService
    {
        private readonly ILocalDAO _localDAO;
        private readonly ILogger<LocalService> _logger;

        public LocalService(ILocalDAO localDAO, ILogger<LocalService> logger)
        {
            _localDAO = localDAO;
            _logger = logger;
        }

        public async Task<LocalListResponseDTO> GetLocalesByCiudadAsync(int? idCiudad)
        {
            try
            {
                var locales = await _localDAO.ListarLocalesPorCiudadAsync(idCiudad);
                var listaLocales = locales.ToList();

                return new LocalListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = listaLocales.Any()
                        ? $"Se encontraron {listaLocales.Count} locales."
                        : "No se encontraron locales para esta ciudad.",
                    TotalLocales = listaLocales.Count,
                    Locales = listaLocales
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar locales por ciudad {IdCiudad}", idCiudad);

                return new LocalListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error interno al procesar la solicitud: {ex.Message}",
                    TotalLocales = 0,
                    Locales = new List<LocalDTO>()
                };
            }
        }
    
    }
}
