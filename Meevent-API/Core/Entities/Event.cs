namespace Meevent_API.Core.Entities
{
    public class Event
    {
        public int IdEvent { get; set; }
        public string NameEvent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }

        // Relación con Venue (FK)
        public int IdVenue { get; set; }
        public Venue Venue { get; set; }

    }
}
