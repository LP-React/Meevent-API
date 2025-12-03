namespace Meevent_API.src.Core.Entities
{
    public class EventSubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        // RELATION
        public int CategoryId { get; set; }
        // NAVIGATION
        public EventCategory? Category { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
