using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.Resenas
{
    public class ResenaOrganizadorDTO
    {
        public int IdResenaOrganizador { get; set; }
        public int CalificacionResena { get; set; }
        public string ComentarioResena { get; set; }
        public string FechaCreacion { get; set; }
        public int ContadorUtilidad { get; set; }
        public bool CompradorVerificado { get; set; }
        public int PerfilOrganizadorId { get; set; }
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string ImagenPerfilUrl { get; set; }
    }

    // DTO para crear reseña
    public class CrearResenaOrganizadorDTO
    {
        [Required(ErrorMessage = "La calificación es obligatoria")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int CalificacionResena { get; set; }

        [Required(ErrorMessage = "El comentario es obligatorio")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "El comentario debe tener entre 10 y 2000 caracteres")]
        public string ComentarioResena { get; set; }

        [Required(ErrorMessage = "El ID del perfil organizador es obligatorio")]
        public int PerfilOrganizadorId { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        public int UsuarioId { get; set; }

        public bool CompradorVerificado { get; set; } = false;
    }

    // DTO para actualizar reseña
    public class ActualizarResenaOrganizadorDTO
    {
        [Required(ErrorMessage = "La calificación es obligatoria")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int CalificacionResena { get; set; }

        [Required(ErrorMessage = "El comentario es obligatorio")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "El comentario debe tener entre 10 y 2000 caracteres")]
        public string ComentarioResena { get; set; }
    }


    // DTO de respuesta para lista
    public class ResenaOrganizadorListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalResenas { get; set; }
        public IEnumerable<ResenaOrganizadorDTO> Resenas { get; set; }
    }

    // DTO de respuesta genérica
    public class ResenaOrganizadorResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public ResenaOrganizadorDTO Resena { get; set; }
    }

}
