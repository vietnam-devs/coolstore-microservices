FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /build
EXPOSE 5001

# Copy the global and csproj files
COPY ./src/services/idp/VND.CoolStore.Services.Idp.csproj ./src/services/idp/VND.CoolStore.Services.Idp.csproj
COPY ./global.json ./global.json

# Restore all packages
RUN dotnet restore ./src/services/idp/VND.CoolStore.Services.Idp.csproj --force --no-cache -nowarn:msb3202,nu1503

# Copy the remaining source
COPY ./src/services/idp/ ./src/services/idp/

# Build the source code
RUN dotnet build ./src/services/idp/VND.CoolStore.Services.Idp.csproj --configuration Release --no-restore

# Publish application
RUN dotnet publish ./src/services/idp/VND.CoolStore.Services.Idp.csproj --configuration Release --no-restore --no-build --output "../../../dist"

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /build/dist .
ENV ASPNETCORE_URLS http://+:5001

ENTRYPOINT ["dotnet", "VND.CoolStore.Services.Idp.dll"]
