namespace Meevent_API.Core.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
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

        // Navigation properties
        public Order Order { get; set; } = null!;
    }
}
