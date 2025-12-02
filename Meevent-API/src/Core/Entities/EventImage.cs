namespace Meevent_API.src.Core.Entities
{
    public class EventImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int SortOrder { get; set; }

        // RELATION
        public int EventId { get; set; }

        // NAVIGATION
        public Event? Event { get; set; }
    }
}
