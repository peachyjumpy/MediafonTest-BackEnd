using Microsoft.EntityFrameworkCore;
using Mediafon.Api.Models;

namespace Mediafon.Api.Data
{
    public class MediafonDbContext: DbContext
    {
        public MediafonDbContext(DbContextOptions<MediafonDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationRequest> ApplicationRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<ApplicationRequest>()
                .HasOne(ar => ar.User)
                .WithMany(u => u.ApplicationRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationRequest>()
                .ToTable(t => t.HasCheckConstraint("CK_ApplicationRequest_Type", "\"Type\" IN ('request', 'offer', 'complaint')"));

            modelBuilder.Entity<ApplicationRequest>()
                .ToTable(t => t.HasCheckConstraint("CK_ApplicationRequest_Status", "\"Status\" IN ('submitted', 'completed')"));

        }

    }
}
