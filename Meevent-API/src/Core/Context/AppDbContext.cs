namespace Meevent_API.src.Core.Context;

using Microsoft.EntityFrameworkCore;
using Meevent_API.src.Core.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ArtistProfileEntity> ArtistProfiles { get; set; }
    public DbSet<OrganizerProfileEntity> OrganizerProfiles { get; set; }
    public DbSet<OrganizerReviewEntity> OrganizerReviews { get; set; }
    public DbSet<WishlistEntity> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
        modelBuilder.Entity<ArtistProfileEntity>(entity =>
        {
            entity.ToTable("ArtistProfiles");
            entity.HasKey(x => x.IdArtistProfile);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.ArtistProfile)
                  .HasForeignKey<ArtistProfileEntity>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟧 ORGANIZER PROFILE (1-1 User → OrganizerProfile)
        modelBuilder.Entity<OrganizerProfileEntity>(entity =>
        {
            entity.ToTable("OrganizerProfiles");
            entity.HasKey(x => x.IdOrganizerProfile);

            entity.HasOne(x => x.User)
                  .WithOne(u => u.OrganizerProfile)
                  .HasForeignKey<OrganizerProfileEntity>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🟥 ORGANIZER REVIEWS (1-N User → Reviews y 1-N OrganizerProfile → Reviews)
        modelBuilder.Entity<OrganizerReviewEntity>(entity =>
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
        modelBuilder.Entity<WishlistEntity>(entity =>
        {
            entity.ToTable("Wishlists");
            entity.HasKey(x => x.IdWishlist);

            entity.Property(x => x.ItemType).IsRequired();

            entity.HasOne(x => x.User)
                  .WithMany(u => u.Wishlists)
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
