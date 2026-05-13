using EShoppingZone.Review.API.Controllers;
using EShoppingZone.Review.API.Domain;
using EShoppingZone.Review.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EShoppingZone.Review.Tests
{
    [TestFixture]
    public class ReviewsControllerTests
    {
        private Mock<IReviewService> _mockService = null!;
        private ReviewsController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IReviewService>();
            _controller = new ReviewsController(_mockService.Object);
        }

        [Test]
        public async Task SubmitReview_ValidDto_ReturnsOk()
        {
            // Arrange
            var dto = new ReviewDto { ProductId = 1, CustomerId = 1, Rating = 5, Title = "Great", Body = "Good product" };
            var expectedReview = new API.Domain.Review { ReviewId = 1, ProductId = 1, CustomerId = 1, Rating = 5, Title = "Great", Body = "Good product" };
            _mockService.Setup(s => s.SubmitReviewAsync(dto)).ReturnsAsync(expectedReview);

            // Act
            var result = await _controller.SubmitReview(dto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result!;
            Assert.That(okResult.Value, Is.InstanceOf<API.Domain.Review>());
            var returnValue = (API.Domain.Review)okResult.Value!;
            Assert.That(returnValue.ReviewId, Is.EqualTo(expectedReview.ReviewId));
        }

        [Test]
        public async Task GetReviewsByProduct_ReturnsOkWithReviews()
        {
            // Arrange
            int productId = 1;
            var expectedReviews = new List<API.Domain.Review> { new API.Domain.Review { ReviewId = 1, ProductId = productId } };
            _mockService.Setup(s => s.GetReviewsByProductAsync(productId)).ReturnsAsync(expectedReviews);

            // Act
            var result = await _controller.GetReviewsByProduct(productId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result!;
            Assert.That(okResult.Value, Is.InstanceOf<IEnumerable<API.Domain.Review>>());
            var returnValue = (IEnumerable<API.Domain.Review>)okResult.Value!;
            Assert.That(returnValue.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAverageRating_ReturnsOkWithRating()
        {
            // Arrange
            int productId = 1;
            double expectedRating = 4.5;
            _mockService.Setup(s => s.GetAverageRatingAsync(productId)).ReturnsAsync(expectedRating);

            // Act
            var result = await _controller.GetAverageRating(productId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result!;
            Assert.That(okResult.Value, Is.EqualTo(expectedRating));
        }

        [Test]
        public async Task DeleteReview_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int reviewId = 1;
            int customerId = 1;
            _mockService.Setup(s => s.DeleteReviewAsync(reviewId, customerId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteReview(reviewId, customerId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.Not.Null);
        }

        [Test]
        public async Task DeleteReview_ReturnsNotFound_WhenUnsuccessful()
        {
            // Arrange
            int reviewId = 1;
            int customerId = 1;
            _mockService.Setup(s => s.DeleteReviewAsync(reviewId, customerId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteReview(reviewId, customerId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.Not.Null);
        }
    }
}
