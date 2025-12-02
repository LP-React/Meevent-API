namespace Meevent_API.src.Core.Context;

using Microsoft.EntityFrameworkCore;
using Meevent_API.src.Core.Entities;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ELTON
        // 🟦 USERS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.IdUser);

            entity.Property(x => x.FullName).IsRequired();
            entity.Property(x => x.Email).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
        });

        // 🟩 ARTIST PROFILE (1-1 User → ArtistProfile)
        modelBuilder.Entity<ArtistProfile>(entity =>
        {
            entity.ToTable("ArtistProfiles");
            entity.HasKey(x => x.IdArtistProfile);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.ArtistProfile)
                  .HasForeignKey<ArtistProfile>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟧 ORGANIZER PROFILE (1-1 User → OrganizerProfile)
        modelBuilder.Entity<OrganizerProfile>(entity =>
        {
            entity.ToTable("OrganizerProfiles");
            entity.HasKey(x => x.IdOrganizerProfile);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.OrganizerProfile)
                  .HasForeignKey<OrganizerProfile>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟥 ORGANIZER REVIEWS (1-N User → Reviews y 1-N OrganizerProfile → Reviews)
        modelBuilder.Entity<OrganizerReview>(entity =>
        {
            entity.ToTable("OrganizerReviews");
            entity.HasKey(x => x.IdOrganizerReview);

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
            entity.ToTable("Wishlists");
            entity.HasKey(x => x.IdWishlist);

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
            entity.ToTable("EventCategories");
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
            entity.ToTable("EventSubCategories");
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
            entity.ToTable("Events");
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
        });

        // 🟪 EVENT FOLLOWERS
        modelBuilder.Entity<EventFollower>(entity =>
        {
            entity.ToTable("EventFollowers");
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
            entity.ToTable("EventReviews");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Comment).IsRequired();
            entity.Property(x => x.Rating).IsRequired();
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(r => r.Event)
                  .WithMany(e => e.Reviews)
                  .HasForeignKey(r => r.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Si quieres relacionarlo con User en el futuro:
            // entity.HasOne(r => r.User)
            //       .WithMany()
            //       .HasForeignKey(r => r.UserId)
            //       .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟧 EVENT IMAGES
        modelBuilder.Entity<EventImage>(entity =>
        {
            entity.ToTable("EventImages");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Url).IsRequired();

            entity.HasOne(img => img.Event)
                  .WithMany(e => e.Images)
                  .HasForeignKey(img => img.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
