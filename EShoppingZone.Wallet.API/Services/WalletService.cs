using EShoppingZone.Wallet.API.Domain;
using EShoppingZone.Wallet.API.Repositories;

namespace EShoppingZone.Wallet.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _repository;

        public WalletService(IWalletRepository repository)
        {
            _repository = repository;
        }

        public async Task<EWallet?> GetWalletByIdAsync(int walletId)
        {
            return await _repository.GetWalletByIdAsync(walletId);
        }

        public async Task<IEnumerable<EWallet>> GetAllWalletsAsync()
        {
            return await _repository.GetAllWalletsAsync();
        }

        public async Task<IEnumerable<Statement>> GetStatementsByWalletIdAsync(int walletId)
        {
            return await _repository.GetStatementsByWalletIdAsync(walletId);
        }

        public async Task<EWallet> AddWalletAsync(EWallet wallet)
        {
            await _repository.AddWalletAsync(wallet);
            await _repository.SaveChangesAsync();
            return wallet;
        }

        public async Task AddMoneyAsync(int walletId, decimal amount)
        {
            var wallet = await _repository.GetWalletByIdAsync(walletId);
            if (wallet == null) throw new KeyNotFoundException("Wallet not found");

            wallet.CurrentBalance += amount;
            wallet.Statements.Add(new Statement
            {
                TransactionType = "CREDIT",
                Amount = amount,
                DateTime = DateTime.UtcNow,
                TransactionRemarks = "Money added to wallet"
            });

            await _repository.UpdateWalletAsync(wallet);
            await _repository.SaveChangesAsync();
        }

        public async Task<EWallet> PayMoneyAsync(int walletId, decimal amount, int orderId)
        {
            using var tx = await _repository.BeginTransactionAsync();
            try
            {
                var wallet = await _repository.GetWalletByIdAsync(walletId);
                if (wallet == null) throw new KeyNotFoundException("Wallet not found");

                if (wallet.CurrentBalance < amount)
                    throw new InvalidOperationException("Insufficient balance");

                wallet.CurrentBalance -= amount;
                wallet.Statements.Add(new Statement
                {
                    TransactionType = "DEBIT",
                    Amount = amount,
                    DateTime = DateTime.UtcNow,
                    OrderId = orderId,
                    TransactionRemarks = $"Payment for order {orderId}"
                });

                await _repository.UpdateWalletAsync(wallet);
                await _repository.SaveChangesAsync();
                await tx.CommitAsync();
                return wallet;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteWalletAsync(int walletId)
        {
            await _repository.DeleteWalletAsync(walletId);
            await _repository.SaveChangesAsync();
        }
    }
}
