FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine AS base
RUN apk update && apk add libc6-compat
WORKDIR /app

ARG service_version
ENV SERVICE_VERSION ${service_version:-0.0.1}

ENV ASPNETCORE_URLS http://+:5011
EXPOSE 5011

FROM microsoft/dotnet:2.2.100-sdk-alpine AS build
WORKDIR /src/src/services/graphql

COPY /src/services/graphql/*csproj /src/src/services/graphql/
RUN dotnet restore /property:Configuration=Release -nowarn:msb3202,nu1503

COPY /src/services/graphql/ /src/src/services/graphql/
COPY /src/gql/coolstore.graphql /src/src/services/graphql/v1/coolstore.graphql
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VND.CoolStore.Services.GraphQL.dll"]
