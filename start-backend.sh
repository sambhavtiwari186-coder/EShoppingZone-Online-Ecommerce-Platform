#!/bin/bash
echo "Starting microservices..."
dotnet run --project EShoppingZone.Profile.API > profile.log 2>&1 &
dotnet run --project EShoppingZone.Product.API > product.log 2>&1 &
dotnet run --project EShoppingZone.Cart.API > cart.log 2>&1 &
dotnet run --project EShoppingZone.Orders.API > orders.log 2>&1 &
dotnet run --project EShoppingZone.Wallet.API > wallet.log 2>&1 &
dotnet run --project EShoppingZone.Review.API > review.log 2>&1 &
dotnet run --project EShoppingZone.Notify.API > notify.log 2>&1 &
dotnet run --project EShoppingZone.Gateway > gateway.log 2>&1 &
echo "All microservices starting in background."
