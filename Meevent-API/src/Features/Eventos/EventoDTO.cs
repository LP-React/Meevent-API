using Meevent_API.src.Features.Paises;
using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.Eventos
{
    public class EventoDTO
    {
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; } = null!;
        public string SlugEvento { get; set; } = null!;
        public string DescripcionEvento { get; set; } = null!;
        public string? DescripcionCorta { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string ZonaHoraria { get; set; } = "UTC-5";
        public string EstadoEvento { get; set; } = "borrador";
        public int CapacidadEvento { get; set; }
        public bool EventoGratuito { get; set; } = false;
        public bool EventoOnline { get; set; } = false;
        public string? ImagenPortadaUrl { get; set; }
        public int PerfilOrganizadorId { get; set; }
        public int SubcategoriaEventoId { get; set; }
        public int LocalId { get; set; }

    }
    public class EventoListadoDTO
    {
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; } = null!;
        public string SlugEvento { get; set; } = null!;
        public string? DescripcionCorta { get; set; }

        public string FechaInicio { get; set; } = null!;
        public string FechaFin { get; set; } = null!;
        public string ZonaHoraria { get; set; } = null!;

        public string EstadoEvento { get; set; } = null!;
        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }

        public string? ImagenPortadaUrl { get; set; }

        public string NombreOrganizador { get; set; } = null!;
        public string NombreCategoria { get; set; } = null!;
        public string NombreSubcategoria { get; set; } = null!;
        public string? NombreLocal { get; set; }
    }


    public class EventoListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int Total_Eventos { get; set; }
        public IEnumerable<EventoListadoDTO> Eventos { get; set; } = new List<EventoListadoDTO>();
    }
    public class EventoDetalleDTO
    {
        [Required(ErrorMessage = "El título del evento es obligatorio")]
        [StringLength(250, ErrorMessage = "El título no puede exceder 250 caracteres")]
        [MinLength(3, ErrorMessage = "El título debe tener al menos 3 caracteres")]
        public string TituloEvento { get; set; } = null!;

        [Required(ErrorMessage = "La descripción del evento es obligatoria")]
        [StringLength(5000, ErrorMessage = "La descripción no puede exceder 5000 caracteres")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string DescripcionEvento { get; set; } = null!;

        [Required(ErrorMessage = "El slug del evento es obligatorio")]
        [StringLength(250, ErrorMessage = "El slug no puede exceder 250 caracteres")]
        public string SlugEvento { get; set; } = null!;

        [StringLength(500, ErrorMessage = "La descripción corta no puede exceder 500 caracteres")]
        public string? DescripcionCorta { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public string FechaInicio { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public string FechaFin { get; set; } = null!;

        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }

        [Required(ErrorMessage = "La capacidad del evento es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        public int CapacidadEvento { get; set; }

        [Required(ErrorMessage = "La subcategoría del evento es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La subcategoría debe ser válida")]
        public int SubcategoriaEventoId { get; set; }

        [Required(ErrorMessage = "El local del evento es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El local debe ser válido")]
        public int LocalId { get; set; }

        [Required(ErrorMessage = "El perfil del organizador es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El perfil del organizador debe ser válido")]
        public int PerfilOrganizadorId { get; set; } 
    }

    public class EventoActualizarDTO
    {
        [StringLength(250, ErrorMessage = "El título no puede exceder 250 caracteres")]
        [MinLength(3, ErrorMessage = "El título debe tener al menos 3 caracteres")]
        public string TituloEvento { get; set; } = null!;

        [StringLength(5000, ErrorMessage = "La descripción no puede exceder 5000 caracteres")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string DescripcionEvento { get; set; } = null!;

        [StringLength(250, ErrorMessage = "El slug no puede exceder 250 caracteres")]
        public string SlugEvento { get; set; } = null!;

        [StringLength(500, ErrorMessage = "La descripción corta no puede exceder 500 caracteres")]
        public string? DescripcionCorta { get; set; }

        public string FechaInicio { get; set; } = null!;

        public string FechaFin { get; set; } = null!;

        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        public int CapacidadEvento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La subcategoría debe ser válida")]
        public int SubcategoriaEventoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El local debe ser válido")]
        public int LocalId { get; set; }

    }



    public class EventoResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = null!;
        public EventoDetalleDTO? Evento { get; set; }
    }
}
