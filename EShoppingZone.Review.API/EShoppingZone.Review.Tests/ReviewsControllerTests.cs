using EShoppingZone.Review.API.Controllers;
using EShoppingZone.Review.API.Domain;
using EShoppingZone.Review.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EShoppingZone.Review.Tests
{
    public class ReviewsControllerTests
    {
        private readonly Mock<IReviewService> _mockService;
        private readonly ReviewsController _controller;

        public ReviewsControllerTests()
        {
            _mockService = new Mock<IReviewService>();
            _controller = new ReviewsController(_mockService.Object);
        }

        [Fact]
        public async Task SubmitReview_ValidDto_ReturnsOk()
        {
            // Arrange
            var dto = new ReviewDto { ProductId = 1, CustomerId = 1, Rating = 5, Title = "Great", Body = "Good product" };
            var expectedReview = new API.Domain.Review { ReviewId = 1, ProductId = 1, CustomerId = 1, Rating = 5, Title = "Great", Body = "Good product" };
            _mockService.Setup(s => s.SubmitReviewAsync(dto)).ReturnsAsync(expectedReview);

            // Act
            var result = await _controller.SubmitReview(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<API.Domain.Review>(okResult.Value);
            Assert.Equal(expectedReview.ReviewId, returnValue.ReviewId);
        }

        [Fact]
        public async Task GetReviewsByProduct_ReturnsOkWithReviews()
        {
            // Arrange
            int productId = 1;
            var expectedReviews = new List<API.Domain.Review> { new API.Domain.Review { ReviewId = 1, ProductId = productId } };
            _mockService.Setup(s => s.GetReviewsByProductAsync(productId)).ReturnsAsync(expectedReviews);

            // Act
            var result = await _controller.GetReviewsByProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<API.Domain.Review>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAverageRating_ReturnsOkWithRating()
        {
            // Arrange
            int productId = 1;
            double expectedRating = 4.5;
            _mockService.Setup(s => s.GetAverageRatingAsync(productId)).ReturnsAsync(expectedRating);

            // Act
            var result = await _controller.GetAverageRating(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<double>(okResult.Value);
            Assert.Equal(expectedRating, returnValue);
        }

        [Fact]
        public async Task DeleteReview_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int reviewId = 1;
            int customerId = 1;
            _mockService.Setup(s => s.DeleteReviewAsync(reviewId, customerId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteReview(reviewId, customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task DeleteReview_ReturnsNotFound_WhenUnsuccessful()
        {
            // Arrange
            int reviewId = 1;
            int customerId = 1;
            _mockService.Setup(s => s.DeleteReviewAsync(reviewId, customerId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteReview(reviewId, customerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
