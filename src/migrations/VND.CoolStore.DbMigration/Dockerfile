FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/migrations/VND.CoolStore.DbMigration/VND.CoolStore.DbMigration.csproj", "src/migrations/VND.CoolStore.DbMigration/"]
RUN dotnet restore "src/migrations/VND.CoolStore.DbMigration/VND.CoolStore.DbMigration.csproj"
COPY . .
WORKDIR "/src/src/migrations/VND.CoolStore.DbMigration"
RUN dotnet build "VND.CoolStore.DbMigration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.DbMigration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/src/migrations/VND.CoolStore.DbMigration/Scripts/ ./Scripts/
#RUN ls -la && pwd
