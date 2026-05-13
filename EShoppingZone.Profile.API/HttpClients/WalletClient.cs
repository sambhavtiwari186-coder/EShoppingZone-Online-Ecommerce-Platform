using System.Net.Http.Json;

namespace EShoppingZone.Profile.API.HttpClients
{
    public interface IWalletClient
    {
        Task<bool> CreateWalletAsync(int profileId);
    }

    public class WalletClient : IWalletClient
    {
        private readonly HttpClient _httpClient;

        public WalletClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateWalletAsync(int profileId)
        {
            var walletRequest = new { WalletId = profileId, CurrentBalance = 0m };
            var response = await _httpClient.PostAsJsonAsync("api/wallet/addNew", walletRequest);
            return response.IsSuccessStatusCode;
        }
    }
}
