using Microsoft.Extensions.Logging;

namespace Meevent_API.Core.Entities
{
    public class TicketType
    {
        public int TicketTypeId { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int QuantitySold { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int MinPurchase { get; set; } = 1;
        public int MaxPurchase { get; set; } = 10;
        public bool IsActive { get; set; } = true;

        // Descomentar cuando se cree la entidad Event
        // public Event Event { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
        
    }
}
