using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Usuarios.DAO
{
    public interface IUsuarioDAO
    {
        Task<IEnumerable<UsuarioDetalleDTO>> GetUsuarios();
        Task<UsuarioDetalleDTO> GetUsuariosPorId(int id_usuario);
        Task<UsuarioDetalleDTO> GetUsuariosPorCorreo(string correo_electronico);
        Task<string> InsertUsuario(UsuarioRegistroDTO reg);
        string LoginUsuario(LoginDTO login);
        string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario);
        Task<bool> VerificarCorreoExistente(string correo_electronico);
        bool VerificarPaisExiste(int id_pais);
        bool VerificarCiudadExiste(int id_ciudad);
        string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa);

    }
}