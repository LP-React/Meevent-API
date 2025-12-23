namespace Meevent_API.src.Features.Locales.Service
{
    public interface ILocalService
    {
        Task<LocalListResponseDTO> GetLocalesByCiudadAsync(int? idCiudad);
    }
}
