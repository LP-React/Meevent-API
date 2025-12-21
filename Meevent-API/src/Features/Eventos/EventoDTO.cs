using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.Eventos
{
    // DTO para crear un nuevo evento
    public class EventoCrearDTO
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(250)]
        public string TituloEvento { get; set; } = string.Empty;

        [Required]
        public string DescripcionEvento { get; set; } = string.Empty;

        public string? DescripcionCorta { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public string ZonaHoraria { get; set; } = "UTC-5";

        [Required]
        public int CapacidadEvento { get; set; }

        [Required]
        public bool EventoGratuito { get; set; }
        [Required]
        public bool EventoOnline { get; set; }

        public string EstadoEvento { get; set; } = "borrador";

        public string? ImagenPortadaUrl { get; set; }

        [Required]
        public int PerfilOrganizadorId { get; set; }

        [Required]
        public int SubcategoriaEventoId { get; set; }

        [Required]
        public int LocalId { get; set; }
    }

    // DTO para actualizar un evento existente
    public class EventoActualizarDTO
    {
        // El resto son opcionales
        public string? TituloEvento { get; set; }
        public string? DescripcionEvento { get; set; }
        public string? DescripcionCorta { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? ZonaHoraria { get; set; }
        public string? EstadoEvento { get; set; }
        public int CapacidadEvento { get; set; }
        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }
        public string ImagenPortadaUrl { get; set; }
        public int SubcategoriaEventoId { get; set; }
        public int LocalId { get; set; }
    }

    // DTOs relacionados
    public class OrganizadorDTO
    {
        public int IdPerfilOrganizador { get; set; }
        public string NombreOrganizador { get; set; }
        public string DescripcionOrganizador { get; set; }
        public string SitioWeb { get; set; }
        public string LogoUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TiktokUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string DireccionOrganizador { get; set; }
        public string TelefonoContacto { get; set; }
    }

    public class CategoriaEventoDTO
    {
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; }
        public string SlugCategoria { get; set; }
        public string IconoUrl { get; set; }
    }

    public class SubcategoriaEventoDTO
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }
        public CategoriaEventoDTO Categoria { get; set; }
    }

    public class UbicacionDTO 
    {
        public int IdPais { get; set; }
        public string NombrePais { get; set; }
        public string CodigoISO { get; set; }

        public int IdCiudad { get; set; }
        public string NombreCiudad { get; set; }

        public int IdLocal { get; set; }
        public string NombreLocal { get; set; }
        public int CapacidadLocal { get; set; }
        public string DireccionLocal { get; set; }

        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

    // DTO completo del evento con objetos relacionados
    public class EventoCompletoDTO
    {
        // Datos básicos del evento
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; }
        public string SlugEvento { get; set; }
        public string DescripcionEvento { get; set; }
        public string DescripcionCorta { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string ZonaHoraria { get; set; }
        public string EstadoEvento { get; set; }
        public string EstadoEventoCliente { get; set; }
        public string EstadoEventoAdmin { get; set; }
        public int CapacidadEvento { get; set; }
        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }
        public string? ImagenPortadaUrl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        // Objetos relacionados completos
        public OrganizadorDTO? Organizador { get; set; }
        public SubcategoriaEventoDTO? Subcategoria { get; set; }
        public UbicacionDTO? Ubicacion { get; set; }
    }

    // Response DTOs
    public class EventoCompletoResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public EventoCompletoDTO? Evento { get; set; }
    }

    // DTO para la respuesta de listar eventos completos
    public class EventosCompletosListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalEventos { get; set; }
        public IEnumerable<EventoCompletoDTO?> Eventos { get; set; }
    }

}
