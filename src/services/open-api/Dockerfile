FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine AS base
WORKDIR /app

ARG service_version
ENV SERVICE_VERSION ${service_version:-0.0.1}

ARG api_version
ENV API_VERSION ${api_version:-1.0}

ENV ASPNETCORE_URLS http://+:5010
EXPOSE 5010

FROM microsoft/dotnet:2.2.100-sdk-alpine AS build
WORKDIR /src/src/services/open-api

COPY /src/services/open-api/*csproj /src/src/services/open-api/
RUN dotnet restore /property:Configuration=Release -nowarn:msb3202,nu1503

COPY /src/services/open-api/. /src/src/services/open-api/
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.OpenApi.dll"]
