using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Product.API.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int StockQuantity { get; set; }

        // Complex JSON fields
        public int MerchantId { get; set; }
        public Dictionary<string, string>? Rating { get; set; }
        public Dictionary<string, string>? Review { get; set; }
        public List<string>? Image { get; set; }
        public Dictionary<string, string>? Specification { get; set; }
    }
}
