FROM microsoft/dotnet:2.1.2-aspnetcore-runtime-alpine AS base
WORKDIR /app

ARG service_version
ENV SERVICE_VERSION ${service_version:-0.0.1}

ARG api_version
ENV API_VERSION ${api_version:-1.0}

ENV ASPNETCORE_URLS http://+:5006
EXPOSE 5006

FROM microsoft/dotnet:2.1.302-sdk-alpine AS build
WORKDIR /src
COPY . .

WORKDIR /src/src/services/review

RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.Review.dll"]
