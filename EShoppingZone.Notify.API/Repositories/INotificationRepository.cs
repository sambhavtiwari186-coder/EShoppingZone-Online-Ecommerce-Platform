using EShoppingZone.Notify.API.Domain;

namespace EShoppingZone.Notify.API.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task UpdateNotificationAsync(Notification notification);
        Task UpdateAllNotificationsAsync(IEnumerable<Notification> notifications);
        Task DeleteNotificationAsync(Notification notification);
    }
}
