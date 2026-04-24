using Microsoft.EntityFrameworkCore;
using EShoppingZone.Notify.API.Domain;

namespace EShoppingZone.Notify.API.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; } = null!;
    }
}
