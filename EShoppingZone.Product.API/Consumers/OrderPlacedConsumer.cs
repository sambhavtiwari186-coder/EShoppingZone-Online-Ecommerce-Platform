using EShoppingZone.Product.API.Events;
using EShoppingZone.Product.API.Services;
using MassTransit;

namespace EShoppingZone.Product.API.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IProductService _productService;
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(IProductService productService, ILogger<OrderPlacedConsumer> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("RabbitMQ: OrderPlacedEvent received. ProductId: {ProductId}, Qty: {Quantity}", message.ProductId, message.Quantity);

            try
            {
                // Note: The OrderService already calls this via HTTP during initial placement.
                // This consumer can be used for secondary processing or if we fully move to Async.
                // For now, let's just log it or implement a 'SafeDecrement' if not already done.
                _logger.LogInformation("RabbitMQ: Stock adjustment acknowledged for Order {OrderId}", message.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ: Error processing stock for Order {OrderId}", message.OrderId);
            }
        }
    }
}
