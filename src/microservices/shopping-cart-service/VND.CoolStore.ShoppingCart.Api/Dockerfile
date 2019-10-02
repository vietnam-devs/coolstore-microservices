FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-bionic AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 15003

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Api/VND.CoolStore.ShoppingCart.Api.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Api/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/VND.CoolStore.ShoppingCart.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/VND.CoolStore.ShoppingCart.DataContracts.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/"]
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/VND.CoolStore.Inventory.DataContracts.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
RUN dotnet restore "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Api/VND.CoolStore.ShoppingCart.Api.csproj"
COPY . .
WORKDIR "/src/src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Api"
RUN dotnet build "VND.CoolStore.ShoppingCart.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.ShoppingCart.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.ShoppingCart.Api.dll"]
