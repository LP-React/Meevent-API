namespace Meevent_API.src.Core.Entities
{
    public class OrganizerReview
    {
        public int Id { get; set; }
        public int Rating { get; set; }  // 1–5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int WasHelpfulCount { get; set; } = 0;
        public bool IsVerifiedBuyer { get; set; } = false;

        // RELATION
        public int OrganizerProfileId { get; set; }
        public int UserId { get; set; }

        // NAVIGATION
        public User User { get; set; }
        public OrganizerProfile OrganizerProfile { get; set; }
    }
}
