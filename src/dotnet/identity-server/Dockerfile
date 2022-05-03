#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["identity-server/IdentityServer.csproj", "identity-server/"]
RUN dotnet restore "identity-server/IdentityServer.csproj"
COPY . .
WORKDIR "/src/identity-server"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# COPY certs/localhost.crt /usr/local/share/ca-certificates
# RUN chmod 644 /usr/local/share/ca-certificates/localhost.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
