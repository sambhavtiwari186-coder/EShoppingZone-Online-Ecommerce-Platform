using EShoppingZone.Orders.API.Domain;
using EShoppingZone.Orders.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Orders.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        // GET /api/orders/all — ADMIN/MERCHANT
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN,MERCHANT")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET /api/orders/byCustomer/{id} — CUSTOMER
        [HttpGet("byCustomer/{id}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(int id)
        {
            var orders = await _service.GetOrdersByCustomerAsync(id);
            return Ok(orders);
        }

        // GET /api/orders/allAddress/{id} — CUSTOMER
        [HttpGet("allAddress/{id}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<IEnumerable<OrderAddress>>> GetAllAddresses(int id)
        {
            var addresses = await _service.GetAddressesByCustomerAsync(id);
            return Ok(addresses);
        }

        // POST /api/orders/placeOrder — COD — CUSTOMER
        [HttpPost("placeOrder")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<Order>> PlaceOrder([FromBody] Order order)
        {
            try
            {
                var placed = await _service.PlaceOrderAsync(order);
                return Ok(placed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST /api/orders/onlinePayment — wallet payment — CUSTOMER
        [HttpPost("onlinePayment")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<Order>> OnlinePayment([FromBody] Order order)
        {
            try
            {
                var placed = await _service.PlaceOnlinePaymentOrderAsync(order);
                return Ok(placed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST /api/orders/storeAddress — CUSTOMER
        [HttpPost("storeAddress")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<OrderAddress>> StoreAddress(
            [FromQuery] int orderId,
            [FromBody] OrderAddress address)
        {
            try
            {
                var saved = await _service.StoreAddressAsync(orderId, address);
                return Ok(saved);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT /api/orders/changeStatus — ADMIN/AGENT
        [HttpPut("changeStatus")]
        [Authorize(Roles = "ADMIN,AGENT")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string status, [FromQuery] int orderId)
        {
            var result = await _service.ChangeStatusAsync(status, orderId);
            if (!result) return NotFound(new { Message = "Order not found" });
            return Ok(new { Message = $"Order status changed to {status}" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _service.CancelOrderAsync(id);
            if (!result) return NotFound(new { Message = "Order not found" });
            return Ok(new { Message = "Order cancelled successfully" });
        }

        // GET /api/orders/verifyPurchase/{custId}/{prodId}
        [HttpGet("verifyPurchase/{custId}/{prodId}")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<ActionResult<bool>> VerifyPurchase(int custId, int prodId)
        {
            var result = await _service.VerifyPurchaseAsync(custId, prodId);
            return Ok(result);
        }

    }
}
