using EShoppingZone.Cart.API.Domain;
using EShoppingZone.Cart.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Cart.API.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    [Authorize(Roles = "CUSTOMER,ADMIN")]
    public class WishlistController : ControllerBase
    {
        private readonly ICartService _service;

        public WishlistController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlist(int uid)
        {
            var wishlist = await _service.GetWishlistAsync(uid);
            return Ok(wishlist);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Wishlist>> AddToWishlist(Wishlist wishlist)
        {
            var created = await _service.AddToWishlistAsync(wishlist);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var result = await _service.RemoveFromWishlistAsync(id);
            if (!result) return NotFound();
            return Ok(new { Message = "Removed from wishlist" });
        }

        [HttpPost("moveToCart/{id}")]
        public async Task<ActionResult<Domain.Cart>> MoveToCart(int id)
        {
            try
            {
                var cart = await _service.MoveToCartAsync(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
