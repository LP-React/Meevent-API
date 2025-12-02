namespace Meevent_API.src.Core.Entities
{
    public class EventFollower
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // RELATIONS
        public int UserId { get; set; }
        public int EventId { get; set; }

        // NAVIGATIONS
        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
