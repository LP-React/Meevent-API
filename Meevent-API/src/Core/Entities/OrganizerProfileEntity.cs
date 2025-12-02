namespace Meevent_API.src.Core.Entities
{
    public class OrganizerProfileEntity
    {
        public int IdOrganizerProfile { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string OrganizationName { get; set; }
        public string Description { get; set; }

        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TiktokUrl { get; set; }
        public string? TwitterUrl { get; set; }

        public string? Address { get; set; }
        public string? PhoneContact { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrganizerReviewEntity> OrganizerReviews { get; set; }
            = new List<OrganizerReviewEntity>();
    }
}
