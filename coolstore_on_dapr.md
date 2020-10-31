# Sdks & Toolings

> [dotnet](dot.net) --version 5.0.100-rc.2.20479.15

> [dapr](https://github.com/dapr/dapr) --version CLI version: 0.11.0 Runtime version: 0.11.3

> [osm](https://github.com/openservicemesh/osm) version Version: v0.3.0; Commit: c91c782; Date: 2020-08-12-21:49

> [tye](https://github.com/dotnet/tye) --version 0.5.0-alpha.20520.1+739de2eeb94f0246f50fa4993e953abb4b0da18a


# Dapr CLI for locally development

## Init core component with docker-compose

```
$ docker-compose -f docker-compose.yml -f docker-compose.override.yml down --remove-orphans -v
$ docker-compose -f docker-compose.yml -f docker-compose.override.yml up postgresql zipkin redis elasticsearch
```

## Run Dapr apps

```
$ dapr run --app-port 25002 --app-id identityapp --dapr-http-port 5001 dotnet run -- -p src\Services\Identity\IdentityService\IdentityService.csproj
```

```
$ dapr run --app-port 5003 --app-id productcatalogapp dotnet run -- -p src\Services\ProductCatalog\ProductCatalogService.Api\ProductCatalogService.Api.csproj
```

```
$ dapr run --app-port 5002 --app-id inventoryapp dotnet run -- -p src\Services\Inventory\InventoryService.Api\InventoryService.Api.csproj
```