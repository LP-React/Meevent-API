using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Paises.DAO.Interfaces
{
    public interface IPaisDAO
    {
        Task<IEnumerable<Pais>> GetAllAsync();
    }
}
