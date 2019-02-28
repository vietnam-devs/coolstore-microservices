FROM microsoft/dotnet:2.1.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /src/src/services/pricing

COPY /src/services/pricing/*csproj /src/src/services/pricing/
RUN dotnet restore -nowarn:msb3202,nu1503

COPY /src/services/pricing/. /src/src/services/pricing/
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Pricing.dll"]
