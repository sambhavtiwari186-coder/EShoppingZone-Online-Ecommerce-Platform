using EShoppingZone.Review.API.Domain;

namespace EShoppingZone.Review.API.Repositories
{
    public interface IReviewRepository
    {
        Task<Domain.Review> AddReviewAsync(Domain.Review review);
        Task<IEnumerable<Domain.Review>> GetReviewsByProductAsync(int productId);
        Task<double> GetAverageRatingAsync(int productId);
        Task<bool> DeleteReviewAsync(int reviewId, int customerId);
        Task<bool> HasCustomerReviewedAsync(int productId, int customerId);
        Task<bool> AddReviewVoteAsync(ReviewVote vote);
        Task<bool> HasCustomerVotedAsync(int reviewId, int customerId);
        Task<Domain.Review?> GetReviewByIdAsync(int reviewId);
        Task UpdateReviewAsync(Domain.Review review);
    }
}
