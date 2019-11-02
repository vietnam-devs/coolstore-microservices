FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src
COPY ["src/migrations/VND.CoolStore.DbMigration/VND.CoolStore.DbMigration.csproj", "src/migrations/VND.CoolStore.DbMigration/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/microservices/search-service/VND.CoolStore.Search.DataContracts/VND.CoolStore.Search.DataContracts.csproj", "src/microservices/search-service/VND.CoolStore.Search.DataContracts/"]
RUN dotnet restore "src/migrations/VND.CoolStore.DbMigration/VND.CoolStore.DbMigration.csproj"
COPY . .
WORKDIR "/src/src/migrations/VND.CoolStore.DbMigration"
RUN dotnet build "VND.CoolStore.DbMigration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.DbMigration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/src/migrations/VND.CoolStore.DbMigration/Scripts/ ./Scripts/
#RUN ls -la && pwd
ENTRYPOINT ["dotnet", "VND.CoolStore.DbMigration.dll", "shoppingcart", "productcatalog", "inventory", "search"]
