using Microsoft.EntityFrameworkCore;
using EShoppingZone.Orders.API.Domain;

namespace EShoppingZone.Orders.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure OrderAddress as an owned entity (no separate table)
            modelBuilder.Entity<Order>().OwnsOne(o => o.Address, addr =>
            {
                addr.Property(a => a.CustomerId).HasColumnName("Address_CustomerId");
                addr.Property(a => a.FullName).HasColumnName("Address_FullName");
                addr.Property(a => a.MobileNumber).HasColumnName("Address_MobileNumber");
                addr.Property(a => a.FlatNumber).HasColumnName("Address_FlatNumber");
                addr.Property(a => a.City).HasColumnName("Address_City");
                addr.Property(a => a.Pincode).HasColumnName("Address_Pincode");
                addr.Property(a => a.State).HasColumnName("Address_State");
            });
        }
    }
}
