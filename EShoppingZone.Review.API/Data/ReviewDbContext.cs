using Microsoft.EntityFrameworkCore;
using EShoppingZone.Review.API.Domain;

namespace EShoppingZone.Review.API.Data
{
    public class ReviewDbContext : DbContext
    {
        public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Review> Reviews { get; set; }
        public DbSet<ReviewVote> ReviewVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
