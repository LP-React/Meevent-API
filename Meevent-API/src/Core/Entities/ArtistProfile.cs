namespace Meevent_API.src.Core.Entities
{
    public class ArtistProfile
    {
        public int IdArtistProfile { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string StageName { get; set; }
        public string Biography { get; set; }
        public string Genre { get; set; }

        public string? Website { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TiktokUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
