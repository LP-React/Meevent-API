namespace Meevent_API.src.Features.PerfilesOrganizadores.Services
{
    public interface IPerfilOrganizadorService
    {
        Task<PerfilOrganizadorListResponseDTO> ListarPerfilesOrganizadorAsync();
        Task<PerfilOrganizadorDetalleDTO> ObtenerPerfilOrganizadorPorIdAsync(int id);
        Task<string> CrearPerfilOrganizadorAsync(PerfilOrganizadorCrearDTO perfil);
        Task<string> ActualizarPerfilOrganizadorAsync(int id, PerfilOrganizadorEditarDTO perfil);
    }
}
