using EShoppingZone.Notify.API.Data;
using EShoppingZone.Notify.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Notify.API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAllNotificationsAsync(IEnumerable<Notification> notifications)
        {
            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
