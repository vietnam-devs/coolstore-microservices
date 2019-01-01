#!/bin/bash
set -ex

readonly ROOT_DIR=`pwd`
readonly SERVICE_DIR=${ROOT_DIR}/src/services/review

readonly GRPC_PATH=${HOME}/.nuget/packages/grpc.tools/1.17.1/tools/linux_x64
readonly PROTO_PATH=${ROOT_DIR}/src/grpc/v1
readonly PROTO_FILE=review.proto
readonly API_FILE=review.yaml

readonly OUTPUT_PATH=${SERVICE_DIR}/v1/Grpc
readonly OUTPUT_PROXY_PATH=${ROOT_DIR}/src/services/grpc-proxy/v1

cd `$SERVICE_DIR/App_Build`

$GRPC_PATH/protoc -I $PROTO_PATH -I /usr/local/include \
    -I $GOPATH/src/github.com/grpc-ecosystem/grpc-gateway/third_party/googleapis \
    -I $GOPATH/src/github.com/grpc-ecosystem/grpc-gateway \
    --csharp_out $OUTPUT_PATH \
    --go_out=plugins=grpc:$OUTPUT_PROXY_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/$PROTO_FILE \
    --grpc-gateway_out=logtostderr=true,grpc_api_configuration=$PROTO_PATH/$API_FILE:$OUTPUT_PROXY_PATH \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

cd -
