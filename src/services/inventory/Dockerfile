FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build
EXPOSE 5004

# Copy the global and csproj files
COPY ./src/framework/VND.FW.Utils/VND.Fw.Utils.csproj ./src/framework/VND.FW.Utils/
COPY ./src/framework/VND.FW.Domain/VND.Fw.Domain.csproj ./src/framework/VND.FW.Domain/
COPY ./src/framework/VND.FW.Infrastructure.AspNetCore/VND.FW.Infrastructure.AspNetCore.csproj ./src/framework/VND.FW.Infrastructure.AspNetCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore/VND.FW.Infrastructure.EfCore.csproj ./src/framework/VND.FW.Infrastructure.EfCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/VND.FW.Infrastructure.EfCore.SqlServer.csproj ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/
COPY ./src/services/inventory/VND.CoolStore.Services.Inventory.csproj ./src/services/inventory/
COPY ./global.json ./
COPY ./NuGet.config ./

# Restore all packages
RUN dotnet restore ./src/services/inventory/VND.CoolStore.Services.Inventory.csproj --force --no-cache

# Copy the remaining source
COPY ./src/framework/VND.FW.Utils/ ./src/framework/VND.FW.Utils/
COPY ./src/framework/VND.FW.Domain/ ./src/framework/VND.FW.Domain/
COPY ./src/framework/VND.FW.Infrastructure.AspNetCore/ ./src/framework/VND.FW.Infrastructure.AspNetCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore/ ./src/framework/VND.FW.Infrastructure.EfCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/ ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/
COPY ./src/services/inventory/ ./src/services/inventory/

# Build the source code
RUN dotnet build ./src/services/inventory/VND.CoolStore.Services.Inventory.csproj --configuration Release --no-restore

# Publish application
RUN dotnet publish ./src/services/inventory/VND.CoolStore.Services.Inventory.csproj --configuration Release --no-restore --no-build --output "../../../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:5004

ENTRYPOINT ["dotnet", "VND.CoolStore.Services.Inventory.dll"]
