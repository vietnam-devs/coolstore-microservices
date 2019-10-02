FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-bionic AS base
WORKDIR /app
EXPOSE 5004
EXPOSE 15004

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory.Api/VND.CoolStore.Inventory.Api.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory.Api/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory/VND.CoolStore.Inventory.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory/"]
COPY ["src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/VND.CoolStore.Inventory.DataContracts.csproj", "src/microservices/inventory-service/VND.CoolStore.Inventory.DataContracts/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
RUN dotnet restore "src/microservices/inventory-service/VND.CoolStore.Inventory.Api/VND.CoolStore.Inventory.Api.csproj"
COPY . .
WORKDIR "/src/src/microservices/inventory-service/VND.CoolStore.Inventory.Api"
RUN dotnet build "VND.CoolStore.Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.Inventory.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.Inventory.Api.dll"]
