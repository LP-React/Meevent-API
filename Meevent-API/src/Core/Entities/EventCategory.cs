namespace Meevent_API.src.Core.Entities
{
    public class EventCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? IconUrl { get; set; }

        // Navigation
        public ICollection<EventSubCategory>? SubCategories { get; set; }
    }
}
