using Meevent_API.src.Features.Paises;

namespace Meevent_API.src.Features.Eventos
{
    public class EventoDTO
    {
        public string TituloEvento { get; set; } = null!;
        public string? DescripcionCorta { get; set; }
        public bool EventoGratuito { get; set; } = false;
        public bool EventoOnline { get; set; } = false;
        public int SubcategoriaEventoId { get; set; }
        public int LocalId { get; set; }


    }
    public class EventoListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int Total_Eventos { get; set; }
        public IEnumerable<EventoDTO> Eventos { get; set; }
    }
    public class EventoDetalleDTO
    {
        public string TituloEvento { get; set; } = null!;
        public string DescripcionEvento { get; set; } = null!;
        public string SlugEvento { get; set; }
        public string? DescripcionCorta { get; set; }
        public string FechaInicio { get; set; } = null!;
        public string FechaFin { get; set; } = null!;
        public bool EventoGratuito { get; set; }
        public bool EventoOnline { get; set; }
        public int CapacidadEvento { get; set; }
        public int SubcategoriaEventoId { get; set; }
        public int LocalId { get; set; }
        public int PerfilOrganizadorId { get; set; }

    }

    public class EventoResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = null!;
        public EventoDetalleDTO? Evento { get; set; }
    }

}
