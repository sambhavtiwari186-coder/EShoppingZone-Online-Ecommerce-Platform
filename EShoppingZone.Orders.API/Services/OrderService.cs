using EShoppingZone.Orders.API.Data;
using EShoppingZone.Orders.API.Domain;
using EShoppingZone.Orders.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using EShoppingZone.Orders.API.HttpClients;
using Microsoft.Extensions.Logging;
using MassTransit;
using EShoppingZone.Orders.API.Events;

namespace EShoppingZone.Orders.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly OrderDbContext _context;
        private readonly IProductClient _productClient;
        private readonly IWalletClient _walletClient;
        private readonly INotifyClient _notifyClient;
        private readonly ICartClient _cartClient;
        private readonly ILogger<OrderService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(
            IOrderRepository repository,
            OrderDbContext context,
            IProductClient productClient,
            IWalletClient walletClient,
            INotifyClient notifyClient,
            ICartClient cartClient,
            ILogger<OrderService> logger,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _context = context;
            _productClient = productClient;
            _walletClient = walletClient;
            _notifyClient = notifyClient;
            _cartClient = cartClient;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
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
        {
            _logger.LogInformation("AUDIT: Changing status of Order {OrderId} to {Status}", orderId, status);
            return await _repository.ChangeStatusAsync(status, orderId);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            if (order.OrderStatus == "Cancelled") return true; // Already cancelled

            var success = await _repository.ChangeStatusAsync("Cancelled", orderId);
            if (success)
            {
                _logger.LogInformation("AUDIT: Order {OrderId} cancelled by user/admin", orderId);

                // 1. Replenish stock
                try {
                    await _productClient.IncrementStockAsync(order.ProductId, order.Quantity);
                } catch (Exception ex) {
                    _logger.LogWarning(ex, "Failed to replenish stock for cancelled Order {OrderId}", orderId);
                }

                // 2. Refund to wallet if paid online
                if (order.ModeOfPayment == "ONLINE")
                {
                    try {
                        await _walletClient.AddMoneyAsync(order.CustomerId, order.AmountPaid);
                        _logger.LogInformation("AUDIT: Refunded {Amount} to Customer {CustomerId} for cancelled Order {OrderId}", order.AmountPaid, order.CustomerId, orderId);
                    } catch (Exception ex) {
                        _logger.LogError(ex, "FAILED REFUND for Order {OrderId}", orderId);
                    }
                }

                // 3. Notify customer
                await SendNotificationAsync(order.CustomerId, "ORDER", "Order Cancelled", $"Your order #{orderId} has been cancelled and refund initiated.");
            }

            return success;
        }

        public async Task<bool> VerifyPurchaseAsync(int customerId, int productId)
        {
            var orders = await _repository.GetOrdersByCustomerAsync(customerId);
            return orders.Any(o => o.ProductId == productId);
        }


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

                _logger.LogInformation("AUDIT: Placing new COD Order for Customer {CustomerId}, Product {ProductId}, Amount {Amount}", order.CustomerId, order.ProductId, order.Price);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer via Notify-Service
                await SendNotificationAsync(order.CustomerId, "ORDER", "Order Placed", $"Your order for product #{order.ProductId} has been placed successfully (COD).");

                // Clear cart
                await _cartClient.ClearCartAsync(order.CustomerId);

                // Publish Event to RabbitMQ
                await PublishOrderPlacedEvent(order);


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
                var paySuccess = await _walletClient.PayMoneyAsync(order.CustomerId, order.AmountPaid, order.OrderId);
                if (!paySuccess) throw new Exception("Insufficient wallet balance or wallet service error.");


                order.ModeOfPayment = "ONLINE";
                order.OrderStatus = "Placed";
                order.OrderDate = DateTime.Now;

                _logger.LogInformation("AUDIT: Placing new ONLINE Order for Customer {CustomerId}, Product {ProductId}, Amount {Amount}", order.CustomerId, order.ProductId, order.Price);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer
                await SendNotificationAsync(order.CustomerId, "ORDER", "Order Placed Online", $"Your order for product #{order.ProductId} has been placed successfully (Paid Online).");

                // Clear cart
                await _cartClient.ClearCartAsync(order.CustomerId);

                // Publish Event to RabbitMQ
                await PublishOrderPlacedEvent(order);


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
            var success = await _productClient.DecrementStockAsync(productId, qty);
            if (!success) throw new Exception("Failed to decrement stock via Product-Service");
        }

        private async Task SendNotificationAsync(int customerId, string eventType, string title, string message)
        {
            try
            {
                await _notifyClient.SendNotificationAsync(customerId, eventType, title, message);
            }
            catch
            {
                // Notification failure should not block order placement
            }
        }

        private async Task PublishOrderPlacedEvent(Order order)
        {
            try
            {
                var productJson = await _productClient.GetProductByIdAsync(order.ProductId);
                int merchantId = 0;
                if (productJson.HasValue && productJson.Value.TryGetProperty("merchantId", out var merchantIdProp))
                {
                    merchantId = merchantIdProp.GetInt32();
                }

                var @event = new OrderPlacedEvent
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    MerchantId = merchantId,
                    Amount = order.AmountPaid,
                    ProductName = order.ProductName,
                    OrderDate = order.OrderDate,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity
                };

                await _publishEndpoint.Publish(@event);
                _logger.LogInformation("PUBLISHED OrderPlacedEvent for OrderId: {OrderId}", order.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish OrderPlacedEvent for OrderId: {OrderId}", order.OrderId);
            }
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(o => o.OrderStatus != "Cancelled")
                .SumAsync(o => o.AmountPaid);
        }

        public async Task<IEnumerable<object>> GetTopProductsAsync(int count)
        {
            var top = await _context.Orders
                .GroupBy(o => new { o.ProductId, o.ProductName })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToListAsync();
            
            return top.Cast<object>();
        }
    }
}
