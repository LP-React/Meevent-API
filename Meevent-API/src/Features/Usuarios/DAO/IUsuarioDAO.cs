using Meevent_API.src.Core.Entities;
using Meevent_API.src.Core.Entities.Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Usuarios;
using Meevent_API.src.Features.Usuarios.Meevent_API.src.Features.Usuarios;

namespace Meevent_API.src.Features.Usuarios.DAO
{
    public interface IUsuarioDAO
    {
        IEnumerable<Usuario> GetUsuarios();
        IEnumerable<Usuario> GetUsuariosPorId(int id_usuario);
        IEnumerable<Usuario> GetUsuariosPorCorreo(string correo_electronico);
        string InsertUsuario(UsuarioRegistroDTO reg);
        string LoginUsuario(LoginDTO login);
        string ActualizarUsuario(int id_usuario, UsuarioEditarDTO usuario);
        bool VerificarCorreoExistente(string correo_electronico);
        bool VerificarPaisExiste(int id_pais);
        string ActivarDesactivarCuenta(int id_usuario, bool cuenta_activa);

    }
}