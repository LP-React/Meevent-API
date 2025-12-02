namespace Meevent_API.src.Core.Entities
{
    public class EventFollower
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
