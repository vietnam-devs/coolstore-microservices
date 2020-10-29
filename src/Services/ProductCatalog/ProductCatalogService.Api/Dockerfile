#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5003
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Services/ProductCatalog/ProductCatalogService.Api/ProductCatalogService.Api.csproj", "src/Services/ProductCatalog/ProductCatalogService.Api/"]
COPY ["src/Services/ProductCatalog/ProductCatalogService/ProductCatalogService.csproj", "src/Services/ProductCatalog/ProductCatalogService/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.App/N8T.Infrastructure.App.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.App/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.Cache/N8T.Infrastructure.Cache.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.Cache/"]
COPY ["src/BasicBuildingBlocks/N8T.Domain/N8T.Domain.csproj", "src/BasicBuildingBlocks/N8T.Domain/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure/N8T.Infrastructure.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.GraphQL/N8T.Infrastructure.GraphQL.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.GraphQL/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.Dapper/N8T.Infrastructure.Dapper.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.Dapper/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.OTel/N8T.Infrastructure.OTel.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.OTel/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/N8T.Infrastructure.EfCore.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/"]
RUN dotnet restore "src/Services/ProductCatalog/ProductCatalogService.Api/ProductCatalogService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ProductCatalog/ProductCatalogService.Api"
RUN dotnet build "ProductCatalogService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalogService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalogService.Api.dll"]
