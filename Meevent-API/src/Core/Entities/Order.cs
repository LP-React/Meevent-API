namespace Meevent_API.src.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled, Refunded
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }

        // RELATIONS
        public int UserId { get; set; }
        public int? PromoCodeId { get; set; }

        // NAVIGATIONS
        public User User { get; set; } = null!;
        public Payment Payment { get; set; } = null!;
        public PromoCode? PromoCode { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
