using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Resenas.DAO
{
    public interface IResenasOrganizadorDAO
    {
        Task<IEnumerable<ResenaOrganizador>> GetAllByOrganizadorAsync(int perfilOrganizadorId);
        Task<ResenaOrganizador> GetByIdAsync(int idResenaOrganizador);
        Task<int> InsertAsync(ResenaOrganizador resena);
        Task<int> UpdateAsync(int idResenaOrganizador,int idUsuario, int calificacion, string comentario);
        Task<int> IncrementarUtilidadAsync(int idResenaOrganizador);
        Task<int> DecrementarUtilidadAsync(int idResenaOrganizador);
        Task<ResenaOrganizador> VerificarResenaExistenteAsync(int perfilOrganizadorId, int usuarioId);
    }
}
