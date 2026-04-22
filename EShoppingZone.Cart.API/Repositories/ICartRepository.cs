using EShoppingZone.Cart.API.Domain;

namespace EShoppingZone.Cart.API.Repositories
{
    public interface ICartRepository
    {
        // Cart operations
        Task<IEnumerable<Domain.Cart>> GetAllCartsAsync();
        Task<Domain.Cart?> GetCartByUserIdAsync(int userId);
        Task<Domain.Cart> CreateCartAsync(Domain.Cart cart);
        Task<Domain.Cart> UpdateCartAsync(Domain.Cart cart);
        
        // Wishlist operations
        Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(int userId);
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlist);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<Wishlist?> GetWishlistItemAsync(int wishlistId);
    }
}
