using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;

namespace Meevent_API.src.Features.Usuarios.DAO
{
    public interface IUsuarioDAO
    {
        IEnumerable<Usuario> GetUsuarios();
        IEnumerable<Usuario> GetUsuariosPorId(int id_usuario);
        Task<UsuarioDetalleDTO> GetUsuariosPorCorreo(string correo_electronico);
        string InsertUsuario(UsuarioRegistroDTO reg);
        string LoginUsuario(LoginDTO login);
        string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario);
        bool VerificarCorreoExistente(string correo_electronico);
        bool VerificarPaisExiste(int id_pais);
        bool VerificarCiudadExiste(int id_ciudad);
        string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa);

    }
}