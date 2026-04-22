using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShoppingZone.Cart.API.Domain
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartId { get; set; } // Matches UserId
        
        public decimal TotalPrice { get; set; }
        
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
