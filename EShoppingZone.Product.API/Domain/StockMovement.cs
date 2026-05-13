using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Product.API.Domain
{
    public class StockMovement
    {
        [Key]
        public int MovementId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int MerchantId { get; set; }
        public int Change { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // e.g., "Sold", "Added", "Updated"
    }
}
