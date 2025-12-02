namespace Meevent_API.src.Core.Entities
{
    public class OrganizerProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

        // RELATIONS
        public int UserId { get; set; }

        // NAVIGATIONS
        public User User { get; set; }
        public ICollection<OrganizerReview> OrganizerReviews { get; set; }
            = new List<OrganizerReview>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
