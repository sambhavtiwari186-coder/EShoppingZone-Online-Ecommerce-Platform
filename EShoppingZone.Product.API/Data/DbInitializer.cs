using EShoppingZone.Product.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShoppingZone.Product.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return; // DB has been seeded
            }

            var products = new Domain.Product[]
            {
                // ── ELECTRONICS (10) ──────────────────────────────────
                new Domain.Product {
                    ProductId = 1, ProductType = "Regular", ProductName = "iPhone 15 Pro", Category = "Electronics",
                    Price = 129999, StockQuantity = 12, Description = "Titanium body with A17 Pro chip and 48MP camera system.",
                    MerchantId = 2,
                    Image = new List<string> { "https://images.unsplash.com/photo-1696446701796-da61225697cc?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "1240" } }
                },
                new Domain.Product {
                    ProductId = 2, ProductType = "Regular", ProductName = "MacBook Air M3", Category = "Electronics",
                    Price = 114900, StockQuantity = 8, Description = "Supercharged by M3 chip with 18-hour battery life.",
                    MerchantId = 2,
                    Image = new List<string> { "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "850" } }
                },
                new Domain.Product {
                    ProductId = 3, ProductType = "Regular", ProductName = "Sony WH-1000XM5", Category = "Electronics",
                    Price = 29999, StockQuantity = 25, Description = "Industry-leading noise cancellation headphones.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "2100" } }
                },
                new Domain.Product {
                    ProductId = 4, ProductType = "Regular", ProductName = "Samsung S24 Ultra", Category = "Electronics",
                    Price = 134999, StockQuantity = 15, Description = "Galaxy AI meets titanium — 200MP camera, S Pen included.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "980" } }
                },
                new Domain.Product {
                    ProductId = 5, ProductType = "Regular", ProductName = "Dell XPS 15 OLED", Category = "Electronics",
                    Price = 189999, StockQuantity = 5, Description = "OLED touch display, Intel Core i9, NVIDIA RTX 4070.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1593642632559-0c6d3fc62b89?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "430" } }
                },
                new Domain.Product {
                    ProductId = 6, ProductType = "Regular", ProductName = "iPad Pro 12.9\"", Category = "Electronics",
                    Price = 112900, StockQuantity = 10, Description = "M2 chip with Liquid Retina XDR display and Thunderbolt.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "670" } }
                },
                new Domain.Product {
                    ProductId = 7, ProductType = "Regular", ProductName = "Sony 4K OLED TV 55\"", Category = "Electronics",
                    Price = 149900, StockQuantity = 6, Description = "Triluminos Pro display with Cognitive Processor XR.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1593784991095-a205069470b6?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "310" } }
                },
                new Domain.Product {
                    ProductId = 8, ProductType = "Regular", ProductName = "Apple Watch Ultra 2", Category = "Electronics",
                    Price = 89900, StockQuantity = 20, Description = "The most rugged and capable Apple Watch with 60-hr battery.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1434494878577-86c23bcb06b9?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "780" } }
                },
                new Domain.Product {
                    ProductId = 9, ProductType = "Regular", ProductName = "Canon EOS R6 Mark II", Category = "Electronics",
                    Price = 239999, StockQuantity = 4, Description = "40fps burst, 6K RAW video, dual pixel AF – pro mirrorless.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "220" } }
                },
                new Domain.Product {
                    ProductId = 10, ProductType = "Regular", ProductName = "Bose QuietComfort 45", Category = "Electronics",
                    Price = 24999, StockQuantity = 30, Description = "World-class noise cancellation with TriPort acoustics.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1546435770-a3e426bf472b?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "1560" } }
                },

                // ── ACCESSORIES (10) ──────────────────────────────────
                new Domain.Product {
                    ProductId = 11, ProductType = "Regular", ProductName = "Rolex Submariner", Category = "Accessories",
                    Price = 1200000, StockQuantity = 2, Description = "The definitive diver's watch. Oystersteel, waterproof to 300m.",
                    MerchantId = 2,
                    Image = new List<string> { "https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "5.0" }, { "Count", "88" } }
                },
                new Domain.Product {
                    ProductId = 12, ProductType = "Regular", ProductName = "Ray-Ban Aviator Classic", Category = "Accessories",
                    Price = 12999, StockQuantity = 40, Description = "Iconic gold frame with G-15 green lenses. UV400 protection.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1572635196237-14b3f281503f?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "3400" } }
                },
                new Domain.Product {
                    ProductId = 13, ProductType = "Regular", ProductName = "Montblanc Meisterstück Pen", Category = "Accessories",
                    Price = 34999, StockQuantity = 10, Description = "The iconic ballpoint pen. Precision-crafted precious resin.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1583334648808-14f30c78b7e7?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "210" } }
                },
                new Domain.Product {
                    ProductId = 14, ProductType = "Regular", ProductName = "Louis Vuitton Scarf Silk", Category = "Accessories",
                    Price = 85000, StockQuantity = 5, Description = "100% silk twill, hand-rolled edges, iconic LV monogram.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1601924994987-69e26d50dc26?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "120" } }
                },
                new Domain.Product {
                    ProductId = 15, ProductType = "Regular", ProductName = "Leather Passport Holder", Category = "Accessories",
                    Price = 3499, StockQuantity = 60, Description = "Full-grain leather, RFID blocking, slim 3-card pocket design.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "890" } }
                },
                new Domain.Product {
                    ProductId = 16, ProductType = "Regular", ProductName = "Tag Heuer Carrera", Category = "Accessories",
                    Price = 285000, StockQuantity = 3, Description = "Swiss chronograph. Calibre Heuer 02, 43mm steel case.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1542496658-e33a6d0d3e62?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "145" } }
                },
                new Domain.Product {
                    ProductId = 17, ProductType = "Regular", ProductName = "Titanium Money Clip Wallet", Category = "Accessories",
                    Price = 2799, StockQuantity = 80, Description = "Aerospace-grade titanium, ultra-slim RFID wallet.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1599598425947-5202edd56fda?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.4" }, { "Count", "1230" } }
                },
                new Domain.Product {
                    ProductId = 18, ProductType = "Regular", ProductName = "Prada Nylon Backpack", Category = "Accessories",
                    Price = 135000, StockQuantity = 4, Description = "Re-Nylon fabric, triangular logo, padded laptop compartment.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "98" } }
                },
                new Domain.Product {
                    ProductId = 19, ProductType = "Regular", ProductName = "AirTag 4-Pack", Category = "Accessories",
                    Price = 10900, StockQuantity = 55, Description = "Find your things with the Precision Finding feature.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1611532736597-de2d4265fba3?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "4500" } }
                },
                new Domain.Product {
                    ProductId = 20, ProductType = "Regular", ProductName = "Gucci GG Belt 4cm", Category = "Accessories",
                    Price = 58000, StockQuantity = 7, Description = "Double G buckle on black leather. Made in Italy.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1594938298603-c8148c4dae35?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "340" } }
                },

                // ── FOOTWEAR (10) ──────────────────────────────────
                new Domain.Product {
                    ProductId = 21, ProductType = "Regular", ProductName = "Nike Air Jordan 1 Retro OG", Category = "Footwear",
                    Price = 18999, StockQuantity = 20, Description = "The OG silhouette. Tumbled leather upper, Air-Sole unit.",
                    MerchantId = 2,
                    Image = new List<string> { "https://images.unsplash.com/photo-1542291026-7eec264c27ff?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "5600" } }
                },
                new Domain.Product {
                    ProductId = 22, ProductType = "Regular", ProductName = "Adidas Ultraboost 24", Category = "Footwear",
                    Price = 17999, StockQuantity = 18, Description = "BOOST midsole returns energy with every stride. Primeknit+.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1608231387042-66d1773070a5?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "3200" } }
                },
                new Domain.Product {
                    ProductId = 23, ProductType = "Regular", ProductName = "Timberland 6-Inch Premium Boot", Category = "Footwear",
                    Price = 16499, StockQuantity = 14, Description = "Waterproof nubuck leather with rustproof hardware.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1520639888713-7851133b1ed0?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "2100" } }
                },
                new Domain.Product {
                    ProductId = 24, ProductType = "Regular", ProductName = "New Balance 990v6 Made in USA", Category = "Footwear",
                    Price = 22999, StockQuantity = 10, Description = "Domestic craftsmanship, ENCAP midsole, pigskin suede.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1491553895911-0055eca6402d?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "1450" } }
                },
                new Domain.Product {
                    ProductId = 25, ProductType = "Regular", ProductName = "Converse Chuck 70 Hi", Category = "Footwear",
                    Price = 7999, StockQuantity = 35, Description = "Premium canvas upper with OrthoLite insole. Classic hi-top.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1463100099107-aa0980c362e6?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "7800" } }
                },
                new Domain.Product {
                    ProductId = 26, ProductType = "Regular", ProductName = "Salvatore Ferragamo Leather Oxford", Category = "Footwear",
                    Price = 98000, StockQuantity = 5, Description = "Hand-stitched calfskin with the iconic Gancini buckle.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1449505278894-297fdb3edbc1?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "98" } }
                },
                new Domain.Product {
                    ProductId = 27, ProductType = "Regular", ProductName = "Puma RS-X Efekt", Category = "Footwear",
                    Price = 8999, StockQuantity = 28, Description = "Chunky 90s silhouette, RS foam, breathable mesh upper.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.3" }, { "Count", "1900" } }
                },
                new Domain.Product {
                    ProductId = 28, ProductType = "Regular", ProductName = "Dr. Martens 1460 Mono", Category = "Footwear",
                    Price = 14999, StockQuantity = 16, Description = "All-black smooth leather with iconic welt stitch and air-cushioned sole.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1595341888016-a392ef81b7de?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "2780" } }
                },
                new Domain.Product {
                    ProductId = 29, ProductType = "Regular", ProductName = "Vans Old Skool Pro", Category = "Footwear",
                    Price = 7499, StockQuantity = 40, Description = "DURACAP upper, Sickle foxing, classic skate DNA.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1525966222134-fcfa99b8ae77?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "4320" } }
                },
                new Domain.Product {
                    ProductId = 30, ProductType = "Regular", ProductName = "Birkenstock Arizona Premium", Category = "Footwear",
                    Price = 8999, StockQuantity = 22, Description = "Suede leather footbed, cork-latex sole, anatomical arch support.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "3100" } }
                },

                // ── LIFESTYLE (10) ──────────────────────────────────
                new Domain.Product {
                    ProductId = 31, ProductType = "Regular", ProductName = "Herman Miller Aeron Chair", Category = "Lifestyle",
                    Price = 145000, StockQuantity = 6, Description = "8Z Pellicle suspension, PostureFit SL, fully adjustable. Made for life.",
                    MerchantId = 2,
                    Image = new List<string> { "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "620" } }
                },
                new Domain.Product {
                    ProductId = 32, ProductType = "Regular", ProductName = "Dyson Airwrap Complete", Category = "Lifestyle",
                    Price = 49900, StockQuantity = 12, Description = "Styles, waves, curls and dries. No extreme heat damage.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "1800" } }
                },
                new Domain.Product {
                    ProductId = 33, ProductType = "Regular", ProductName = "Nespresso Vertuo Next", Category = "Lifestyle",
                    Price = 18999, StockQuantity = 20, Description = "Centrifusion extraction, 5 cup sizes, 30s heat-up time.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "2340" } }
                },
                new Domain.Product {
                    ProductId = 34, ProductType = "Regular", ProductName = "Kindle Paperwhite 16GB", Category = "Lifestyle",
                    Price = 14999, StockQuantity = 30, Description = "6.8\" warm backlit display, IPX8 waterproof, weeks of battery.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "5600" } }
                },
                new Domain.Product {
                    ProductId = 35, ProductType = "Regular", ProductName = "Moleskine Classic Notebook Large", Category = "Lifestyle",
                    Price = 1899, StockQuantity = 100, Description = "240 ivory pages, elastic closure, expandable inner pocket.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1531346680769-a1d79b57de5c?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "9800" } }
                },
                new Domain.Product {
                    ProductId = 36, ProductType = "Regular", ProductName = "Philips Hue Starter Kit", Category = "Lifestyle",
                    Price = 12999, StockQuantity = 18, Description = "3 A19 White & Color bulbs + Bridge. 16M colors, voice control.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "3400" } }
                },
                new Domain.Product {
                    ProductId = 37, ProductType = "Regular", ProductName = "Ember Mug 2 Travel", Category = "Lifestyle",
                    Price = 14999, StockQuantity = 25, Description = "Smart mug that maintains your perfect temperature all day.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1514228742587-6b1558fcca3d?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "2100" } }
                },
                new Domain.Product {
                    ProductId = 38, ProductType = "Regular", ProductName = "LEGO Architecture Tokyo 21051", Category = "Lifestyle",
                    Price = 8499, StockQuantity = 15, Description = "547 pieces. Tokyo skyline model for display or building fun.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1562255818-e428cf6e1e23?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "1780" } }
                },
                new Domain.Product {
                    ProductId = 39, ProductType = "Regular", ProductName = "Vitamix A3500 Ascent Blender", Category = "Lifestyle",
                    Price = 89999, StockQuantity = 7, Description = "5 program settings, self-detect containers, wireless connectivity.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1570197571499-166b36435e9f?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "890" } }
                },
                new Domain.Product {
                    ProductId = 40, ProductType = "Regular", ProductName = "Echoes Max Echo 4th Gen", Category = "Lifestyle",
                    Price = 9999, StockQuantity = 45, Description = "Premium sound, 360° audio, Alexa built-in, smart home hub.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1512446816042-444d641267d4?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "6700" } }
                },

                // ── FASHION (10) ──────────────────────────────────
                new Domain.Product {
                    ProductId = 41, ProductType = "Regular", ProductName = "Gucci GG Canvas Tote", Category = "Fashion",
                    Price = 245000, StockQuantity = 4, Description = "GG Supreme canvas with leather trim. Spacious & iconic.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "340" } }
                },
                new Domain.Product {
                    ProductId = 42, ProductType = "Regular", ProductName = "Levi's 501 Original Jeans", Category = "Fashion",
                    Price = 4999, StockQuantity = 80, Description = "The original button-fly jeans. 100% cotton denim.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.5" }, { "Count", "12000" } }
                },
                new Domain.Product {
                    ProductId = 43, ProductType = "Regular", ProductName = "Burberry Classic Check Trench Coat", Category = "Fashion",
                    Price = 450000, StockQuantity = 3, Description = "Double-breasted in gabardine, iconic London check lining.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1539533113208-f6df8cc8b543?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "156" } }
                },
                new Domain.Product {
                    ProductId = 44, ProductType = "Regular", ProductName = "Ralph Lauren Oxford Shirt", Category = "Fashion",
                    Price = 9999, StockQuantity = 35, Description = "Classic fit, garment-washed, soft 100% cotton Oxford cloth.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.6" }, { "Count", "3200" } }
                },
                new Domain.Product {
                    ProductId = 45, ProductType = "Regular", ProductName = "Zara Oversized Blazer", Category = "Fashion",
                    Price = 8999, StockQuantity = 22, Description = "Textured oversized blazer with lapel collar and front pockets.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1594938298603-c8148c4dae35?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.3" }, { "Count", "4500" } }
                },
                new Domain.Product {
                    ProductId = 46, ProductType = "Regular", ProductName = "H&M Cashmere Sweater", Category = "Fashion",
                    Price = 5999, StockQuantity = 40, Description = "Fine-knit 100% cashmere crew-neck in 8 seasonal colors.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.4" }, { "Count", "6700" } }
                },
                new Domain.Product {
                    ProductId = 47, ProductType = "Regular", ProductName = "Hermès Silk Pocket Square", Category = "Fashion",
                    Price = 38000, StockQuantity = 8, Description = "Hand-rolled edges, 45×45cm, exclusive print on 100% silk twill.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1519689680058-324335c77eba?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "210" } }
                },
                new Domain.Product {
                    ProductId = 48, ProductType = "Regular", ProductName = "Uniqlo Ultra-Light Down Jacket", Category = "Fashion",
                    Price = 5999, StockQuantity = 50, Description = "90% down, 10% feather fill. Packs into its own pocket.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1551028719-00167b16eac5?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.7" }, { "Count", "8900" } }
                },
                new Domain.Product {
                    ProductId = 49, ProductType = "Regular", ProductName = "The North Face Nuptse 1996", Category = "Fashion",
                    Price = 24999, StockQuantity = 12, Description = "700-fill goose down, retro boxy silhouette, water-resistant.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1547949003-9792a18a2601?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.8" }, { "Count", "3400" } }
                },
                new Domain.Product {
                    ProductId = 50, ProductType = "Regular", ProductName = "Valentino Studded Clutch", Category = "Fashion",
                    Price = 185000, StockQuantity = 3, Description = "Iconic Rockstud clutch in smooth calfskin with gold tone studs.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "78" } }
                },
                new Domain.Product {
                    ProductId = 51, ProductType = "Regular", ProductName = "Nike Air Max Red Premium", Category = "Footwear",
                    Price = 15999, StockQuantity = 50, Description = "High-performance running shoes with breathable Flyknit upper and responsive cushioning. Perfect for athletes and casual wearers alike.",
                    Image = new List<string> { "https://images.unsplash.com/photo-1542291026-7eec264c27ff?q=80&w=800" },
                    Rating = new Dictionary<string, string> { { "Rate", "4.9" }, { "Count", "1200" } }
                },
            };

            foreach (var p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}
