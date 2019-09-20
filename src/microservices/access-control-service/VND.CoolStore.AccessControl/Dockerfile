FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/microservices/access-control-service/VND.CoolStore.AccessControl/VND.CoolStore.AccessControl.csproj", "src/microservices/access-control-service/VND.CoolStore.AccessControl/"]
RUN dotnet restore "src/microservices/access-control-service/VND.CoolStore.AccessControl/VND.CoolStore.AccessControl.csproj"
COPY . .
WORKDIR "/src/src/microservices/access-control-service/VND.CoolStore.AccessControl"
RUN dotnet build "VND.CoolStore.AccessControl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VND.CoolStore.AccessControl.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VND.CoolStore.AccessControl.dll"]
