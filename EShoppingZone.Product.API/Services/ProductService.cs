using EShoppingZone.Product.API.Domain;
using EShoppingZone.Product.API.Repositories;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using EShoppingZone.Product.API.HttpClients;


namespace EShoppingZone.Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly INotifyClient _notifyClient;

        public ProductService(IProductRepository repository, INotifyClient notifyClient)
        {
            _repository = repository;
            _notifyClient = notifyClient;
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
            try
            {
                await _notifyClient.SendNotificationAsync(0, "LOW_STOCK", "Low Stock Alert", $"Product {product.ProductName} is low in stock ({product.StockQuantity} left)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to notify Low Stock: {ex.Message}");
            }
        }

    }
}
