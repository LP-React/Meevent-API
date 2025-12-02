namespace Meevent_API.src.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsVerifiedEmail { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Tipo de usuario (normal, artist, organizer)
        public string UserType { get; set; } = "normal";

        // NAVIGATIONS
        public ArtistProfile? ArtistProfile { get; set; }
        public OrganizerProfile? OrganizerProfile { get; set; }

        public ICollection<OrganizerReview> OrganizerReviews { get; set; }
            = new List<OrganizerReview>();

        public ICollection<Wishlist> Wishlists { get; set; }
            = new List<Wishlist>();

        public ICollection<EventReview> EventReview { get; set; }
            = new List<EventReview>();
    }
}
