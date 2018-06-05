FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build
COPY ./src/services/inventory/VND.Services.Inventory.csproj ./src/services/inventory/VND.Services.Inventory.csproj
COPY ./global.json ./global.json
RUN dotnet restore ./src/services/inventory/VND.Services.Inventory.csproj --force --no-cache
COPY ./src/services/inventory/ ./src/services/inventory/
RUN dotnet build ./src/services/inventory/VND.Services.Inventory.csproj --configuration Release --no-restore
RUN dotnet publish ./src/services/inventory/VND.Services.Inventory.csproj --configuration Release --no-restore --no-build --output "../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:80

ENTRYPOINT ["dotnet", "Inventory.dll"]