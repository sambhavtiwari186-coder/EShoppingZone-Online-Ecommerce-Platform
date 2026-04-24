FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EShoppingZone.Review.API.csproj", "./"]
RUN dotnet restore "EShoppingZone.Review.API.csproj"
COPY . .
RUN dotnet build "EShoppingZone.Review.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShoppingZone.Review.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShoppingZone.Review.API.dll"]
