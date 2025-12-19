namespace Meevent_API.src.Features.Resenas.Service
{
    public interface IResenasOrganizadorService
    {
        Task<ResenaOrganizadorListResponseDTO> GetAllByOrganizadorAsync(int perfilOrganizadorId);
        Task<ResenaOrganizadorResponseDTO> GetByIdAsync(int idResenaOrganizador);
        Task<ResenaOrganizadorResponseDTO> CreateAsync(CrearResenaOrganizadorDTO dto);
        Task<ResenaOrganizadorResponseDTO> UpdateAsync(int idResenaOrganizador,int idUsuario, ActualizarResenaOrganizadorDTO dto);
        Task<ResenaOrganizadorResponseDTO> IncrementarUtilidadAsync(int idResenaOrganizador);
        Task<ResenaOrganizadorResponseDTO> DecrementarUtilidadAsync(int idResenaOrganizador);
    }
}
