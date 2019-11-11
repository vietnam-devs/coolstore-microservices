#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-bionic AS base
WORKDIR /app
EXPOSE 5010
EXPOSE 15010

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
WORKDIR /src

COPY ["src/building-blocks/CloudNativeKit.Domain/CloudNativeKit.Domain.csproj", "src/building-blocks/CloudNativeKit.Domain/"]
COPY ["src/building-blocks/CloudNativeKit.Utils/CloudNativeKit.Utils.csproj", "src/building-blocks/CloudNativeKit.Utils/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure/CloudNativeKit.Infrastructure.csproj", "src/building-blocks/CloudNativeKit.Infrastructure/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Bus/CloudNativeKit.Infrastructure.Bus.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Bus/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Data/CloudNativeKit.Infrastructure.Data.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Data/"]
COPY ["src/building-blocks/CloudNativeKit.Infrastructure.Tracing/CloudNativeKit.Infrastructure.Tracing.csproj", "src/building-blocks/CloudNativeKit.Infrastructure.Tracing/"]

COPY ["src/microservices/search-service/VND.CoolStore.Search.Api/VND.CoolStore.Search.Api.csproj", "src/microservices/search-service/VND.CoolStore.Search.Api/"]
COPY ["src/microservices/search-service/VND.CoolStore.Search.DataContracts/VND.CoolStore.Search.DataContracts.csproj", "src/microservices/search-service/VND.CoolStore.Search.DataContracts/"]
COPY ["src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/VND.CoolStore.ProductCatalog.DataContracts.csproj", "src/microservices/product-catalog-service/VND.CoolStore.ProductCatalog.DataContracts/"]
COPY ["src/microservices/search-service/VND.CoolStore.Search/VND.CoolStore.Search.csproj", "src/microservices/search-service/VND.CoolStore.Search/"]

RUN dotnet restore "src/microservices/search-service/VND.CoolStore.Search.Api/VND.CoolStore.Search.Api.csproj"
COPY . .
WORKDIR "/src/src/microservices/search-service/VND.CoolStore.Search.Api"
RUN dotnet build "VND.CoolStore.Search.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.Search.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.Search.Api.dll"]
