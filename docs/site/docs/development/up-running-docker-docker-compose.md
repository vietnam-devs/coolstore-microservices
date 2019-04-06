# Up and Running with Docker and Docker Compose

# Environment variables

To developing in the localhost, we need to add `.env` file and put it to the root of `coolstore-microservices` project. The content of it as below

```
WEB_PORT=8084
BACKOFFICE_PORT=3000

OPENAPI_SVR_PORT=5010
GRAPHQL_SVR_PORT=5011
IDP_SVR_PORT=8085

CATALOG_SVC_PORT=5002
CART_SVC_PORT=5003
INVENTORY_SVC_PORT=5004
RATING_SVC_PORT=5007

MONGODB_PORT=27017
MYSQLDB_PORT=3306

HOST_IP=<localhost ip get from ipconfig>
```

## Protobuf

In each microservices, we also have `cmd_gen_proto.sh` to use `protobuf` tools which generates `C#` target file for .NET microservice project.

## Docker

In each microservices, we also have `cmd_build_image.sh` to build the standalone service and tag it with `vndg` prefix.

## Docker Compose

To make the development easy, we've usually used `docker-compose` to boost up all microservices and related components. We can make it run or debug the local microservices using this approach

Library and tool:

- Docker Compose
- Envoy Proxy
- Open Api
- Rest and gRPC protocols

We support 4 modes of docker-compose at the moment:

- `docker-compose.yml`: full running with all services, server and web endpoints
- `docker-compose-graphql.yml`: only graphql endpoints with its backoffice app
- `docker-compose-graphql.headless.yml`: only headless graphql endpoints
- `docker-compose-graphql.dev.yml`: only microservices

### List of endpoints

#### Envoy Proxy Endpoints

- Main Uri: http://localhost:8082
- Admin Uri: http://localhost:8081

#### Web UI Endpoint

- http://localhost:8084

#### Identity Server Endpoint

- http://localhost:8085

#### Open Api Endpoint

- http://localhost:8082/oai/

#### GraphQL Endpoint

- http://localhost:8082/gql/graphiql
- http://localhost:8082/gql/playground
- http://localhost:8082/gql/voyager

### Debugging

Let says we want to debug `cart-service` so we need to do some steps below

#### _Step 1_:

Open `docker-compose.yml`, find the section below, then comment or remove it

```yml
cart-service:
  container_name: cart-service
  image: 'vndg/cs-cart-service'
  restart: always
  environment:
    - Features__EfCore__MySqlDb__FQDN=mysqldb:3306
  ports:
    - '5003:5003'
  expose:
    - '5003'
  build:
    context: .
    dockerfile: ./src/services/cart/Dockerfile
```

#### _Step 2_:

Open `src/deploys/dockers/envoy-proxy/envoy.yaml` file, then change a bit as below

```yml
- name: cart_grpc_service
  connect_timeout: 0.25s
  type: static
  lb_policy: round_robin
  http2_protocol_options: {}
  hosts:
    - socket_address:
        address: 192.168.137.1
        port_value: 5006
```

`type` should change to `static`, and `address` should be your real IP of the laptop you run

Then, on the command prompt type `bash`, and `./deploys/dockers/envoy-proxy/cmd_build_image.sh`

#### _Step 3_:

Run `docker-compose up`

#### _Step 4_:

Run your gRPC service in debug mode

#### _Step 5_:

Go to `http://localhost:8082/oai/swagger/index.html`, click to any `cart-service` endpoints in there

Enjoy your hack!
