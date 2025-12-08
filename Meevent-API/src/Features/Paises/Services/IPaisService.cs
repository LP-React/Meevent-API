namespace Meevent_API.src.Features.Paises.Services
{
    public interface IPaisService
    {
        Task<PaisListResponseDTO> GetAllPaisesAsync();
    
    }
}
