using EShoppingZone.Notify.API.Domain;
using EShoppingZone.Notify.API.Repositories;
using EShoppingZone.Notify.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EShoppingZone.Notify.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationRepository> _mockRepo;
        private readonly Mock<ILogger<NotificationService>> _mockLogger;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            _mockRepo = new Mock<INotificationRepository>();
            _mockLogger = new Mock<ILogger<NotificationService>>();
            _service = new NotificationService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateNotificationAsync_ShouldSaveNotification()
        {
            // Arrange
            var userId = 1;
            var type = "ORDER_PLACED";
            var title = "Order Placed";
            var message = "Your order has been placed.";

            Notification savedNotification = null!;
            _mockRepo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .Callback<Notification>(n => savedNotification = n)
                .ReturnsAsync((Notification n) => n);

            // Act
            var result = await _service.CreateNotificationAsync(userId, type, title, message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(type, result.Type);
            Assert.Equal("Order Placed", result.Title);
            Assert.Equal("Your order has been placed.", result.Message);
            Assert.False(result.IsRead);

            _mockRepo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task GetUnreadCountAsync_ShouldReturnUnreadCount()
        {
            // Arrange
            var userId = 1;
            _mockRepo.Setup(r => r.GetUnreadCountAsync(userId)).ReturnsAsync(3);

            // Act
            var result = await _service.GetUnreadCountAsync(userId);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task MarkAsReadAsync_ShouldUpdateIsRead()
        {
            // Arrange
            var id = 5;
            var notification = new Notification { NotificationId = id, IsRead = false };
            _mockRepo.Setup(r => r.GetNotificationByIdAsync(id)).ReturnsAsync(notification);
            
            // Act
            var result = await _service.MarkAsReadAsync(id);

            // Assert
            Assert.True(result);
            Assert.True(notification.IsRead);
            _mockRepo.Verify(r => r.UpdateNotificationAsync(notification), Times.Once);
        }
    }
}
