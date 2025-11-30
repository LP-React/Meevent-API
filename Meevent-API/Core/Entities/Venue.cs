namespace Meevent_API.Core.Entities
{
    public class Venue
    {
        public int IdVenue { get; set; }
        public string NameVenue { get; set; }
        public int Capacity { get; set; }
        public string AddressVenue { get; set; }

        // Relación con City (FK)
        public int IdCity { get; set; }
        public City City { get; set; }

        // Venue 1 - N Event
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
