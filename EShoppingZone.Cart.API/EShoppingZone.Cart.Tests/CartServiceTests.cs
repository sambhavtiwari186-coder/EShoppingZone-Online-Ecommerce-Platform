using EShoppingZone.Cart.API.Domain;
using EShoppingZone.Cart.API.Repositories;
using EShoppingZone.Cart.API.Services;
using Moq;
using NUnit.Framework;

namespace EShoppingZone.Cart.Tests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<ICartRepository> _mockRepo = null!;
        private CartService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ICartRepository>();
            _service = new CartService(_mockRepo.Object);
        }

        [Test]
        public async Task GetCartAsync_ShouldCreateCartIfNoneExists()
        {
            // Arrange
            int userId = 1;
            _mockRepo.Setup(r => r.GetCartByUserIdAsync(userId)).ReturnsAsync((API.Domain.Cart)null!);
            _mockRepo.Setup(r => r.CreateCartAsync(It.IsAny<API.Domain.Cart>()))
                .ReturnsAsync((API.Domain.Cart c) => c);

            // Act
            var result = await _service.GetCartAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CartId, Is.EqualTo(userId));
            _mockRepo.Verify(r => r.CreateCartAsync(It.IsAny<API.Domain.Cart>()), Times.Once);
        }

        [Test]
        public void CartTotal_ShouldCalculateSumCorrectly()
        {
            // Arrange
            var cart = new API.Domain.Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { Price = 100, Quantity = 2 },
                    new CartItem { Price = 50, Quantity = 1 }
                }
            };

            // Act
            var total = _service.CartTotal(cart);

            // Assert
            Assert.That(total, Is.EqualTo(250));
        }

        [Test]
        public async Task MoveToCartAsync_ValidItem_ShouldAddAndRemoveFromWishlist()
        {
            // Arrange
            int wishlistId = 10;
            var wishlist = new Wishlist { UserId = 1, ProductId = 100, Price = 500, ProductName = "Test Product" };
            var cart = new API.Domain.Cart { CartId = 1, Items = new List<CartItem>() };

            _mockRepo.Setup(r => r.GetWishlistItemAsync(wishlistId)).ReturnsAsync(wishlist);
            _mockRepo.Setup(r => r.GetCartByUserIdAsync(1)).ReturnsAsync(cart);

            // Act
            var result = await _service.MoveToCartAsync(wishlistId);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.Items[0].ProductId, Is.EqualTo(100));
            Assert.That(result.TotalPrice, Is.EqualTo(500));
            _mockRepo.Verify(r => r.RemoveFromWishlistAsync(wishlistId), Times.Once);
        }
    }
}
