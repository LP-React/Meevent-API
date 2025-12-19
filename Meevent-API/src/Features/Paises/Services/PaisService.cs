using Meevent_API.src.Features.Paises.DAO;
using Meevent_API.src.Features.Paises.Services.Interfaces;
using gRcp_Paises;

namespace Meevent_API.src.Features.Paises.Services
{
    public class PaisService : IPaisService
    {

        private readonly ServicioPaises.ServicioPaisesClient _grpcClient;
        private readonly ILogger<PaisService> _logger;

        public PaisService(
            ServicioPaises.ServicioPaisesClient grpcClient,
            ILogger<PaisService> logger)
        {
            _grpcClient = grpcClient;
            _logger = logger;
        }

        public async Task<PaisListResponseDTO> GetAllPaisesAsync()
        {
            try
            {
                // 1️ Llamada gRPC
                var response = await _grpcClient.GetAllAsync(new Empty());

                // 2️ Mapeo gRPC → DTO API
                var paisesDTO = response.Items.Select(p => new PaisDTO
                {
                    IdPais = p.IdPais,
                    NombrePais = p.NombrePais
                }).ToList();

                // 3️ Respuesta REST
                return new PaisListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Países obtenidos correctamente",
                    Total_Paises = paisesDTO.Count,
                    Paises = paisesDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir gRPC de Países");

                return new PaisListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener países: {ex.Message}",
                    Total_Paises = 0,
                    Paises = new List<PaisDTO>()
                };
            }
        }

    }
}
