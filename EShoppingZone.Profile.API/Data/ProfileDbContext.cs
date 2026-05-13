using EShoppingZone.Profile.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Profile.API.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DeliveryAgent> DeliveryAgents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasKey(p => p.ProfileId);

            modelBuilder.Entity<UserProfile>()
                .Property(p => p.IsSuspended)
                .HasDefaultValue(false);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.Profile)
                .WithMany(p => p.Addresses)
                .HasForeignKey(a => a.ProfileId);
        }
    }
}
