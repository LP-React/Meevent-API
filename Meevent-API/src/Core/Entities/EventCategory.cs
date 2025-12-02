namespace Meevent_API.src.Features.Event
{
    public class EventCategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? IconUrl { get; set; }

        // Navigation
        public ICollection<EventSubCategoryEntity>? SubCategories { get; set; }
    }
}
