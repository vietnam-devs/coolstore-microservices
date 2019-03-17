# Up and Running with Docker and Docker Compose

## Docker

TODO

## Docker Compose

To make the development easy, we've usually used `docker-compose` to boost up all microservices and related components. We can make it run or debug the local microservices using this approach

Library and tool:

- Docker Compose
- Envoy Proxy
- Open Api
- Rest and gRPC protocols

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

Let says we want to debug `review-service` so we need to do some steps below

#### _Step 1_:

Open `docker-compose.yml`, find the section below, then comment or remove it

```yml
review-service:
  container_name: review-service
  image: 'vndg/cs-review-service'
  restart: always
  ports:
    - '5006:5006'
  expose:
    - '5006'
  build:
    context: .
    dockerfile: ./src/services/review/Dockerfile
```

#### _Step 2_:

Open `src/deploys/dockers/envoy-proxy/envoy.yaml` file, then change a bit as below

```yml
- name: review_grpc_service
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

Go to `http://localhost:8082/oai/swagger/index.html`, click to any `review-service` endpoints in there

Enjoy your hack!
