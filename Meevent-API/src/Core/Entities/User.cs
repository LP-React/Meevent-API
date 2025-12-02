namespace Meevent_API.src.Core.Entities
{
    public class User
    {
        public int IdUser { get; set; }

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

        // === Relaciones ===
        public ArtistProfileEntity? ArtistProfile { get; set; }
        public OrganizerProfileEntity? OrganizerProfile { get; set; }

        public ICollection<OrganizerReviewEntity> OrganizerReviews { get; set; }
            = new List<OrganizerReviewEntity>();

        public ICollection<WishlistEntity> Wishlists { get; set; }
            = new List<WishlistEntity>();
    }
}
