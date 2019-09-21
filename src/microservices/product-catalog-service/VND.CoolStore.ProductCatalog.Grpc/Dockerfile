FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1-bionic AS build
WORKDIR /src
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Grpc/VND.CoolStore.ProductCatalog.Grpc.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Grpc/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/VND.CoolStore.ProductCatalog.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
COPY ["src/building-blocks/VND.CoolStore.Certs/VND.CoolStore.Certs.csproj", "src/building-blocks/VND.CoolStore.Certs/"]
RUN dotnet restore "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Grpc/VND.CoolStore.ProductCatalog.Grpc.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.Grpc"
RUN dotnet build "VND.CoolStore.ProductCatalog.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.ProductCatalog.Grpc.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
EXPOSE 15002
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.ProductCatalog.Grpc.dll"]
