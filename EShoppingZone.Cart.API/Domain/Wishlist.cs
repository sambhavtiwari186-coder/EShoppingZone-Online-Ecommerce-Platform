using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Cart.API.Domain
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
