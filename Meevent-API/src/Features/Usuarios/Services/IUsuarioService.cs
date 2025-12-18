
using Meevent_API.src.Features.Usuarios;
using Meevent_API.src.Features.Usuarios.Meevent_API.src.Features.Usuarios;

namespace Meevent_API.src.Features.Usuarios.Service
{
    public interface IUsuarioService
    {
        Task<UsuarioListResponseDTO> ObtenerUsuariosAsync();
        Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int id_usuario);
        Task<UsuarioDTO> ObtenerUsuarioPorCorreoAsync(string correo_electronico);
        Task<string> RegistrarUsuarioAsync(UsuarioRegistroDTO registro);
        Task<LoginResponseDTO> LoginAsync(LoginDTO login);
        Task<UsuarioEditarResponseDTO> ActualizarUsuarioAsync(int id_usuario, UsuarioEditarDTO usuario);
        Task<VerificarEmailResponseDTO> VerificarCorreoExistenteAsync(string correo_electronico);
    }
}