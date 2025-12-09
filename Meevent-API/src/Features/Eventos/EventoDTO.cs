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

}
