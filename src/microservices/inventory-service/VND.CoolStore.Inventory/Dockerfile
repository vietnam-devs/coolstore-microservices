FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/microservices/inventory-service/VND.CoolStore.InventoryService/VND.CoolStore.InventoryService.csproj", "src/microservices/inventory-service/VND.CoolStore.InventoryService/"]
RUN dotnet restore "src/microservices/inventory-service/VND.CoolStore.InventoryService/VND.CoolStore.InventoryService.csproj"
COPY . .
WORKDIR "/src/src/microservices/inventory-service/VND.CoolStore.InventoryService"
RUN dotnet build "VND.CoolStore.InventoryService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.InventoryService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.InventoryService.dll"]