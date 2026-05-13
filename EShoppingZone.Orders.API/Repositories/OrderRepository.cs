using EShoppingZone.Orders.API.Data;
using EShoppingZone.Orders.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Orders.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Address).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders.Include(o => o.Address).Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.Address).FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> ChangeStatusAsync(string status, int orderId)
        {
            // Atomic update — no full entity load, uses ExecuteUpdateAsync as per spec
            var updated = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .ExecuteUpdateAsync(s => s.SetProperty(o => o.OrderStatus, status));
            return updated > 0;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderAddress>> GetAddressesByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId && o.Address != null)
                .Select(o => o.Address!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<OrderAddress> StoreAddressAsync(int orderId, OrderAddress address)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            order.Address = address;
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
