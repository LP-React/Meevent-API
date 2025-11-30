namespace Meevent_API.Core.Entities
{
    public class City
    {
        public int IdCity { get; set; }
        public string NameCity { get; set; }

        // Relación con Country (FK)
        public int IdCountry { get; set; }
        public Country Country { get; set; }

        public ICollection<Venue> Venues { get; set; } = new List<Venue>();

    }
}
