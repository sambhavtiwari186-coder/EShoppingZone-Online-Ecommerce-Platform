namespace EShoppingZone.Orders.API.HttpClients
{
    public interface IWalletClient
    {
        Task<bool> PayMoneyAsync(int customerId, decimal amount, int orderId);
    }

    public class WalletClient : IWalletClient
    {
        private readonly HttpClient _httpClient;

        public WalletClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PayMoneyAsync(int customerId, decimal amount, int orderId)
        {
            var response = await _httpClient.PostAsync($"/api/wallet/payMoney/{customerId}/{amount}/{orderId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
