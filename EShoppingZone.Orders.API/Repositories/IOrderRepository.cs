using EShoppingZone.Orders.API.Domain;

namespace EShoppingZone.Orders.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> ChangeStatusAsync(string status, int orderId);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderAddress>> GetAddressesByCustomerAsync(int customerId);
        Task<OrderAddress> StoreAddressAsync(int orderId, OrderAddress address);
    }
}
