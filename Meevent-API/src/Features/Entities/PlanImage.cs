namespace Meevent_API.src.Features.Entities
{
    public class PlanImage
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }

}
