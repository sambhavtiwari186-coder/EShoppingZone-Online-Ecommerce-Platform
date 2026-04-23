using EShoppingZone.Review.API.Domain;

namespace EShoppingZone.Review.API.Services
{
    public interface IReviewService
    {
        Task<Domain.Review> SubmitReviewAsync(ReviewDto dto);
        Task<IEnumerable<Domain.Review>> GetReviewsByProductAsync(int productId);
        Task<double> GetAverageRatingAsync(int productId);
        Task<bool> DeleteReviewAsync(int reviewId, int customerId);
        Task<bool> VoteReviewHelpfulAsync(int reviewId, int customerId, bool isHelpful);
    }
}
