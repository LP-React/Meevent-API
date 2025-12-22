using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.PerfilesOrganizadores;

namespace Meevent_API.src.Features.PerfilesOrganizadores.DAO
{
    public interface IPerfilOrganizadorDAO
    {
        IEnumerable<PerfilOrganizador> GetPerfilesOrganizador();
        IEnumerable<PerfilOrganizador> GetPerfilOrganizadorPorId(int id_perfil_organizador);
        string CrearPerfilOrganizador(PerfilOrganizadorCrearDTO perfil);
        string ActualizarPerfilOrganizador(int id, PerfilOrganizadorEditarDTO perfil);
        Task<PerfilOrganizador> ObtenerPorUsuarioIdAsync(int usuarioId);

    }

}
