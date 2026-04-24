namespace EShoppingZone.Orders.API.HttpClients
{
    public interface IProductClient
    {
        Task<bool> DecrementStockAsync(int productId, int qty);
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
    }
}
