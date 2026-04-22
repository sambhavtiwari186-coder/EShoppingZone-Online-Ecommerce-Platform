using EShoppingZone.Cart.API.Data;
using EShoppingZone.Cart.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Cart.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Cart>> GetAllCartsAsync()
        {
            return await _context.Carts.Include(c => c.Items).ToListAsync();
        }

        public async Task<Domain.Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == userId);
        }

        public async Task<Domain.Cart> CreateCartAsync(Domain.Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Domain.Cart> UpdateCartAsync(Domain.Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(int userId)
        {
            return await _context.Wishlists.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<Wishlist> AddToWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            var item = await _context.Wishlists.FindAsync(wishlistId);
            if (item == null) return false;

            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Wishlist?> GetWishlistItemAsync(int wishlistId)
        {
            return await _context.Wishlists.FindAsync(wishlistId);
        }
    }
}
