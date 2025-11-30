using Meevent_API.src.Features.Users;
using Meevent_API.src.Features.OrganizerProfiles;

namespace Meevent_API.src.Features.OrganizerReviews
{
    public class OrganizerReviewEntity
    {
        public int IdOrganizerReview { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int OrganizerProfileId { get; set; }
        public OrganizerProfileEntity OrganizerProfile { get; set; }

        public int Rating { get; set; }  // 1–5
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Opcionales
        public int WasHelpfulCount { get; set; } = 0;
        public bool IsVerifiedBuyer { get; set; } = false;
    }
}
