namespace Meevent_API.src.Core.Entities

{
    public class Attendee
    {
        public string TicketNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool CheckedIn { get; set; } = false;
        public DateTime? CheckedInAt { get; set; }
        public string QrCode { get; set; } = string.Empty;

        // RELATIONS
        public int AttendeeId { get; set; }
        public int OrderId { get; set; }
        public int TicketTypeId { get; set; }

        // NAVIGATIONS
        public Order Order { get; set; } = null!;
        public TicketType TicketType { get; set; } = null!;
    }
}
