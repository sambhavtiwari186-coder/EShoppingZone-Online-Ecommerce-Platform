using EShoppingZone.Notify.API.Domain;
using EShoppingZone.Notify.API.Repositories;
using EShoppingZone.Notify.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace EShoppingZone.Notify.Tests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<INotificationRepository> _mockRepo = null!;
        private Mock<ILogger<NotificationService>> _mockLogger = null!;
        private NotificationService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<INotificationRepository>();
            _mockLogger = new Mock<ILogger<NotificationService>>();
            _service = new NotificationService(_mockRepo.Object, _mockLogger.Object);
        }

        [Test]
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Type, Is.EqualTo(type));
            Assert.That(result.Title, Is.EqualTo("Order Placed"));
            Assert.That(result.Message, Is.EqualTo("Your order has been placed."));
            Assert.That(result.IsRead, Is.False);

            _mockRepo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Test]
        public async Task GetUnreadCountAsync_ShouldReturnUnreadCount()
        {
            // Arrange
            var userId = 1;
            _mockRepo.Setup(r => r.GetUnreadCountAsync(userId)).ReturnsAsync(3);

            // Act
            var result = await _service.GetUnreadCountAsync(userId);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public async Task MarkAsReadAsync_ShouldUpdateIsRead()
        {
            // Arrange
            var id = 5;
            var notification = new Notification { NotificationId = id, IsRead = false };
            _mockRepo.Setup(r => r.GetNotificationByIdAsync(id)).ReturnsAsync(notification);
            
            // Act
            var result = await _service.MarkAsReadAsync(id);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(notification.IsRead, Is.True);
            _mockRepo.Verify(r => r.UpdateNotificationAsync(notification), Times.Once);
        }
    }
}
