FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build

# Copy the global and csproj files
COPY ./src/framework/VND.FW.Utils/VND.Fw.Utils.csproj ./src/framework/VND.FW.Utils/
COPY ./src/framework/VND.FW.Domain/VND.Fw.Domain.csproj ./src/framework/VND.FW.Domain/
COPY ./src/framework/VND.FW.Infrastructure.AspNetCore/VND.FW.Infrastructure.AspNetCore.csproj ./src/framework/VND.FW.Infrastructure.AspNetCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore/VND.FW.Infrastructure.EfCore.csproj ./src/framework/VND.FW.Infrastructure.EfCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/VND.FW.Infrastructure.EfCore.SqlServer.csproj ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/
COPY ./src/shared/VND.CoolStore.Shared.Cart/VND.CoolStore.Shared.Cart.csproj ./src/shared/VND.CoolStore.Shared.Cart/
COPY ./src/shared/VND.CoolStore.Shared.Catalog/VND.CoolStore.Shared.Catalog.csproj ./src/shared/VND.CoolStore.Shared.Catalog/

COPY ./src/services/cart/VND.CoolStore.Services.Cart.csproj ./src/services/cart/
COPY ./global.json ./
COPY ./NuGet.config ./

# Restore all packages
RUN dotnet restore ./src/services/cart/VND.CoolStore.Services.Cart.csproj --force --no-cache

# Copy the remaining source
COPY ./src/framework/VND.FW.Utils/ ./src/framework/VND.FW.Utils/
COPY ./src/framework/VND.FW.Domain/ ./src/framework/VND.FW.Domain/
COPY ./src/framework/VND.FW.Infrastructure.AspNetCore/ ./src/framework/VND.FW.Infrastructure.AspNetCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore/ ./src/framework/VND.FW.Infrastructure.EfCore/
COPY ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/ ./src/framework/VND.FW.Infrastructure.EfCore.SqlServer/
COPY ./src/shared/VND.CoolStore.Shared.Cart/ ./src/shared/VND.CoolStore.Shared.Cart/
COPY ./src/shared/VND.CoolStore.Shared.Catalog/ ./src/shared/VND.CoolStore.Shared.Catalog/

COPY ./src/services/cart/ ./src/services/cart/

# Build the source code
RUN dotnet build ./src/services/cart/VND.CoolStore.Services.Cart.csproj --configuration Release --no-restore

# Publish application
RUN dotnet publish ./src/services/cart/VND.CoolStore.Services.Cart.csproj --configuration Release --no-restore --no-build --output "../../../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:5003
EXPOSE 5003

ENTRYPOINT ["dotnet", "VND.CoolStore.Services.Cart.dll"]
