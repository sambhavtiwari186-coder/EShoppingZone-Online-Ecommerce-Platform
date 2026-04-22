using EShoppingZone.Cart.API.Domain;
using EShoppingZone.Cart.API.Repositories;
using EShoppingZone.Cart.API.Services;
using Moq;
using FluentAssertions;

// Alias to avoid Cart namespace vs Cart class ambiguity
using CartModel = EShoppingZone.Cart.API.Domain.Cart;

namespace EShoppingZone.Cart.Tests
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _mockRepo;
        private readonly CartService _service;

        public CartServiceTests()
        {
            _mockRepo = new Mock<ICartRepository>();
            _service = new CartService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            int userId = 123;
            _mockRepo.Setup(r => r.GetCartByUserIdAsync(userId)).ReturnsAsync((CartModel?)null);
            _mockRepo.Setup(r => r.CreateCartAsync(It.IsAny<CartModel>()))
                     .ReturnsAsync(new CartModel { CartId = userId, TotalPrice = 0 });

            // Act
            var result = await _service.GetCartAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.CartId.Should().Be(userId);
            _mockRepo.Verify(r => r.CreateCartAsync(It.IsAny<CartModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldRecalculateTotalPrice()
        {
            // Arrange
            int userId = 123;
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Price = 50, Quantity = 2, CartId = userId },
                new CartItem { ProductId = 2, Price = 30, Quantity = 1, CartId = userId }
            };
            var existingCart = new CartModel { CartId = userId, TotalPrice = 0, Items = new List<CartItem>() };

            _mockRepo.Setup(r => r.GetCartByUserIdAsync(userId)).ReturnsAsync(existingCart);
            _mockRepo.Setup(r => r.UpdateCartAsync(It.IsAny<CartModel>()))
                     .ReturnsAsync((CartModel c) => c);

            // Act
            var result = await _service.UpdateCartAsync(userId, items);

            // Assert
            result.TotalPrice.Should().Be(130m); // 50*2 + 30*1
        }

        [Fact]
        public void CartTotal_ShouldSumItemPrices()
        {
            // Arrange
            var cart = new CartModel
            {
                CartId = 1,
                Items = new List<CartItem>
                {
                    new CartItem { Price = 10, Quantity = 3 },
                    new CartItem { Price = 25, Quantity = 2 }
                }
            };

            // Act
            var total = _service.CartTotal(cart);

            // Assert
            total.Should().Be(80m); // 10*3 + 25*2
        }
    }
}
