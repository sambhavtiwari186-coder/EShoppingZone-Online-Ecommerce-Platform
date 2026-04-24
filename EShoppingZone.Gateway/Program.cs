using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add Swagger for Gateway
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShoppingZone.Gateway", Version = "v1" });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API");
        c.SwaggerEndpoint("/profile-docs/v1/swagger.json", "Profile API");
        c.SwaggerEndpoint("/product-docs/v1/swagger.json", "Product API");
        c.SwaggerEndpoint("/cart-docs/v1/swagger.json", "Cart API");
        c.SwaggerEndpoint("/order-docs/v1/swagger.json", "Order API");
        c.SwaggerEndpoint("/wallet-docs/v1/swagger.json", "Wallet API");
        c.SwaggerEndpoint("/review-docs/v1/swagger.json", "Review API");
        c.SwaggerEndpoint("/notify-docs/v1/swagger.json", "Notify API");
        c.RoutePrefix = "swagger";
    });

}


app.MapReverseProxy();

app.Run();
