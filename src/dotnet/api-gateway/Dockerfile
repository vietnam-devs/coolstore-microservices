#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["api-gateway/Gateway.csproj", "api-gateway/"]
RUN dotnet restore "api-gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/api-gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# COPY certs/localhost.crt /usr/local/share/ca-certificates
# RUN chmod 644 /usr/local/share/ca-certificates/localhost.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "Gateway.dll"]
