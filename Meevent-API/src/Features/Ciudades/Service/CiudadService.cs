using Meevent_API.src.Features.Ciudades.DAO;

namespace Meevent_API.src.Features.Ciudades.Service
{
    public class CiudadService : ICiudadService
    {
        private readonly ICiudadDAO _ciudadDAO;
        private readonly ILogger<CiudadService> _logger;

        public CiudadService(ICiudadDAO ciudadDAO, ILogger<CiudadService> logger)
        {
            _ciudadDAO = ciudadDAO;
            _logger = logger;
        }

        public async Task<CiudadListResponseDTO> GetCiudadesByPaisAsync(int idPais)
        {
            try
            {
                var ciudades = await _ciudadDAO.ListarCiudadesPorPaisAsync(idPais);
                var listaCiudades = ciudades.ToList();

                return new CiudadListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = listaCiudades.Any()
                        ? $"Se encontraron {listaCiudades.Count} ciudades."
                        : "No se encontraron ciudades para este país.",
                    TotalCiudades = listaCiudades.Count,
                    Ciudades = listaCiudades
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar ciudades para el país {IdPais}", idPais);
                return new CiudadListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error interno: {ex.Message}",
                    TotalCiudades = 0,
                    Ciudades = new List<CiudadDTO>()
                };
            }
        }
    
    }
}
