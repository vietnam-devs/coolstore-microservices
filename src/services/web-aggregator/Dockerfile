FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine AS base
RUN apk update && apk add libc6-compat
WORKDIR /app

ARG service_version
ENV SERVICE_VERSION ${service_version:-0.0.1}

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

FROM microsoft/dotnet:2.2.100-sdk-alpine AS build
WORKDIR /src/src/services/web-aggregator

COPY /src/services/web-aggregator/*csproj /src/src/services/web-aggregator/
RUN dotnet restore /property:Configuration=Release -nowarn:msb3202,nu1503

COPY /src/services/web-aggregator/. /src/src/services/web-aggregator/
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.WebAggregator.dll"]
