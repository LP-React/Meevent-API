using Meevent_API.src.Features.PerfilesOrganizadores.DAO;

namespace Meevent_API.src.Features.PerfilesOrganizadores.Services
{
    public class PerfilOrganizadorService : IPerfilOrganizadorService
    {
        private readonly IPerfilOrganizadorDAO _perfilOrganizadorDAO;

        public PerfilOrganizadorService(IPerfilOrganizadorDAO perfilOrganizadorDAO)
        {
            _perfilOrganizadorDAO = perfilOrganizadorDAO;
        }

        public async Task<PerfilOrganizadorListResponseDTO> ListarPerfilesOrganizadorAsync()
        {
            try
            {
                var perfiles = await Task.Run(() => _perfilOrganizadorDAO.GetPerfilesOrganizador());

                var perfilesDTO = perfiles.Select(p => new PerfilOrganizadorDTO
                {
                    id_perfil_organizador = p.IdPerfilOrganizador,
                    nombre_organizador = p.NombreOrganizador,
                    descripcion_organizador = p.DescripcionOrganizador,
                    sitio_web = p.SitioWeb,
                    facebook_url = p.FacebookUrl,
                    instagram_url = p.InstagramUrl,
                    tiktok_url = p.TiktokUrl,
                    fecha_creacion = p.FechaCreacion,
                    usuario_id = p.UsuarioId
                }).ToList();

                return new PerfilOrganizadorListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Perfiles de organizadores obtenidos exitosamente",
                    TotalOrganizadores = perfilesDTO.Count,
                    Organizadores = perfilesDTO
                };
            }
            catch (Exception ex)
            {
                return new PerfilOrganizadorListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener perfiles: {ex.Message}",
                    TotalOrganizadores = 0,
                    Organizadores = Enumerable.Empty<PerfilOrganizadorDTO>()
                };
            }
        }

        public async Task<PerfilOrganizadorDetalleDTO> ObtenerPerfilOrganizadorPorIdAsync(int id)
        {
            try
            {
                var perfiles = await Task.Run(() => _perfilOrganizadorDAO.GetPerfilOrganizadorPorId(id));
                var perfil = perfiles.FirstOrDefault();

                if (perfil == null) return null;

                return new PerfilOrganizadorDetalleDTO
                {
                    id_perfil_organizador = perfil.IdPerfilOrganizador,
                    nombre_organizador = perfil.NombreOrganizador,
                    descripcion_organizador = perfil.DescripcionOrganizador,
                    sitio_web = perfil.SitioWeb,
                    facebook_url = perfil.FacebookUrl,
                    instagram_url = perfil.InstagramUrl,
                    tiktok_url = perfil.TiktokUrl,
                    fecha_creacion = perfil.FechaCreacion,
                    usuario_id = perfil.UsuarioId
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> CrearPerfilOrganizadorAsync(PerfilOrganizadorCrearDTO perfil)
        {
            try
            {
                return await Task.Run(() => _perfilOrganizadorDAO.CrearPerfilOrganizador(perfil));
            }
            catch (Exception ex)
            {
                return $"Error en el servicio: {ex.Message}";
            }
        }

        public async Task<string> ActualizarPerfilOrganizadorAsync(int id, PerfilOrganizadorEditarDTO perfil)
        {
            try
            {
                if (string.IsNullOrEmpty(perfil.nombre_organizador) &&
                    string.IsNullOrEmpty(perfil.descripcion_organizador) &&
                    string.IsNullOrEmpty(perfil.sitio_web) &&
                    string.IsNullOrEmpty(perfil.facebook_url) &&
                    string.IsNullOrEmpty(perfil.instagram_url) &&
                    string.IsNullOrEmpty(perfil.tiktok_url))
                {
                    return "Debe proporcionar al menos un campo para actualizar";
                }

                return await Task.Run(() => _perfilOrganizadorDAO.ActualizarPerfilOrganizador(id, perfil));
            }
            catch (Exception ex)
            {
                return $"Error en el servicio: {ex.Message}";
            }
        }
    }
}