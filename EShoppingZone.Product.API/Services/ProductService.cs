using EShoppingZone.Product.API.Domain;
using EShoppingZone.Product.API.Repositories;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace EShoppingZone.Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductService(IProductRepository repository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Domain.Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllProductsAsync();
        }

        public async Task<Domain.Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetProductByIdAsync(id);
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByNameAsync(string name)
        {
            return await _repository.GetProductsByNameAsync(name);
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByCategoryAsync(string category)
        {
            return await _repository.GetProductsByCategoryAsync(category);
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByTypeAsync(string type)
        {
            return await _repository.GetProductsByTypeAsync(type);
        }

        public async Task<Domain.Product> AddProductAsync(Domain.Product product)
        {
            return await _repository.AddProductAsync(product);
        }

        public async Task<Domain.Product?> UpdateProductAsync(Domain.Product product)
        {
            return await _repository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteProductAsync(id);
        }

        public async Task<bool> DecrementStockAsync(int productId, int qty)
        {
            var rowsAffected = await _repository.DecrementStockAsync(productId, qty);
            if (rowsAffected > 0)
            {
                var product = await _repository.GetProductByIdAsync(productId);
                if (product != null && product.StockQuantity < 5)
                {
                    await NotifyLowStock(product);
                }
                return true;
            }
            return false;
        }

        private async Task NotifyLowStock(Domain.Product product)
        {
            var client = _httpClientFactory.CreateClient();
            var notifyServiceUrl = _configuration["Services:NotifyServiceUrl"] ?? "http://localhost:5005/api/notify";
            
            var notification = new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CurrentStock = product.StockQuantity,
                Message = $"Low stock alert for {product.ProductName}. Current quantity: {product.StockQuantity}"
            };

            var content = new StringContent(JsonSerializer.Serialize(notification), Encoding.UTF8, "application/json");
            try 
            {
                await client.PostAsync(notifyServiceUrl, content);
            }
            catch (Exception ex)
            {
                // Log error or handle failure to notify
                Console.WriteLine($"Failed to notify Low Stock: {ex.Message}");
            }
        }
    }
}
