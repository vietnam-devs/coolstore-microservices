FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine AS base
RUN apk update && apk add libc6-compat
WORKDIR /app

ARG service_version
ENV ServiceVersion ${service_version:-0.0.1}

ARG api_version
ENV ApiVersion ${api_version:-1.0}

ENV ASPNETCORE_URLS http://+:5012
EXPOSE 5012

FROM microsoft/dotnet:2.2.100-sdk-alpine AS build
WORKDIR /src/src/services/open-api-v1

COPY /src/services/open-api-v1/*csproj /src/src/services/open-api-v1/

RUN dotnet restore -nowarn:msb3202,nu1503
COPY /src/services/open-api-v1/. /src/src/services/open-api-v1/

RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.OpenApiV1.dll"]
