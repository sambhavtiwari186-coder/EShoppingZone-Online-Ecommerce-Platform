using EShoppingZone.Product.API.Domain;
using EShoppingZone.Product.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Product.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Product>>> GetAllProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Product>> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<IEnumerable<Domain.Product>>> GetProductsByName(string name)
        {
            var products = await _service.GetProductsByNameAsync(name);
            return Ok(products);
        }

        [HttpGet("byCategory/{cat}")]
        public async Task<ActionResult<IEnumerable<Domain.Product>>> GetProductsByCategory(string cat)
        {
            var products = await _service.GetProductsByCategoryAsync(cat);
            return Ok(products);
        }

        [HttpGet("byType/{type}")]
        public async Task<ActionResult<IEnumerable<Domain.Product>>> GetProductsByType(string type)
        {
            var products = await _service.GetProductsByTypeAsync(type);
            return Ok(products);
        }

        [HttpGet("byMerchant/{id}")]
        public async Task<ActionResult<IEnumerable<Domain.Product>>> GetProductsByMerchant(int id)
        {
            var products = await _service.GetProductsByMerchantIdAsync(id);
            return Ok(products);
        }

        [HttpGet("stockMovements/{merchantId}")]
        [Authorize(Roles = "MERCHANT")]
        public async Task<ActionResult<IEnumerable<StockMovement>>> GetStockMovementsByMerchant(int merchantId)
        {
            var movements = await _service.GetStockMovementsByMerchantAsync(merchantId);
            return Ok(movements);
        }

        [HttpPost("add")]
        [Authorize(Roles = "MERCHANT")]
        public async Task<ActionResult<Domain.Product>> AddProduct(Domain.Product product)
        {
            var createdProduct = await _service.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("update")]
        [Authorize(Roles = "MERCHANT")]
        public async Task<IActionResult> UpdateProduct(Domain.Product product)
        {
            var updatedProduct = await _service.UpdateProductAsync(product);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpPut("decrementStock/{id}/{qty}")]
        public async Task<IActionResult> DecrementStock(int id, int qty)
        {
            var result = await _service.DecrementStockAsync(id, qty);
            if (!result) return NotFound();
            return Ok(new { Message = "Stock decremented successfully" });
        }

        [HttpPut("incrementStock/{id}/{qty}")]
        public async Task<IActionResult> IncrementStock(int id, int qty)
        {
            var result = await _service.IncrementStockAsync(id, qty);
            if (!result) return NotFound();
            return Ok(new { Message = "Stock incremented successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MERCHANT,ADMIN")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _service.DeleteProductAsync(id);
            if (!result) return NotFound();
            return Ok(new { Message = "Product deleted successfully" });
        }
    }
}
