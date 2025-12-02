namespace Meevent_API.src.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        // RELATIONS
        public int OrderId { get; set; }
        public int TicketTypeId { get; set; }


        // NAVIGATIONS
        public Order Order { get; set; } = null!;
        public TicketType TicketType { get; set; } = null!;
    }
}
