using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Cart.API.Domain
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        
        // Foreign Key to Cart
        public int CartId { get; set; }
    }
}
