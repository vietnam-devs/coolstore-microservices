FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build

# Copy the global and csproj files
COPY ./src/services/gateway/VND.CoolStore.Services.ApiGateway.csproj ./src/services/gateway/VND.CoolStore.Services.ApiGateway.csproj
COPY ./global.json ./global.json

# Restore all packages
RUN dotnet restore ./src/services/gateway/VND.CoolStore.Services.ApiGateway.csproj --force --no-cache

# Copy the remaining source
COPY ./src/services/gateway/ ./src/services/gateway/

# Build the source code
RUN dotnet build ./src/services/gateway/VND.CoolStore.Services.ApiGateway.csproj --configuration Release --no-restore

# Publish application
RUN dotnet publish ./src/services/gateway/VND.CoolStore.Services.ApiGateway.csproj --configuration Release --no-restore --no-build --output "../../../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:5000

ENTRYPOINT ["dotnet", "VND.CoolStore.Services.ApiGateway.dll"]
