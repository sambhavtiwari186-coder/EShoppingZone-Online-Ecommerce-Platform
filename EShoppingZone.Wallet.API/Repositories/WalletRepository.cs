using EShoppingZone.Wallet.API.Data;
using EShoppingZone.Wallet.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EShoppingZone.Wallet.API.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;

        public WalletRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<EWallet?> GetWalletByIdAsync(int walletId)
        {
            return await _context.Wallets.Include(w => w.Statements)
                .FirstOrDefaultAsync(w => w.WalletId == walletId);
        }

        public async Task<IEnumerable<EWallet>> GetAllWalletsAsync()
        {
            return await _context.Wallets.Include(w => w.Statements).ToListAsync();
        }

        public async Task<IEnumerable<Statement>> GetStatementsByWalletIdAsync(int walletId)
        {
            return await _context.Statements
                .Where(s => s.WalletId == walletId)
                .OrderByDescending(s => s.DateTime)
                .ToListAsync();
        }

        public async Task AddWalletAsync(EWallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
        }

        public async Task UpdateWalletAsync(EWallet wallet)
        {
            _context.Wallets.Update(wallet);
            await Task.CompletedTask;
        }

        public async Task DeleteWalletAsync(int walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
