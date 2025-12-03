namespace Meevent_API.src.Features.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int PlanSubCategoryId { get; set; }
        public PlanSubCategory PlanSubCategory { get; set; }

        public ICollection<PlanImage> Images { get; set; }
        public ICollection<PlanFollower> Followers { get; set; }
    }

}
