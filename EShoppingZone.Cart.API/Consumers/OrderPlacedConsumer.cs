using EShoppingZone.Cart.API.Events;
using EShoppingZone.Cart.API.Services;
using MassTransit;

namespace EShoppingZone.Cart.API.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(ICartService cartService, ILogger<OrderPlacedConsumer> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("RabbitMQ: OrderPlacedEvent received for Customer {CustomerId}. Clearing cart...", message.CustomerId);

            try
            {
                await _cartService.UpdateCartAsync(message.CustomerId, new List<Domain.CartItem>());
                _logger.LogInformation("RabbitMQ: Cart cleared for Customer {CustomerId}.", message.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ: Failed to clear cart for Customer {CustomerId}.", message.CustomerId);
            }
        }
    }
}
