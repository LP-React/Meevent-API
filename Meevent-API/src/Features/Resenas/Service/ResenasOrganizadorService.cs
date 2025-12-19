using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.Resenas.DAO;

namespace Meevent_API.src.Features.Resenas.Service
{
    public class ResenasOrganizadorService : IResenasOrganizadorService
    {
        private readonly IResenasOrganizadorDAO _resenaDAO;

        public ResenasOrganizadorService(IResenasOrganizadorDAO resenaDAO)
        {
            _resenaDAO = resenaDAO;
        }

        public async Task<ResenaOrganizadorListResponseDTO> GetAllByOrganizadorAsync(int perfilOrganizadorId)
        {
            try
            {
                var resenas = await _resenaDAO.GetAllByOrganizadorAsync(perfilOrganizadorId);

                var resenasDTO = resenas.Select(r => new ResenaOrganizadorDTO
                {
                    IdResenaOrganizador = r.IdResenaOrganizador,
                    CalificacionResena = r.CalificacionResena,
                    ComentarioResena = r.ComentarioResena,
                    FechaCreacion = r.FechaCreacion.ToString("yyyy-mm-dd"),
                    ContadorUtilidad = r.ContadorUtilidad,
                    CompradorVerificado = r.CompradorVerificado,
                    PerfilOrganizadorId = r.PerfilOrganizadorId,
                    UsuarioId = r.UsuarioId,
                    NombreCompleto = r.NombreCompleto,
                    ImagenPerfilUrl = r.ImagenPerfilUrl
                }).ToList();

                return new ResenaOrganizadorListResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Reseñas obtenidas correctamente",
                    TotalResenas = resenasDTO.Count,
                    Resenas = resenasDTO
                };
            }
            catch (Exception ex)
            {
                return new ResenaOrganizadorListResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener reseñas: {ex.Message}",
                    TotalResenas = 0,
                    Resenas = new List<ResenaOrganizadorDTO>()
                };
            }
        }

        public async Task<ResenaOrganizadorResponseDTO> GetByIdAsync(int idResenaOrganizador)
        {
            try
            {
                var resena = await _resenaDAO.GetByIdAsync(idResenaOrganizador);

                if (resena == null)
                {
                    return new ResenaOrganizadorResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Reseña no encontrada",
                        Resena = null
                    };
                }

                var resenaDTO = new ResenaOrganizadorDTO
                {
                    IdResenaOrganizador = resena.IdResenaOrganizador,
                    CalificacionResena = resena.CalificacionResena,
                    ComentarioResena = resena.ComentarioResena,
                    FechaCreacion = resena.FechaCreacion.ToString("yyyy-mm-dd"),
                    ContadorUtilidad = resena.ContadorUtilidad,
                    CompradorVerificado = resena.CompradorVerificado,
                    PerfilOrganizadorId = resena.PerfilOrganizadorId,
                    UsuarioId = resena.UsuarioId,
                    NombreCompleto = resena.NombreCompleto,
                    ImagenPerfilUrl = resena.ImagenPerfilUrl
                };

                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = true,
                    Mensaje = "Reseña obtenida correctamente",
                    Resena = resenaDTO
                };
            }
            catch (Exception ex)
            {
                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener reseña: {ex.Message}",
                    Resena = null
                };
            }
        }

        public async Task<ResenaOrganizadorResponseDTO> CreateAsync(CrearResenaOrganizadorDTO dto)
        {
            try
            {
                // Verificar si el usuario ya reseñó al organizador
                var resenaExistente = await _resenaDAO.VerificarResenaExistenteAsync(
                    dto.PerfilOrganizadorId, dto.UsuarioId);

                if (resenaExistente != null)
                {
                    return new ResenaOrganizadorResponseDTO
                    {
                        Exitoso = false,
                        Mensaje = "Ya has publicado una reseña para este organizador",
                        Resena = null
                    };
                }

                var resena = new ResenaOrganizador
                {
                    CalificacionResena = dto.CalificacionResena,
                    ComentarioResena = dto.ComentarioResena,
                    CompradorVerificado = dto.CompradorVerificado,
                    PerfilOrganizadorId = dto.PerfilOrganizadorId,
                    UsuarioId = dto.UsuarioId
                };

                int idGenerado = await _resenaDAO.InsertAsync(resena);

                if (idGenerado > 0)
                {
                    var resenaCreada = await _resenaDAO.GetByIdAsync(idGenerado);

                    return new ResenaOrganizadorResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = "Reseña creada exitosamente",
                        Resena = new ResenaOrganizadorDTO
                        {
                            IdResenaOrganizador = resenaCreada.IdResenaOrganizador,
                            CalificacionResena = resenaCreada.CalificacionResena,
                            ComentarioResena = resenaCreada.ComentarioResena,
                            FechaCreacion = resenaCreada.FechaCreacion.ToString("yyyy-mm-dd"),
                            ContadorUtilidad = resenaCreada.ContadorUtilidad,
                            CompradorVerificado = resenaCreada.CompradorVerificado,
                            PerfilOrganizadorId = resenaCreada.PerfilOrganizadorId,
                            UsuarioId = resenaCreada.UsuarioId,
                            NombreCompleto = resenaCreada.NombreCompleto,
                            ImagenPerfilUrl = resenaCreada.ImagenPerfilUrl
                        }
                    };
                }

                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "No se encontró la reseña o no se pudo actualizar",
                    Resena = null
                };
            }
            catch (Exception ex)
            {
                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al actualizar reseña: {ex.Message}",
                    Resena = null
                };
            }
        }

        public async Task<ResenaOrganizadorResponseDTO> UpdateAsync(int idResenaOrganizador,int usuarioId, ActualizarResenaOrganizadorDTO dto)
        {
            int filas = await _resenaDAO.UpdateAsync(
                idResenaOrganizador,
                usuarioId,
                dto.CalificacionResena,
                dto.ComentarioResena
            );

            if (filas == 0)
            {
                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "No tienes permiso para actualizar esta reseña",
                    Resena = null
                };
            }

            var resena = await _resenaDAO.GetByIdAsync(idResenaOrganizador);

            return new ResenaOrganizadorResponseDTO
            {
                Exitoso = true,
                Mensaje = "Reseña actualizada correctamente",
                Resena = null
            };
        }

        public async Task<ResenaOrganizadorResponseDTO> IncrementarUtilidadAsync(int idResenaOrganizador)
        {
            try
            {
                int filasAfectadas = await _resenaDAO.IncrementarUtilidadAsync(idResenaOrganizador);

                if (filasAfectadas > 0)
                {
                    return new ResenaOrganizadorResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = "Contador de utilidad incrementado",
                        Resena = null
                    };
                }

                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "No se pudo incrementar el contador",
                    Resena = null
                };
            }
            catch (Exception ex)
            {
                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al incrementar utilidad: {ex.Message}",
                    Resena = null
                };
            }
        }

        public async Task<ResenaOrganizadorResponseDTO> DecrementarUtilidadAsync(int idResenaOrganizador)
        {
            try
            {
                int filasAfectadas = await _resenaDAO.DecrementarUtilidadAsync(idResenaOrganizador);

                if (filasAfectadas > 0)
                {
                    return new ResenaOrganizadorResponseDTO
                    {
                        Exitoso = true,
                        Mensaje = "Contador de utilidad decrementado",
                        Resena = null
                    };
                }

                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "No se pudo decrementar el contador",
                    Resena = null
                };
            }
            catch (Exception ex)
            {
                return new ResenaOrganizadorResponseDTO
                {
                    Exitoso = false,
                    Mensaje = $"Error al decrementar utilidad: {ex.Message}",
                    Resena = null
                };
            }
        }
    
    }
}
