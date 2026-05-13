using System;

namespace EShoppingZone.Cart.API.Events
{
    public class OrderPlacedEvent
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
