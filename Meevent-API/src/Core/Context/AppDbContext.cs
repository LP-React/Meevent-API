using Microsoft.EntityFrameworkCore;
using Meevent_API.src.Features.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Meevent_API.Core.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)        {
        }




   
        public DbSet<PlanCategory> PlanCategories { get; set; }
        public DbSet<PlanSubCategory> PlanSubCategories { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanImage> PlanImages { get; set; }
        public DbSet<PlanFollower> PlanFollowers { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

       
            modelBuilder.Entity<PlanCategory>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.PlanCategory)
                .HasForeignKey(sc => sc.PlanCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<PlanSubCategory>()
                .HasMany(sc => sc.Plans)
                .WithOne(p => p.PlanSubCategory)
                .HasForeignKey(p => p.PlanSubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Plan)
                .HasForeignKey(i => i.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

       
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Followers)
                .WithOne(f => f.Plan)
                .HasForeignKey(f => f.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

         
      

       
            modelBuilder.Entity<PlanFollower>()
                .HasIndex(f => new { f.PlanId, f.UserId })
                .IsUnique();

            modelBuilder.Entity<PlanCategory>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PlanSubCategory>()
                .Property(sc => sc.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Plan>()
                .Property(p => p.Title)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
