FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/VND.CoolStore.ProductCatalog.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/"]
RUN dotnet restore "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog/VND.CoolStore.ProductCatalog.csproj"
COPY . .
WORKDIR "/src/src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog"
RUN dotnet build "VND.CoolStore.ProductCatalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.ProductCatalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.ProductCatalog.dll"]