FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1-bionic AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 15003

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1-bionic AS build
WORKDIR /src
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/VND.CoolStore.ShoppingCart.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/VND.CoolStore.ShoppingCart.DataContracts.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.DataContracts/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.AppService/VND.CoolStore.ShoppingCart.AppService.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.AppService/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Data/VND.CoolStore.ShoppingCart.Data.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Data/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Domain/VND.CoolStore.ShoppingCart.Domain.csproj", "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
RUN dotnet restore "src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/VND.CoolStore.ShoppingCart.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart"
RUN dotnet build "VND.CoolStore.ShoppingCart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.ShoppingCart.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.ShoppingCart.dll"]
