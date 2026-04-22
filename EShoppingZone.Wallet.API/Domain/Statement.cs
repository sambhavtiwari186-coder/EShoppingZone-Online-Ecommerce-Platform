using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Wallet.API.Domain
{
    public class Statement
    {
        [Key]
        public int StatementId { get; set; }
        public string TransactionType { get; set; } = string.Empty; // CREDIT/DEBIT
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public int OrderId { get; set; }
        public string TransactionRemarks { get; set; } = string.Empty;

        // Foreign Key
        public int WalletId { get; set; }
    }
}
