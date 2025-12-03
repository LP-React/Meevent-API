namespace Meevent_API.src.Features.Entities
{
    public class PlanSubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int PlanCategoryId { get; set; }
        public PlanCategory PlanCategory { get; set; }

        public ICollection<Plan> Plans { get; set; }
    }

}
