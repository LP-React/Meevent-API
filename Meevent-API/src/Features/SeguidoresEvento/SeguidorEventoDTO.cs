namespace Meevent_API.src.Features.SeguidoresEvento
{
    public class EventoSeguidoDTO
    {
        // Datos específicos del seguimiento
        public int IdSeguidorEvento { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public int UsuarioId { get; set; }

        // Datos del Evento
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; } = string.Empty;
        public string SlugEvento { get; set; } = string.Empty;
        public string? DescripcionEvento { get; set; }
        public string? DescripcionCorta { get; set; }
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public string ZonaHoraria { get; set; } = string.Empty;
        public string EstadoEvento { get; set; } = string.Empty;
        public int CapacidadEvento { get; set; }
        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }
        public string? ImagenPortadaUrl { get; set; }

        // Datos del Organizador
        public int IdPerfilOrganizador { get; set; }
        public string NombreOrganizador { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }

        // Datos de Categoría y Subcategoría
        public int IdSubcategoria { get; set; }
        public string NombreSubcategoria { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string? CategoriaIcono { get; set; }

        // Datos del Local y Ubicación
        public int IdLocal { get; set; }
        public string NombreLocal { get; set; } = string.Empty;
        public string DireccionLocal { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string NombreCiudad { get; set; } = string.Empty;
        public string NombrePais { get; set; } = string.Empty;
    }

    public class EventoSeguidoListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int TotalResultados { get; set; }
        public List<EventoSeguidoDTO> Eventos { get; set; } = new List<EventoSeguidoDTO>();
    }
}
