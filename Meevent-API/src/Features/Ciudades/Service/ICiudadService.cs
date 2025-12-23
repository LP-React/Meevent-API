namespace Meevent_API.src.Features.Ciudades.Service
{
    public interface ICiudadService
    {
        Task<CiudadListResponseDTO> GetCiudadesByPaisAsync(int idPais);
    }
}
