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
            var addedProduct = await _repository.AddProductAsync(product);
            
            // Record initial stock movement
            await _repository.AddStockMovementAsync(new StockMovement
            {
                ProductId = addedProduct.ProductId,
                ProductName = addedProduct.ProductName,
                MerchantId = addedProduct.MerchantId,
                Change = addedProduct.StockQuantity,
                Timestamp = DateTime.Now,
                Reason = "New Product Addition",
                Status = "Added"
            });

            return addedProduct;
        }

        public async Task<Domain.Product?> UpdateProductAsync(Domain.Product product)
        {
            var oldProduct = await _repository.GetProductByIdAsync(product.ProductId);
            if (oldProduct == null) return null;

            int stockDifference = product.StockQuantity - oldProduct.StockQuantity;
            
            var updatedProduct = await _repository.UpdateProductAsync(product);
            
            if (updatedProduct != null && stockDifference != 0)
            {
                await _repository.AddStockMovementAsync(new StockMovement
                {
                    ProductId = updatedProduct.ProductId,
                    ProductName = updatedProduct.ProductName,
                    MerchantId = updatedProduct.MerchantId,
                    Change = stockDifference,
                    Timestamp = DateTime.Now,
                    Reason = stockDifference > 0 ? "Inventory Restock" : "Stock Adjustment",
                    Status = stockDifference > 0 ? "Added" : "Updated"
                });
            }

            return updatedProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteProductAsync(id);
        }

        public async Task<bool> DecrementStockAsync(int productId, int qty)
        {
            var product = await _repository.GetProductByIdAsync(productId);
            if (product == null) return false;

            var rowsAffected = await _repository.DecrementStockAsync(productId, qty);
            if (rowsAffected > 0)
            {
                // Record stock movement
                await _repository.AddStockMovementAsync(new StockMovement
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    MerchantId = product.MerchantId,
                    Change = -qty,
                    Timestamp = DateTime.Now,
                    Reason = "Order Sale",
                    Status = "Sold"
                });

                if (product.StockQuantity - qty < 5)
                {
                    await NotifyLowStock(product);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> IncrementStockAsync(int productId, int qty)
        {
            var product = await _repository.GetProductByIdAsync(productId);
            if (product == null) return false;

            var rowsAffected = await _repository.IncrementStockAsync(productId, qty);
            if (rowsAffected > 0)
            {
                // Record stock movement
                await _repository.AddStockMovementAsync(new StockMovement
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    MerchantId = product.MerchantId,
                    Change = qty,
                    Timestamp = DateTime.Now,
                    Reason = "Inventory Restock",
                    Status = "Added"
                });
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllCategoriesAsync();
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByMerchantIdAsync(int merchantId)
        {
            return await _repository.GetProductsByMerchantIdAsync(merchantId);
        }

        public async Task<IEnumerable<StockMovement>> GetStockMovementsByMerchantAsync(int merchantId)
        {
            return await _repository.GetStockMovementsByMerchantIdAsync(merchantId);
        }

        private async Task NotifyLowStock(Domain.Product product)
        {
            try
            {
                await _notifyClient.SendNotificationAsync(product.MerchantId, "LOW_STOCK", "Low Stock Alert", $"Product {product.ProductName} is low in stock ({product.StockQuantity} left)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to notify Low Stock: {ex.Message}");
            }
        }

    }
}
