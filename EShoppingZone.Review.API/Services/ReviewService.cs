using EShoppingZone.Review.API.Domain;
using EShoppingZone.Review.API.Repositories;
using System.Text.Json;

namespace EShoppingZone.Review.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ReviewService(IReviewRepository repository, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<Domain.Review> SubmitReviewAsync(ReviewDto dto)
        {
            // Verify if already reviewed
            if (await _repository.HasCustomerReviewedAsync(dto.ProductId, dto.CustomerId))
            {
                throw new InvalidOperationException("Customer has already reviewed this product.");
            }

            // Call Order-Service to verify purchase
            var hasPurchased = await VerifyPurchaseAsync(dto.CustomerId, dto.ProductId);
            if (!hasPurchased)
            {
                throw new UnauthorizedAccessException("You must purchase before reviewing");
            }

            var review = new Domain.Review
            {
                ProductId = dto.ProductId,
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                Rating = dto.Rating,
                Title = dto.Title,
                Body = dto.Body,
                CreatedAt = DateTime.UtcNow,
                HelpfulVotes = 0
            };

            return await _repository.AddReviewAsync(review);
        }

        public async Task<IEnumerable<Domain.Review>> GetReviewsByProductAsync(int productId)
        {
            return await _repository.GetReviewsByProductAsync(productId);
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            return await _repository.GetAverageRatingAsync(productId);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, int customerId)
        {
            return await _repository.DeleteReviewAsync(reviewId, customerId);
        }

        public async Task<bool> VoteReviewHelpfulAsync(int reviewId, int customerId, bool isHelpful)
        {
            if (await _repository.HasCustomerVotedAsync(reviewId, customerId))
            {
                throw new InvalidOperationException("You have already voted on this review.");
            }

            var review = await _repository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return false;
            }

            var vote = new ReviewVote
            {
                ReviewId = reviewId,
                CustomerId = customerId,
                IsHelpful = isHelpful
            };

            await _repository.AddReviewVoteAsync(vote);

            if (isHelpful)
            {
                review.HelpfulVotes++;
                await _repository.UpdateReviewAsync(review);
            }
            // If we supported negative votes, we could decrement here, but typically HelpfulVotes only counts "yes" votes.

            return true;
        }

        private async Task<bool> VerifyPurchaseAsync(int customerId, int productId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("OrderService");
                var orderBaseUrl = _config["Services:OrderAPI"] ?? "http://localhost:5400";
                
                // Get orders by customer
                // Endpoint mapping from previous EShoppingZone.Orders.API implementation: GET /api/orders/byCustomer/{id}
                var response = await client.GetAsync($"{orderBaseUrl}/api/orders/byCustomer/{customerId}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var orders = JsonSerializer.Deserialize<List<OrderMock>>(content, options);

                if (orders == null || !orders.Any())
                {
                    return false;
                }

                return orders.Any(o => o.ProductId == productId);
            }
            catch
            {
                // Unreachable or parsing failed, default to not purchased to be safe
                return false;
            }
        }

        // Helper class to deserialize minimal required properties from Orders API
        private class OrderMock
        {
            public int ProductId { get; set; }
        }
    }
}
