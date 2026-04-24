using System.Text.Json;
using System.Text;

namespace EShoppingZone.Wallet.API.HttpClients
{
    public interface INotifyClient
    {
        Task SendNotificationAsync(int userId, string type, string title, string message);
    }

    public class NotifyClient : INotifyClient
    {
        private readonly HttpClient _httpClient;

        public NotifyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendNotificationAsync(int userId, string type, string title, string message)
        {
            var request = new
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/notifications", content);
        }
    }
}
