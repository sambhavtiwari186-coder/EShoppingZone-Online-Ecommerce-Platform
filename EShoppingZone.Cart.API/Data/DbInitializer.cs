using EShoppingZone.Cart.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Cart.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CartDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Carts.Any())
            {
                return;
            }

            var carts = new Domain.Cart[]
            {
                new Domain.Cart { 
                    CartId = 1, 
                    Items = new List<CartItem>
                    {
                        new CartItem { ProductId = 1, ProductName = "iPhone 15 Pro", Price = 129999, Quantity = 1, ImageUrl = "https://images.unsplash.com/photo-1696446701796-da61225697cc?q=80\u0026w=1200" }
                    },
                    TotalPrice = 129999
                }
            };

            foreach (var c in carts)
            {
                context.Carts.Add(c);
            }
            context.SaveChanges();
        }
    }
}
