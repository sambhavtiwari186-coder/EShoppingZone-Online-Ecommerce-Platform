using EShoppingZone.Wallet.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Wallet.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WalletDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Wallets.Any())
            {
                return;
            }

            var wallets = new EWallet[]
            {
                new EWallet { 
                    WalletId = 1, 
                    CurrentBalance = 10000.0m, 
                    Statements = new List<Statement>
                    {
                        new Statement { TransactionType = "CREDIT", Amount = 10000.0m, DateTime = DateTime.Now, TransactionRemarks = "Initial Balance" }
                    }
                }
            };

            foreach (var w in wallets)
            {
                context.Wallets.Add(w);
            }
            context.SaveChanges();
        }
    }
}
