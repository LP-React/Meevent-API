namespace Meevent_API.src.Features.Usuarios.Service
{
    public interface IUsuarioService
    {
        Task<UsuariosListaResponseDTO> ListarUsuariosAsync();
        
        Task<UsuarioDetalleResponseDTO> ObtenerUsuarioPorIdAsync(int id_usuario);
        
        Task<UsuarioDetalleDTO> ObtenerUsuarioPorCorreoAsync(string correo_electronico);
        
        Task<UsuarioDetalleResponseDTO> RegistrarUsuarioAsync(UsuarioRegistroDTO registro);

        Task<LoginResponseDTOE> LoginAsync(LoginDTO login);
        
        //Task<UsuarioEditarResponseDTO> ActualizarUsuarioAsync(int id_usuario, UsuarioEditarDTO usuario);
        
        Task<bool> VerificarCorreoExistenteAsync(string correo_electronico);
        
        Task<bool> CiudadExisteAsync(int id_ciudad);
        
        //Task<UsuarioActivarCuentaResponseDTO> ActivarDesactivarCuentaAsync(int id_usuario, bool cuenta_activa);


    }
}