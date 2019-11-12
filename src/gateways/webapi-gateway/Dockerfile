FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src

COPY ["src/gateways/webapi-gateway/VND.CoolStore.WebApiGateway.csproj", "src/gateways/webapi-gateway/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Tracing/CloudNativeKit.Infrastructure.Tracing.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Tracing/"]
COPY ["src/building-blocks/GrpcJsonTranscoder/GrpcJsonTranscoder.csproj", "src/building-blocks/GrpcJsonTranscoder/"]

COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/VND.CoolStore.ShoppingCart.DataContracts.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/VND.CoolStore.Inventory.DataContracts.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/"]
COPY ["src/microservices/search-service/VND.CoolStore.Search.DataContracts/VND.CoolStore.Search.DataContracts.csproj", "src/microservices/search-service/VND.CoolStore.Search.DataContracts/"]

RUN dotnet restore "src/gateways/webapi-gateway/VND.CoolStore.WebApiGateway.csproj"
COPY . .
WORKDIR "/src/src/gateways/webapi-gateway"
RUN dotnet build "VND.CoolStore.WebApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.WebApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.WebApiGateway.dll"]
