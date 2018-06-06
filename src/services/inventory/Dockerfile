FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build

# Copy the global and csproj files
COPY ./src/services/inventory/VND.Services.Inventory.csproj ./src/services/inventory/VND.Services.Inventory.csproj
COPY ./global.json ./global.json

# Restore all packages
RUN dotnet restore ./src/services/inventory/VND.Services.Inventory.csproj --force --no-cache

# Copy the remaining source
COPY ./src/services/inventory/ ./src/services/inventory/

# Build the source code
RUN dotnet build ./src/services/inventory/VND.Services.Inventory.csproj --configuration Release --no-restore

# Publish application
RUN dotnet publish ./src/services/inventory/VND.Services.Inventory.csproj --configuration Release --no-restore --no-build --output "../../../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:5000

ENTRYPOINT ["dotnet", "VND.Services.Inventory.dll"]