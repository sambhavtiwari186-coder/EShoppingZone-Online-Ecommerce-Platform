using EShoppingZone.Product.API.Domain;
using EShoppingZone.Product.API.Repositories;
using EShoppingZone.Product.API.Services;
using EShoppingZone.Product.API.HttpClients;
using Moq;
using NUnit.Framework;

namespace EShoppingZone.Product.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockRepo = null!;
        private Mock<INotifyClient> _mockNotifyClient = null!;
        private ProductService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockNotifyClient = new Mock<INotifyClient>();
            _service = new ProductService(_mockRepo.Object, _mockNotifyClient.Object);
        }

        [Test]
        public async Task DecrementStockAsync_ShouldRecordMovement_WhenSuccessful()
        {
            // Arrange
            int productId = 1;
            int qty = 2;
            var product = new EShoppingZone.Product.API.Domain.Product { ProductId = productId, ProductName = "Test", StockQuantity = 10, MerchantId = 101 };
            
            _mockRepo.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);
            _mockRepo.Setup(r => r.DecrementStockAsync(productId, qty)).ReturnsAsync(1);

            // Act
            var result = await _service.DecrementStockAsync(productId, qty);

            // Assert
            Assert.That(result, Is.True);
            _mockRepo.Verify(r => r.AddStockMovementAsync(It.Is<StockMovement>(m => 
                m.ProductId == productId && 
                m.Change == -qty && 
                m.Status == "Sold")), Times.Once);
        }

        [Test]
        public async Task DecrementStockAsync_ShouldNotify_WhenStockLow()
        {
            // Arrange
            int productId = 1;
            int qty = 8; // 10 - 8 = 2 (which is < 5)
            var product = new EShoppingZone.Product.API.Domain.Product { ProductId = productId, ProductName = "Test", StockQuantity = 10, MerchantId = 101 };
            
            _mockRepo.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);
            _mockRepo.Setup(r => r.DecrementStockAsync(productId, qty)).ReturnsAsync(1);

            // Act
            await _service.DecrementStockAsync(productId, qty);

            // Assert
            _mockNotifyClient.Verify(n => n.SendNotificationAsync(product.MerchantId, "LOW_STOCK", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task AddProductAsync_ShouldRecordInitialMovement()
        {
            // Arrange
            var product = new EShoppingZone.Product.API.Domain.Product { ProductId = 1, ProductName = "New", StockQuantity = 50, MerchantId = 101 };
            _mockRepo.Setup(r => r.AddProductAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _service.AddProductAsync(product);

            // Assert
            Assert.That(result, Is.EqualTo(product));
            _mockRepo.Verify(r => r.AddStockMovementAsync(It.Is<StockMovement>(m => 
                m.ProductId == 1 && 
                m.Change == 50 && 
                m.Status == "Added")), Times.Once);
        }
    }
}
