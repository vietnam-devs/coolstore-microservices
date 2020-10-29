#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5004
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Services/ShoppingCart/ShoppingCartService.Api/ShoppingCartService.Api.csproj", "src/Services/ShoppingCart/ShoppingCartService.Api/"]
COPY ["src/Services/ShoppingCart/ShoppingCartService/ShoppingCartService.csproj", "src/Services/ShoppingCart/ShoppingCartService/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.App/N8T.Infrastructure.App.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.App/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.Cache/N8T.Infrastructure.Cache.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.Cache/"]
COPY ["src/BasicBuildingBlocks/N8T.Domain/N8T.Domain.csproj", "src/BasicBuildingBlocks/N8T.Domain/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure/N8T.Infrastructure.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.GraphQL/N8T.Infrastructure.GraphQL.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.GraphQL/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.Dapper/N8T.Infrastructure.Dapper.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.Dapper/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.OTel/N8T.Infrastructure.OTel.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.OTel/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/N8T.Infrastructure.EfCore.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/"]
RUN dotnet restore "src/Services/ShoppingCart/ShoppingCartService.Api/ShoppingCartService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ShoppingCart/ShoppingCartService.Api"
RUN dotnet build "ShoppingCartService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingCartService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCartService.Api.dll"]
