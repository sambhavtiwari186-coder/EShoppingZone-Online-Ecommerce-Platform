using EShoppingZone.Cart.API.Domain;
using EShoppingZone.Cart.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Cart.API.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Domain.Cart>>> GetAllCarts()
        {
            var carts = await _service.GetAllCartsAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<Domain.Cart>> GetCart(int id)
        {
            var cart = await _service.GetCartAsync(id);
            return Ok(cart);
        }

        [HttpPost("add/{uid}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<Domain.Cart>> CreateCart(int uid)
        {
            var cart = await _service.CreateCartForUserAsync(uid);
            return Ok(cart);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Domain.Cart>> UpdateCart([FromQuery] int userId, [FromBody] List<CartItem> items)
        {
            var cart = await _service.UpdateCartAsync(userId, items);
            return Ok(cart);
        }
    }
}
