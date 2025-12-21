using Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.PerfilesArtistas.DAO
{
    public interface IPerfilArtistaDAO
    {
        Task<PerfilArtista> ObtenerPorUsuarioIdAsync(int usuarioId);
    }
}
