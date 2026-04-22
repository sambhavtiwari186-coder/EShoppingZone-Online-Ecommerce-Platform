using EShoppingZone.Cart.API.Domain;

namespace EShoppingZone.Cart.API.Services
{
    public interface ICartService
    {
        // Cart operations
        Task<IEnumerable<Domain.Cart>> GetAllCartsAsync();
        Task<Domain.Cart> GetCartAsync(int userId);
        Task<Domain.Cart> CreateCartForUserAsync(int userId);
        Task<Domain.Cart> UpdateCartAsync(int userId, List<CartItem> items);
        
        // Wishlist operations
        Task<IEnumerable<Wishlist>> GetWishlistAsync(int userId);
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlist);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<Domain.Cart> MoveToCartAsync(int wishlistId);
        
        // Pattern from screenshot
        decimal CartTotal(Domain.Cart cart);
    }
}
