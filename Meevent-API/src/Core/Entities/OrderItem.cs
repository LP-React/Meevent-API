namespace Meevent_API.Core.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int TicketTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        // Navigation properties
        public Order Order { get; set; } = null!;
        public TicketType TicketType { get; set; } = null!;
    }
}
