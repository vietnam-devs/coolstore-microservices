#!/bin/bash
set -ex

readonly ROOT_DIR=`pwd`
readonly GOOGLEAPIS_DIR=${ROOT_DIR}/src/grpc/third_party/googleapis
readonly PROTOC_GEN_SWAGGER_DIR=${ROOT_DIR}/src/grpc/third_party/grpc-gateway
readonly SERVICE_DIR=${ROOT_DIR}/src/services/graphql

readonly GRPC_PATH=${HOME}/.nuget/packages/grpc.tools/1.17.1/tools/linux_x64
readonly PROTO_PATH=${ROOT_DIR}/src/grpc/v1
readonly OUTPUT_PATH=${SERVICE_DIR}/v1/Grpc

cd `$SERVICE_DIR`

$GRPC_PATH/protoc -I$PROTO_PATH -I/usr/local/include -I$GOOGLEAPIS_DIR -I$PROTOC_GEN_SWAGGER_DIR \
    --csharp_out $OUTPUT_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/inventory.proto \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

$GRPC_PATH/protoc -I$PROTO_PATH -I/usr/local/include \
    --csharp_out $OUTPUT_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/cart.proto -I$GOOGLEAPIS_DIR -I$PROTOC_GEN_SWAGGER_DIR \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

$GRPC_PATH/protoc -I$PROTO_PATH -I/usr/local/include \
    --csharp_out $OUTPUT_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/catalog.proto -I$GOOGLEAPIS_DIR -I$PROTOC_GEN_SWAGGER_DIR \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

$GRPC_PATH/protoc -I$PROTO_PATH -I/usr/local/include \
    --csharp_out $OUTPUT_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/rating.proto -I$GOOGLEAPIS_DIR -I$PROTOC_GEN_SWAGGER_DIR \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

cd -
