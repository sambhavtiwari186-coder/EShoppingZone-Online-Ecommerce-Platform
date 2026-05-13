using System.Text.Json;

namespace EShoppingZone.Orders.API.HttpClients
{
    public interface IProductClient
    {
        Task<bool> DecrementStockAsync(int productId, int qty);
        Task<bool> IncrementStockAsync(int productId, int qty);
        Task<JsonElement?> GetProductByIdAsync(int productId);
    }

    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DecrementStockAsync(int productId, int qty)
        {
            var response = await _httpClient.PutAsync($"/api/products/decrementStock/{productId}/{qty}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IncrementStockAsync(int productId, int qty)
        {
            var response = await _httpClient.PutAsync($"/api/products/incrementStock/{productId}/{qty}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<JsonElement?> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}");
            if (!response.IsSuccessStatusCode) return null;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(content);
        }
    }
}
