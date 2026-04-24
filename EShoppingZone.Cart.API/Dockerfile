FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EShoppingZone.Cart.API.csproj", "./"]
RUN dotnet restore "EShoppingZone.Cart.API.csproj"
COPY . .
RUN dotnet build "EShoppingZone.Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShoppingZone.Cart.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShoppingZone.Cart.API.dll"]
