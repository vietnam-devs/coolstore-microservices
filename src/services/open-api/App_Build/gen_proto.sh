#!/bin/bash
set -ex

readonly ROOT_DIR=`pwd`
readonly GOOGLEAPIS_DIR=${ROOT_DIR}/src/grpc/third_party/googleapis
readonly PROTOC_GEN_SWAGGER_DIR=${ROOT_DIR}/src/grpc/third_party/grpc-gateway
readonly SERVICE_DIR=${ROOT_DIR}/src/services/open-api

readonly GRPC_PATH=${HOME}/.nuget/packages/grpc.tools/1.17.1/tools/linux_x64
readonly PROTO_PATH=${ROOT_DIR}/src/grpc/v1
readonly PROTO_FILE=*.proto

readonly OUTPUT_PATH=${SERVICE_DIR}/wwwroot

cd `$SERVICE_DIR`

$GRPC_PATH/protoc -I$PROTO_PATH -I/usr/local/include -I$GOOGLEAPIS_DIR -I$PROTOC_GEN_SWAGGER_DIR \
    --include_imports --include_source_info \
    --swagger_out=logtostderr=true,allow_merge=true:$OUTPUT_PATH/ \
    $PROTO_PATH/$PROTO_FILE

cp $OUTPUT_PATH/apidocs.swagger.json $OUTPUT_PATH/apidocs.json
rm $OUTPUT_PATH/apidocs.swagger.json

cd -
