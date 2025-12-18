using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.PerfilesOrganizadores
{

    public class PerfilOrganizadorDTO
    {
        public int id_perfil_organizador { get; set; }
        public string nombre_organizador { get; set; }
        public string descripcion_organizador { get; set; }
        public string? sitio_web { get; set; }
        public string? facebook_url { get; set; }
        public string? instagram_url { get; set; }
        public string? tiktok_url { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int usuario_id { get; set; }
    }
    public class PerfilOrganizadorListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalOrganizadores { get; set; }
        public IEnumerable<PerfilOrganizadorDTO> Organizadores { get; set; }
    }




    public class PerfilOrganizadorCrearDTO
    {
        [Required(ErrorMessage = "El nombre del organizador es obligatorio")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        public string nombre_organizador { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string descripcion_organizador { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? sitio_web { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? facebook_url { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? instagram_url { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? tiktok_url { get; set; }

        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de usuario debe ser mayor a 0")]
        public int usuario_id { get; set; }
    }




    public class PerfilOrganizadorEditarDTO
    {
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        public string? nombre_organizador { get; set; }

        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string? descripcion_organizador { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? sitio_web { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? facebook_url { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? instagram_url { get; set; }

        [StringLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
        public string? tiktok_url { get; set; }
    }


    public class PerfilOrganizadorDetalleDTO
    {
        public int id_perfil_organizador { get; set; }
        public string nombre_organizador { get; set; }
        public string descripcion_organizador { get; set; }
        public string? sitio_web { get; set; }
        public string? facebook_url { get; set; }
        public string? instagram_url { get; set; }
        public string? tiktok_url { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int usuario_id { get; set; }
    }


    public class PerfilOrganizadorOperacionResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public PerfilOrganizadorDetalleDTO Perfil { get; set; }
    }
}