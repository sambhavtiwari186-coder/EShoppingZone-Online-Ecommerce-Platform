using EShoppingZone.Wallet.API.Domain;

namespace EShoppingZone.Wallet.API.Repositories
{
    public interface IWalletRepository
    {
        Task<EWallet?> GetWalletByIdAsync(int walletId);
        Task<IEnumerable<EWallet>> GetAllWalletsAsync();
        Task<IEnumerable<Statement>> GetStatementsByWalletIdAsync(int walletId);
        Task AddWalletAsync(EWallet wallet);
        Task UpdateWalletAsync(EWallet wallet);
        Task DeleteWalletAsync(int walletId);
        Task SaveChangesAsync();
        Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync();
    }
}
