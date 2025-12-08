using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Paises.DAO
{
    public interface IPaisDAO
    {
        Task<IEnumerable<Pais>> GetAllAsync();
    }
}
