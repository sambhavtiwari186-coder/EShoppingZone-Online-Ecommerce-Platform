using EShoppingZone.Orders.API.Data;
using EShoppingZone.Orders.API.Domain;
using EShoppingZone.Orders.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EShoppingZone.Orders.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public OrderService(
            IOrderRepository repository,
            OrderDbContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _repository = repository;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _repository.GetAllOrdersAsync();

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
            => await _repository.GetOrdersByCustomerAsync(customerId);

        public async Task<IEnumerable<OrderAddress>> GetAddressesByCustomerAsync(int customerId)
            => await _repository.GetAddressesByCustomerAsync(customerId);

        public async Task<OrderAddress> StoreAddressAsync(int orderId, OrderAddress address)
            => await _repository.StoreAddressAsync(orderId, address);

        public async Task<bool> ChangeStatusAsync(string status, int orderId)
            => await _repository.ChangeStatusAsync(status, orderId);

        public async Task<bool> CancelOrderAsync(int orderId)
            => await _repository.DeleteOrderAsync(orderId);

        /// <summary>
        /// PlaceOrder — COD. Uses EF Core transaction + inter-service calls (IHttpClientFactory).
        /// Decrements stock via Product-Service and sends notification.
        /// </summary>
        public async Task<Order> PlaceOrderAsync(Order order)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                order.ModeOfPayment = "COD";
                order.OrderStatus = "Placed";
                order.OrderDate = DateTime.Now;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer via Notify-Service
                await SendNotificationAsync(order.CustomerId, "ORDER_PLACED");

                await tx.CommitAsync();
                return order;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// PlaceOnlinePaymentOrder — deducts from wallet first, then places order.
        /// </summary>
        public async Task<Order> PlaceOnlinePaymentOrderAsync(Order order)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // Deduct from wallet via Wallet-Service
                await DeductWalletAsync(order.CustomerId, order.AmountPaid);

                order.ModeOfPayment = "ONLINE";
                order.OrderStatus = "Placed";
                order.OrderDate = DateTime.Now;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer
                await SendNotificationAsync(order.CustomerId, "ORDER_PLACED_ONLINE");

                await tx.CommitAsync();
                return order;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // --- Private inter-service helpers ---

        private async Task DecrementStockAsync(int productId, int qty)
        {
            var client = _httpClientFactory.CreateClient("ProductService");
            var productBaseUrl = _config["Services:ProductAPI"] ?? "http://localhost:5079";
            var response = await client.PutAsync(
                $"{productBaseUrl}/api/products/decrementStock/{productId}/{qty}", null);
            response.EnsureSuccessStatusCode();
        }

        private async Task SendNotificationAsync(int customerId, string eventType)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("NotifyService");
                var notifyBaseUrl = _config["Services:NotifyAPI"] ?? "http://localhost:5300";
                var payload = JsonSerializer.Serialize(new { CustomerId = customerId, EventType = eventType });
                var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync($"{notifyBaseUrl}/api/notify/send", content);
            }
            catch
            {
                // Notification failure should not block order placement
            }
        }

        private async Task DeductWalletAsync(int customerId, decimal amount)
        {
            var client = _httpClientFactory.CreateClient("WalletService");
            var walletBaseUrl = _config["Services:WalletAPI"] ?? "http://localhost:5200";
            var payload = JsonSerializer.Serialize(new { CustomerId = customerId, Amount = amount });
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{walletBaseUrl}/api/wallet/pay", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
