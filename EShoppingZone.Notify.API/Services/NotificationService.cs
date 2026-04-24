using EShoppingZone.Notify.API.Domain;
using EShoppingZone.Notify.API.Repositories;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EShoppingZone.Notify.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository repository, ILogger<NotificationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Notification> CreateNotificationAsync(int userId, string type, string title, string message, string? email = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateNotificationAsync(notification);

            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    await SendEmailNotification(email, title, message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {Email}", email);
                }
            }

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _repository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _repository.GetUnreadCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _repository.GetNotificationByIdAsync(id);
            if (notification == null) return false;

            notification.IsRead = true;
            await _repository.UpdateNotificationAsync(notification);
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = (await _repository.GetNotificationsByUserIdAsync(userId))
                .Where(n => !n.IsRead);

            var unreadList = notifications.ToList();
            if (!unreadList.Any()) return true;

            foreach (var notification in unreadList)
            {
                notification.IsRead = true;
            }

            await _repository.UpdateAllNotificationsAsync(unreadList);
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _repository.GetNotificationByIdAsync(id);
            if (notification == null) return false;

            await _repository.DeleteNotificationAsync(notification);
            return true;
        }

        // MailKit email notification (works on Arch Linux, zero config)
        public async Task SendEmailNotification(string toEmail, string subject, string body)
        {
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EShoppingZone", "no-reply@eshoppingzone.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            // Mailtrap for dev - replace with real SMTP for prod
            await client.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("MAILTRAP_USER", "MAILTRAP_PASS");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
