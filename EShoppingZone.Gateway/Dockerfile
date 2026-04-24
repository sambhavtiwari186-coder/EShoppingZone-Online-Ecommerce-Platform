FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EShoppingZone.Gateway.csproj", "./"]
RUN dotnet restore "EShoppingZone.Gateway.csproj"
COPY . .
RUN dotnet build "EShoppingZone.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShoppingZone.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShoppingZone.Gateway.dll"]
