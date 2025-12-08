using Meevent_API.src.Features.Paises.DAO.Interfaces;
using Meevent_API.src.Features.Paises.Services.Interfaces;

namespace Meevent_API.src.Features.Paises.Services.Implementations
{
    public class PaisService : IPaisService
    {

        private readonly IPaisDAO _paisDAO;

        public PaisService(IPaisDAO paisDAO)
        {
            _paisDAO = paisDAO;
        }

        public async Task<PaisListResponseDTO> GetAllPaisesAsync()
        {
            try
            {
                // 1. Obtener datos del DAO
                var paises = await _paisDAO.GetAllAsync();

                // 2. Convertir a DTO (mapeo manual)
                var paisesDTO = paises.Select(p => new PaisDTO
                {
                    IdPais = p.IdPais,
                    NombrePais = p.NombrePais
                }).ToList();

                // 3. Retornar respuesta estructurada
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
                // 4. Manejo de errores centralizado
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
