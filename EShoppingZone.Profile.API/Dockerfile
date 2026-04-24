FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EShoppingZone.Profile.API.csproj", "./"]
RUN dotnet restore "EShoppingZone.Profile.API.csproj"
COPY . .
RUN dotnet build "EShoppingZone.Profile.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShoppingZone.Profile.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShoppingZone.Profile.API.dll"]
