namespace Meevent_API.src.Features.Ciudades.DAO
{
    public interface ICiudadDAO
    {
        Task<IEnumerable<CiudadDTO>> ListarCiudadesPorPaisAsync(int? idPais);
    }
}
