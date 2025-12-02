namespace Meevent_API.src.Core.Entities
{
    public class OrganizerReview
    {
        public int IdOrganizerReview { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int OrganizerProfileId { get; set; }
        public OrganizerProfile OrganizerProfile { get; set; }

        public int Rating { get; set; }  // 1–5
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Opcionales
        public int WasHelpfulCount { get; set; } = 0;
        public bool IsVerifiedBuyer { get; set; } = false;
    }
}
