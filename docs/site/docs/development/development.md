# Introduction

`coolstore-microservices` uses `gRPC` and `Rest` for all microservices. To get starting, we need to prepare and install toolkit and library for it.

# gRPC

Before you can generate `gRPC` files for all microservices in this project you need to install some of tool as below

## Install `protoc-gen-swagger`

This will help to generate `Open Api` file (former is `Swagger`), and used in `./src/services/open-api` service

More information can be found at https://github.com/grpc-ecosystem/grpc-gateway

```bash
go get -u github.com/grpc-ecosystem/grpc-gateway/protoc-gen-swagger
```

At the root of each microservice, we put one `bash` script with named `cmd_gen_proto.sh` so that you can generate standalone `gRPC` files for each service, and if you want to generate `gRPC` files for all of them, then you can access and run at `./deploys/scripts/gen-protos.sh`
