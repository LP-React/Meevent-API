namespace Meevent_API.src.Core.Entities
{
    public class EventReview
    {
        public int Id { get; set; }
        public int Rating { get; set; }      // 1–5
        public string Comment { get; set; } = null!;
        public bool? WasHelpful { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // RELATIONS
        public int UserId { get; set; }
        public int EventId { get; set; }

        // NAVIGATIONS
        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
