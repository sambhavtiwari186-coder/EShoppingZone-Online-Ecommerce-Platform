using EShoppingZone.Notify.API.Domain;

namespace EShoppingZone.Notify.API.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(int userId, string type, string title, string message, string? email = null);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> DeleteNotificationAsync(int id);
    }
}
