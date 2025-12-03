namespace Meevent_API.src.Features.Entities
{
    public class PlanCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PlanSubCategory> SubCategories { get; set; }
    }

}
