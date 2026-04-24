using System.Text;
using System.Text.Json;

namespace EShoppingZone.Orders.API.HttpClients
{
    public interface ICartClient
    {
        Task<bool> ClearCartAsync(int userId);
    }

    public class CartClient : ICartClient
    {
        private readonly HttpClient _httpClient;

        public CartClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            // Update cart with empty list to clear it
            var content = new StringContent("[]", Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/carts/update?userId={userId}", content);
            return response.IsSuccessStatusCode;
        }
    }
}
