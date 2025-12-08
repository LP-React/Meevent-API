namespace Meevent_API.src.Features.Paises.Services.Interfaces
{
    public interface IPaisService
    {
        Task<PaisListResponseDTO> GetAllPaisesAsync();
    
    }
}
