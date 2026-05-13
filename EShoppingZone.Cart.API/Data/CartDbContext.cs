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

            // Seed Data for Cart
            modelBuilder.Entity<Domain.Cart>().HasData(
                new Domain.Cart { CartId = 1, TotalPrice = 148998 }
            );

            modelBuilder.Entity<CartItem>().HasData(
                new CartItem { CartItemId = 1, CartId = 1, ProductId = 1, ProductName = "iPhone 15 Pro", Price = 129999, Quantity = 1, ImageUrl = "https://images.unsplash.com/photo-1696446701796-da61225697cc?q=80&w=1200" },
                new CartItem { CartItemId = 2, CartId = 1, ProductId = 5, ProductName = "Nike Air Jordan 1", Price = 18999, Quantity = 1, ImageUrl = "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?q=80&w=1200" }
            );
        }
    }
}
