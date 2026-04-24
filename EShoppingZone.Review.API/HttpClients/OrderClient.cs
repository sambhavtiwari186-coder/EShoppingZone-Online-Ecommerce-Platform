namespace EShoppingZone.Review.API.HttpClients
{
    public interface IOrderClient
    {
        Task<bool> VerifyPurchaseAsync(int customerId, int productId);
    }

    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _httpClient;

        public OrderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyPurchaseAsync(int customerId, int productId)
        {
            var response = await _httpClient.GetAsync($"/api/orders/verifyPurchase/{customerId}/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return bool.TryParse(content, out var result) && result;
            }
            return false;
        }
    }
}
