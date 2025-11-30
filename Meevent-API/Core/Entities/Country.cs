namespace Meevent_API.Core.Entities
{
    public class Country
    {
        public int IdCountry { get; set; }
        public string NameCountry { get; set; }
        public string IsoCode { get; set; }

        // Country 1 - N City
        public ICollection<City> Cities { get; set; } = new List<City>();

    }
}
