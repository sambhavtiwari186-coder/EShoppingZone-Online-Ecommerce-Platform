using EShoppingZone.Product.API.Domain;

namespace EShoppingZone.Product.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Domain.Product>> GetAllProductsAsync();
        Task<Domain.Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Domain.Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Domain.Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Domain.Product>> GetProductsByTypeAsync(string type);
        Task<Domain.Product> AddProductAsync(Domain.Product product);
        Task<Domain.Product?> UpdateProductAsync(Domain.Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> DecrementStockAsync(int productId, int qty);
        Task<bool> IncrementStockAsync(int productId, int qty);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<IEnumerable<Domain.Product>> GetProductsByMerchantIdAsync(int merchantId);
        Task<IEnumerable<StockMovement>> GetStockMovementsByMerchantAsync(int merchantId);
    }
}
