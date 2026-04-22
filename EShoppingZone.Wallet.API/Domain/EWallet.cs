using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Wallet.API.Domain
{
    public class EWallet
    {
        [Key]
        public int WalletId { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<Statement> Statements { get; set; } = new List<Statement>();
    }
}
