using EShoppingZone.Wallet.API.Domain;

namespace EShoppingZone.Wallet.API.Services
{
    public interface IWalletService
    {
        Task<EWallet?> GetWalletByIdAsync(int walletId);
        Task<IEnumerable<EWallet>> GetAllWalletsAsync();
        Task<IEnumerable<Statement>> GetStatementsByWalletIdAsync(int walletId);
        Task<EWallet> AddWalletAsync(EWallet wallet);
        Task AddMoneyAsync(int walletId, decimal amount);
        Task WithdrawMoneyAsync(int walletId, decimal amount);
        Task<EWallet> PayMoneyAsync(int walletId, decimal amount, int orderId);
        Task DeleteWalletAsync(int walletId);
    }
}
