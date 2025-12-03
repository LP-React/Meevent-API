namespace Meevent_API.src.Core.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }

        // Polimorfismo → tipo del item
        public string ItemType { get; set; }  // "event" | "plan"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // RELATIONS
        public int UserId { get; set; }
        public int ItemId { get; set; }

        // NAVIGATIONS
        public User User { get; set; }

    }
}
