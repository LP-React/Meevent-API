namespace Meevent_API.Features.Event
{
    public class EventFollowerEntity
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public int EventId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public EventEntity? Event { get; set; }
        public UserEntity? User { get; set; }
    }
}
