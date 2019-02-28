FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine AS base
WORKDIR /app

ARG service_version
ENV SERVICE_VERSION ${service_version:-0.0.1}

ARG api_version
ENV API_VERSION ${api_version:-1.0}

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

FROM microsoft/dotnet:2.2.100-sdk-alpine AS build
WORKDIR /src/src/services/idp

COPY /src/services/idp/*csproj /src/src/services/idp/
RUN dotnet restore -nowarn:msb3202,nu1503

COPY /src/services/idp/. /src/src/services/idp/
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.Idp.dll"]
