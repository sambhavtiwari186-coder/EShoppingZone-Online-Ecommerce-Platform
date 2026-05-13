using EShoppingZone.Product.API.Domain;

namespace EShoppingZone.Product.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Domain.Product>> GetAllProductsAsync();
        Task<Domain.Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Domain.Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Domain.Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Domain.Product>> GetProductsByTypeAsync(string type);
        Task<Domain.Product> AddProductAsync(Domain.Product product);
        Task<Domain.Product?> UpdateProductAsync(Domain.Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<int> DecrementStockAsync(int productId, int qty);
        Task<int> IncrementStockAsync(int productId, int qty);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<IEnumerable<Domain.Product>> GetProductsByMerchantIdAsync(int merchantId);
        
        // Stock Movement methods
        Task AddStockMovementAsync(StockMovement movement);
        Task<IEnumerable<StockMovement>> GetStockMovementsByMerchantIdAsync(int merchantId);
    }
}
