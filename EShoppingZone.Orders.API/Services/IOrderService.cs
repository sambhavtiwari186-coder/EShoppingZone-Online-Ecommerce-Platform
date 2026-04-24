using EShoppingZone.Orders.API.Domain;

namespace EShoppingZone.Orders.API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderAddress>> GetAddressesByCustomerAsync(int customerId);
        Task<Order> PlaceOrderAsync(Order order);
        Task<Order> PlaceOnlinePaymentOrderAsync(Order order);
        Task<OrderAddress> StoreAddressAsync(int orderId, OrderAddress address);
        Task<bool> ChangeStatusAsync(string status, int orderId);
        Task<bool> CancelOrderAsync(int orderId);
        Task<bool> VerifyPurchaseAsync(int customerId, int productId);

    }
}
