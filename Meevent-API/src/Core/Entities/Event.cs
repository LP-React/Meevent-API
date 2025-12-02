namespace Meevent_API.src.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }

        // Relaciones
        public int OrganizerId { get; set; }           // OrganizerProfile
        public int SubCategoryId { get; set; }          // EventSubCategory
        public int VenueId { get; set; }


        // Datos principales
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ShortDescription { get; set; }

        // Fechas
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string TimeZone { get; set; } = "UTC";

        // Estado y configuración
        public EventStatus Status { get; set; } = EventStatus.Draft;
        public int Capacity { get; set; }
        public bool IsFree { get; set; }
        public bool IsOnline { get; set; }

        // Media
        public string? CoverImageUrl { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navegación
        public EventSubCategory? SubCategory { get; set; }
        public VenueEntity? Venue { get; set; }
        public OrganizerProfileEntity? Organizer { get; set; }

        public ICollection<EventImage>? Images { get; set; }
        public ICollection<EventFollower>? Followers { get; set; }
        public ICollection<EventReview>? Reviews { get; set; }
        public ICollection<TicketTypeEntity>? TicketTypes { get; set; }
    }

    public enum EventStatus
    {
        Draft = 0,
        Published = 1,
        Canceled = 2,
        Finished = 3
    }
}
