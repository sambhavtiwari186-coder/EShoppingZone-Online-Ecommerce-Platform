using Microsoft.EntityFrameworkCore;
using EShoppingZone.Wallet.API.Domain;

namespace EShoppingZone.Wallet.API.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        public DbSet<EWallet> Wallets { get; set; }
        public DbSet<Statement> Statements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EWallet>()
                .HasMany(w => w.Statements)
                .WithOne()
                .HasForeignKey(s => s.WalletId);
        }
    }
}
