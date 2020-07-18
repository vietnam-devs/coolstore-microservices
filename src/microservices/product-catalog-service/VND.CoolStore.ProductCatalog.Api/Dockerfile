FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-bionic AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 15002

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Api/VND.CoolStore.ProductCatalog.Api.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Api/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/VND.CoolStore.ProductCatalog.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/"]
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/VND.CoolStore.Inventory.DataContracts.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/"]

RUN dotnet restore "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Api/VND.CoolStore.ProductCatalog.Api.csproj"
COPY src/building-blocks src/building-blocks
COPY src/schema/grpc/v1 src/schema/grpc/v1
COPY src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts
COPY src/microservices/product-catalog-service src/microservices/product-catalog-service
WORKDIR "/src/src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Api"
RUN dotnet build "VND.CoolStore.ProductCatalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.ProductCatalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.ProductCatalog.Api.dll"]
