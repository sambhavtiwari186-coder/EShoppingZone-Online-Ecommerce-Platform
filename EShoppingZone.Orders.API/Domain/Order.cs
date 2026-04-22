using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Orders.API.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public decimal AmountPaid { get; set; }
        public string ModeOfPayment { get; set; } = string.Empty; // "COD" or "ONLINE"
        public string OrderStatus { get; set; } = "Placed";
        public int Quantity { get; set; }

        // Product snapshot (stored at order time)
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Owned entity - delivery address
        public OrderAddress? Address { get; set; }
    }

    public class OrderAddress
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public long MobileNumber { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Pincode { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
