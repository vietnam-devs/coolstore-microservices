#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5005
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Services/Sale/SaleService.Api/SaleService.Api.csproj", "src/Services/Sale/SaleService.Api/"]
COPY ["src/Services/Sale/SaleService/SaleService.csproj", "src/Services/Sale/SaleService/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.App/N8T.Infrastructure.App.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.App/"]
COPY ["src/BasicBuildingBlocks/N8T.Domain/N8T.Domain.csproj", "src/BasicBuildingBlocks/N8T.Domain/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure/N8T.Infrastructure.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.OTel/N8T.Infrastructure.OTel.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.OTel/"]
COPY ["src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/N8T.Infrastructure.EfCore.csproj", "src/BasicBuildingBlocks/N8T.Infrastructure.EfCore/"]
RUN dotnet restore "src/Services/Sale/SaleService.Api/SaleService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Sale/SaleService.Api"
RUN dotnet build "SaleService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SaleService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaleService.Api.dll"]
