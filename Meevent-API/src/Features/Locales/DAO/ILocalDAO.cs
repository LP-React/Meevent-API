namespace Meevent_API.src.Features.Locales.DAO
{
    public interface ILocalDAO
    {
        Task<IEnumerable<LocalDTO>> ListarLocalesPorCiudadAsync(int idCiudad);
    }
}
