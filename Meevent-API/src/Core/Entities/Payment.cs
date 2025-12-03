namespace Meevent_API.src.Core.Entities

{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // PayPal, MercadoPago, Yape, etc.
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        public string? TransactionId { get; set; }
        public string? PaymentGateway { get; set; } // Stripe, PayPal, MercadoPago, etc.
        public string? PaymentGatewayResponse { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public DateTime? RefundedAt { get; set; }

        // RELATIONS
        public int OrderId { get; set; }

        // NAVIGATIONS
        public Order Order { get; set; } = null!;
    }
}
