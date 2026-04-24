using EShoppingZone.Orders.API.Data;
using EShoppingZone.Orders.API.Domain;
using EShoppingZone.Orders.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using EShoppingZone.Orders.API.HttpClients;


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

        public OrderService(
            IOrderRepository repository,
            OrderDbContext context,
            IProductClient productClient,
            IWalletClient walletClient,
            INotifyClient notifyClient,
            ICartClient cartClient)
        {
            _repository = repository;
            _context = context;
            _productClient = productClient;
            _walletClient = walletClient;
            _notifyClient = notifyClient;
            _cartClient = cartClient;
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

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer via Notify-Service
                await SendNotificationAsync(order.CustomerId, "ORDER_PLACED");

                // Clear cart
                await _cartClient.ClearCartAsync(order.CustomerId);


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
                await DeductWalletAsync(order.CustomerId, order.AmountPaid, order.OrderId);


                order.ModeOfPayment = "ONLINE";
                order.OrderStatus = "Placed";
                order.OrderDate = DateTime.Now;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Decrement stock via Product-Service
                await DecrementStockAsync(order.ProductId, order.Quantity);

                // Notify customer
                await SendNotificationAsync(order.CustomerId, "ORDER_PLACED_ONLINE");

                // Clear cart
                await _cartClient.ClearCartAsync(order.CustomerId);


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

        private async Task SendNotificationAsync(int customerId, string eventType)
        {
            try
            {
                await _notifyClient.SendNotificationAsync(customerId, eventType, "Order Update", $"Your order status: {eventType}");
            }
            catch
            {
                // Notification failure should not block order placement
            }
        }

        private async Task DeductWalletAsync(int customerId, decimal amount, int orderId = 0)
        {
            var success = await _walletClient.PayMoneyAsync(customerId, amount, orderId);
            if (!success) throw new Exception("Wallet deduction failed or insufficient balance");
        }

    }
}
