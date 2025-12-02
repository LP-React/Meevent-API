namespace Meevent_API.src.Core.Entities
{
    public class EventSubCategory
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        // Navigation
        public EventCategory? Category { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
