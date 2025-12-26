namespace Meevent_API.src.Core.Entities
{
    public class Evento
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
        public string FechaCreacion { get; set; }
        public string FechaActualizacion { get; set; }
        public int PerfilOrganizadorId { get; set; }
        public int SubcategoriaEventoId { get; set; }
        public int? LocalId { get; set; }
    }
}
