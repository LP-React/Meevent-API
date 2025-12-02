namespace Meevent_API.src.Core.Entities
{
    public class WishlistEntity
    {
        public int IdWishlist { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        // Polimorfismo → tipo del item
        public string ItemType { get; set; }  // "event" | "plan"
        public int ItemId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
