using EShoppingZone.Review.API.Domain;
using EShoppingZone.Review.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Review.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        // POST /api/reviews - CUSTOMER
        [HttpPost]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<Domain.Review>> SubmitReview([FromBody] ReviewDto dto)
        {
            try
            {
                var review = await _service.SubmitReviewAsync(dto);
                return Ok(review);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/reviews/product/{productId} - None (Public)
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Domain.Review>>> GetReviewsByProduct(int productId)
        {
            var reviews = await _service.GetReviewsByProductAsync(productId);
            return Ok(reviews);
        }

        // GET /api/reviews/product/{id}/rating - None (Public)
        [HttpGet("product/{id}/rating")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetAverageRating(int id)
        {
            var rating = await _service.GetAverageRatingAsync(id);
            return Ok(rating);
        }

        // PUT /api/reviews/{id}/helpful - CUSTOMER
        [HttpPut("{id}/helpful")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> VoteReviewHelpful(int id, [FromQuery] int customerId, [FromQuery] bool isHelpful = true)
        {
            try
            {
                var result = await _service.VoteReviewHelpfulAsync(id, customerId, isHelpful);
                if (!result) return NotFound(new { Message = "Review not found." });
                return Ok(new { Message = "Vote recorded." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE /api/reviews/{id} - CUSTOMER
        [HttpDelete("{id}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> DeleteReview(int id, [FromQuery] int customerId)
        {
            // Note: Ideally customerId would come from JWT claims, but keeping simple
            var result = await _service.DeleteReviewAsync(id, customerId);
            if (!result) return NotFound(new { Message = "Review not found or not owned by you." });
            return Ok(new { Message = "Review deleted." });
        }
    }
}
