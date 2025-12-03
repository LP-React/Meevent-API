namespace Meevent_API.src.Features.Entities
{
    public class PlanFollower
    {
        public int Id { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        public int UserId { get; set; }
        
    }

}
