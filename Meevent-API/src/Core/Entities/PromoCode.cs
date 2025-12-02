namespace Meevent_API.Core.Entities
{
    public class PromoCode
    {
        public int PromoCodeId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = "Percentage"; // Percentage, FixedAmount
        public decimal DiscountValue { get; set; }
        public decimal? MinimumPurchase { get; set; }
        public decimal? MaximumDiscount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
