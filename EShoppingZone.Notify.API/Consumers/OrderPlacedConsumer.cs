using EShoppingZone.Notify.API.Events;
using EShoppingZone.Notify.API.Repositories;
using MassTransit;

namespace EShoppingZone.Notify.API.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly INotificationRepository _repository;
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(INotificationRepository repository, ILogger<OrderPlacedConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Processing OrderPlacedEvent for OrderId: {OrderId}", message.OrderId);

            // Notify Customer
            await _repository.CreateNotificationAsync(new Domain.Notification
            {
                UserId = message.CustomerId,
                Message = $"Your order #{message.OrderId} for {message.ProductName} was placed successfully!",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = "ORDER_PLACED"
            });

            // Notify Merchant
            await _repository.CreateNotificationAsync(new Domain.Notification
            {
                UserId = message.MerchantId,
                Message = $"New order received: #{message.OrderId} for {message.ProductName} (Amount: ₹{message.Amount})",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = "NEW_ORDER"
            });

            _logger.LogInformation("Notifications created for OrderId: {OrderId}", message.OrderId);
        }
    }
}
