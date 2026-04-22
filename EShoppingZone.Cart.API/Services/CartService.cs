using EShoppingZone.Cart.API.Domain;
using EShoppingZone.Cart.API.Repositories;

namespace EShoppingZone.Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Domain.Cart>> GetAllCartsAsync()
        {
            return await _repository.GetAllCartsAsync();
        }

        public async Task<Domain.Cart> GetCartAsync(int userId)
        {
            var cart = await _repository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await CreateCartForUserAsync(userId);
            }
            return cart;
        }

        public async Task<Domain.Cart> CreateCartForUserAsync(int userId)
        {
            var cart = new Domain.Cart { CartId = userId, TotalPrice = 0 };
            return await _repository.CreateCartAsync(cart);
        }

        public async Task<Domain.Cart> UpdateCartAsync(int userId, List<CartItem> items)
        {
            var cart = await GetCartAsync(userId);
            cart.Items = items;
            cart.TotalPrice = CartTotal(cart);
            return await _repository.UpdateCartAsync(cart);
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistAsync(int userId)
        {
            return await _repository.GetWishlistByUserIdAsync(userId);
        }

        public async Task<Wishlist> AddToWishlistAsync(Wishlist wishlist)
        {
            return await _repository.AddToWishlistAsync(wishlist);
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            return await _repository.RemoveFromWishlistAsync(wishlistId);
        }

        public async Task<Domain.Cart> MoveToCartAsync(int wishlistId)
        {
            var wishlist = await _repository.GetWishlistItemAsync(wishlistId);
            if (wishlist == null) throw new Exception("Wishlist item not found");

            var cart = await GetCartAsync(wishlist.UserId);
            
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == wishlist.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = wishlist.ProductId,
                    ProductName = wishlist.ProductName,
                    Price = wishlist.Price,
                    Quantity = 1,
                    CartId = cart.CartId
                });
            }

            cart.TotalPrice = CartTotal(cart);
            await _repository.UpdateCartAsync(cart);
            await _repository.RemoveFromWishlistAsync(wishlistId);

            return cart;
        }

        // Pattern from screenshot
        public decimal CartTotal(Domain.Cart cart) => cart.Items.Sum(i => i.Price * i.Quantity);
    }
}
