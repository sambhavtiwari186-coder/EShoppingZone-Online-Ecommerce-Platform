using EShoppingZone.Wallet.API.Domain;
using EShoppingZone.Wallet.API.Repositories;
using EShoppingZone.Wallet.API.HttpClients;
using Microsoft.Extensions.Logging;


namespace EShoppingZone.Wallet.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _repository;
        private readonly INotifyClient _notifyClient;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IWalletRepository repository, INotifyClient notifyClient, ILogger<WalletService> logger)
        {
            _repository = repository;
            _notifyClient = notifyClient;
            _logger = logger;
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

            _logger.LogInformation("AUDIT: Wallet {WalletId} credited with {Amount}", walletId, amount);

            await _repository.UpdateWalletAsync(wallet);
            await _repository.SaveChangesAsync();

            // Notify customer
            await _notifyClient.SendNotificationAsync(walletId, "WALLET", "Wallet Credited", $"Your wallet has been credited with {amount}");
        }


        public async Task WithdrawMoneyAsync(int walletId, decimal amount)
        {
            var wallet = await _repository.GetWalletByIdAsync(walletId);
            if (wallet == null) throw new KeyNotFoundException("Wallet not found");

            if (wallet.CurrentBalance < amount)
                throw new InvalidOperationException("Insufficient balance for withdrawal");

            wallet.CurrentBalance -= amount;
            wallet.Statements.Add(new Statement
            {
                TransactionType = "DEBIT",
                Amount = amount,
                DateTime = DateTime.UtcNow,
                TransactionRemarks = "Money withdrawn from wallet"
            });

            _logger.LogInformation("AUDIT: Wallet {WalletId} debited with {Amount} (Withdrawal)", walletId, amount);

            await _repository.UpdateWalletAsync(wallet);
            await _repository.SaveChangesAsync();

            // Notify customer
            await _notifyClient.SendNotificationAsync(walletId, "WALLET", "Wallet Withdrawal", $"Your wallet has been debited with {amount} due to withdrawal.");
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

                _logger.LogInformation("AUDIT: Wallet {WalletId} debited with {Amount} for Order {OrderId}", walletId, amount, orderId);

                await _repository.UpdateWalletAsync(wallet);
                await _repository.SaveChangesAsync();
                await tx.CommitAsync();

                // Notify customer
                await _notifyClient.SendNotificationAsync(walletId, "WALLET", "Wallet Debited", $"Your wallet has been debited with {amount} for order {orderId}");

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
