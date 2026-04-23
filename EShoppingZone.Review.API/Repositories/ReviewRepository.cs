using EShoppingZone.Review.API.Data;
using EShoppingZone.Review.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Review.API.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewDbContext _context;

        public ReviewRepository(ReviewDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Review> AddReviewAsync(Domain.Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Domain.Review>> GetReviewsByProductAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var hasReviews = await _context.Reviews.AnyAsync(r => r.ProductId == productId);
            if (!hasReviews) return 0.0;

            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (double)r.Rating);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, int customerId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.CustomerId == customerId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasCustomerReviewedAsync(int productId, int customerId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<bool> AddReviewVoteAsync(ReviewVote vote)
        {
            _context.ReviewVotes.Add(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasCustomerVotedAsync(int reviewId, int customerId)
        {
            return await _context.ReviewVotes
                .AnyAsync(v => v.ReviewId == reviewId && v.CustomerId == customerId);
        }

        public async Task<Domain.Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task UpdateReviewAsync(Domain.Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
