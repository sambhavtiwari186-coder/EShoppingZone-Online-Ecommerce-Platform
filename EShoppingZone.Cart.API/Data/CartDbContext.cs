using Microsoft.EntityFrameworkCore;
using EShoppingZone.Cart.API.Domain;

namespace EShoppingZone.Cart.API.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

        public DbSet<Domain.Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Define relationship between Cart and CartItem
            modelBuilder.Entity<Domain.Cart>()
                .HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
