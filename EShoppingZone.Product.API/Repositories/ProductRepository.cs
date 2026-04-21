using EShoppingZone.Product.API.Data;
using EShoppingZone.Product.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Product.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Domain.Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products.Where(p => p.ProductName.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Product>> GetProductsByTypeAsync(string type)
        {
            return await _context.Products.Where(p => p.ProductType == type).ToListAsync();
        }

        public async Task<Domain.Product> AddProductAsync(Domain.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Domain.Product?> UpdateProductAsync(Domain.Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null) return null;

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DecrementStockAsync(int productId, int qty)
        {
            return await _context.Products
                .Where(p => p.ProductId == productId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p => p.StockQuantity - qty));
        }
    }
}
