namespace Meevent_API.src.Core.Context;

using Meevent_API.src.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //ELTON
    public DbSet<User> Users { get; set; }
    public DbSet<ArtistProfile> ArtistProfiles { get; set; }
    public DbSet<OrganizerProfile> OrganizerProfiles { get; set; }
    public DbSet<OrganizerReview> OrganizerReviews { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }

    //LAYSSON
    public DbSet<Event> Events { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<EventSubCategory> EventSubCategories { get; set; }
    public DbSet<EventFollower> EventFollowers { get; set; }
    public DbSet<EventReview> EventReviews { get; set; }
    public DbSet<EventImage> EventImages { get; set; }

    //GABRIEL
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ELTON
        // 🟦 USERS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName).IsRequired();
            entity.Property(x => x.Email).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
        });

        // 🟩 ARTIST PROFILE (1-1 User → ArtistProfile)
        modelBuilder.Entity<ArtistProfile>(entity =>
        {
            entity.ToTable("artist_profile");
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.ArtistProfile)
                  .HasForeignKey<ArtistProfile>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟧 ORGANIZER PROFILE (1-1 User → OrganizerProfile)
        modelBuilder.Entity<OrganizerProfile>(entity =>
        {
            entity.ToTable("organizer_profile");
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.OrganizerProfile)
                  .HasForeignKey<OrganizerProfile>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟥 ORGANIZER REVIEWS (1-N User → Reviews y 1-N OrganizerProfile → Reviews)
        modelBuilder.Entity<OrganizerReview>(entity =>
        {
            entity.ToTable("organizer_review");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Comment).IsRequired();

            entity.HasOne(x => x.User)
                  .WithMany(u => u.OrganizerReviews)
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.OrganizerProfile)
                  .WithMany(o => o.OrganizerReviews)
                  .HasForeignKey(x => x.OrganizerProfileId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟪 WISHLIST (1-N User → Wishlist)
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.ToTable("wishlists");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ItemType).IsRequired();

            entity.HasOne(x => x.User)
                  .WithMany(u => u.Wishlists)
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // LAYSSON
        // 🟦 EVENT CATEGORIES
        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.ToTable("event_categories");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Slug).IsRequired();

            // 1-N EventCategory → EventSubCategory
            entity.HasMany(c => c.SubCategories)
                  .WithOne(sc => sc.Category)
                  .HasForeignKey(sc => sc.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟩 EVENT SUBCATEGORIES
        modelBuilder.Entity<EventSubCategory>(entity =>
        {
            entity.ToTable("event_subcategories");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Slug).IsRequired();

            // 1-N EventSubCategory → Event
            entity.HasMany(sc => sc.Events)
                  .WithOne(e => e.SubCategory)
                  .HasForeignKey(e => e.SubCategoryId)
                  .OnDelete(DeleteBehavior.Restrict); // Restrict para no borrar eventos si subcategory se borra
        });

        // 🟧 EVENTS
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title).IsRequired();
            entity.Property(x => x.Slug).IsRequired();
            entity.Property(x => x.Description).IsRequired();
            entity.Property(x => x.TimeZone).HasDefaultValue("UTC");
            entity.Property(x => x.Status).HasDefaultValue(EventStatus.Draft);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Relación con OrganizerProfile
            entity.HasOne(e => e.Organizer)
                  .WithMany()
                  .HasForeignKey(e => e.OrganizerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación 1-N con EventImage
            entity.HasMany(e => e.Images)
                  .WithOne(img => img.Event)
                  .HasForeignKey(img => img.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación 1-N con EventFollower
            entity.HasMany(e => e.Followers)
                  .WithOne(f => f.Event)
                  .HasForeignKey(f => f.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación 1-N con EventReview
            entity.HasMany(e => e.Reviews)
                  .WithOne(r => r.Event)
                  .HasForeignKey(r => r.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación 1-N con TicketType
            entity.HasMany(e => e.TicketTypes)
                  .WithOne(r => r.Event)
                  .HasForeignKey(r => r.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟪 EVENT FOLLOWERS
        modelBuilder.Entity<EventFollower>(entity =>
        {
            entity.ToTable("event_followers");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(f => f.Event)
                  .WithMany(e => e.Followers)
                  .HasForeignKey(f => f.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.User)
                  .WithMany()
                  .HasForeignKey(f => f.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟨 EVENT REVIEWS
        modelBuilder.Entity<EventReview>(entity =>
        {
            entity.ToTable("event_reviews");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Comment).IsRequired();
            entity.Property(x => x.Rating).IsRequired();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(r => r.Event)
                  .WithMany(e => e.Reviews)
                  .HasForeignKey(r => r.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.User)
                  .WithMany(u => u.EventReviews)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟧 EVENT IMAGES
        modelBuilder.Entity<EventImage>(entity =>
        {
            entity.ToTable("event_images");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Url).IsRequired();

            entity.HasOne(img => img.Event)
                  .WithMany(e => e.Images)
                  .HasForeignKey(img => img.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // GABRIEL
        // 🟨 TICKET TYPES
        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.ToTable("ticket_types");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

            // Relationships
            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.TicketType)
                .HasForeignKey(oi => oi.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Attendees)
                .WithOne(a => a.TicketType)
                .HasForeignKey(a => a.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 🟫 ORDERS
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(10,2)");
            entity.Property(e => e.ServiceFee).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Discount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CustomerEmail).HasMaxLength(255).IsRequired();
            entity.Property(e => e.CustomerName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PromoCode)
                .WithMany(pc => pc.Orders)
                .HasForeignKey(e => e.PromoCodeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Attendees)
                .WithOne(a => a.Order)
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.OrderNumber).IsUnique();
        });

        // 🟥 ORDER ITEMS
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
        });

        // 🟦 ATTENDEES
        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.ToTable("Attendees");
            entity.HasKey(e => e.AttendeeId);
            entity.Property(e => e.TicketNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.QrCode).HasMaxLength(255).IsRequired();

            // Indexes
            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.HasIndex(e => e.QrCode).IsUnique();
        });

        // 🟩 PAYMENTS
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(255);
            entity.Property(e => e.PaymentGateway).HasMaxLength(50);

            // Indexes
            entity.HasIndex(e => e.TransactionId);
        });

        // 🟨 PROMO CODES
        modelBuilder.Entity<PromoCode>(entity =>
        {
            entity.ToTable("PromoCodes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DiscountType).HasMaxLength(50);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(10,2)");
            entity.Property(e => e.MinimumPurchase).HasColumnType("decimal(10,2)");
            entity.Property(e => e.MaximumDiscount).HasColumnType("decimal(10,2)");

            // Indexes
            entity.HasIndex(e => e.Code).IsUnique();
        });
    }
}
