FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/gateways/webapi-gateway/VND.CoolStore.WebApiGateway.csproj", "src/gateways/webapi-gateway/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/VND.CoolStore.ShoppingCart.DataContracts.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/"]
RUN dotnet restore "src/gateways/webapi-gateway/VND.CoolStore.WebApiGateway.csproj"
COPY . .
WORKDIR "/src/src/gateways/webapi-gateway"
RUN dotnet build "VND.CoolStore.WebApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.WebApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#RUN ls -la && cat ocelot.json
ENTRYPOINT ["dotnet", "VND.CoolStore.WebApiGateway.dll"]
