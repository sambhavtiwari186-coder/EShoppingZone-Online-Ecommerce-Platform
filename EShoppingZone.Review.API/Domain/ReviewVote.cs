using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Review.API.Domain
{
    public class ReviewVote
    {
        [Key]
        public int VoteId { get; set; }
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public bool IsHelpful { get; set; }
    }
}
