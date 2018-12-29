#!/bin/bash
set -ex

ROOT_DIR=`pwd`
SERVICE_DIR=${ROOT_DIR}/src/services/review

GRPC_PATH=${HOME}/.nuget/packages/grpc.tools/1.17.1/tools/linux_x64
PROTO_PATH=${ROOT_DIR}/src/grpc/v1
OUTPUT_PATH=${SERVICE_DIR}/v1/Grpc
PROTO_FILE=review.proto

cd `$SERVICE_DIR/App_Build`

$GRPC_PATH/protoc -I $PROTO_PATH -I /usr/local/include \
    --csharp_out $OUTPUT_PATH \
    --grpc_out $OUTPUT_PATH $PROTO_PATH/$PROTO_FILE \
    --plugin=protoc-gen-grpc=${GRPC_PATH}/grpc_csharp_plugin

cd -
