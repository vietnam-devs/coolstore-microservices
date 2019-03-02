#!/bin/bash
set -ex

readonly ROOT_DIR=`pwd`
readonly SERVICE_DIR=${ROOT_DIR}/src/services/rating
readonly PROTO_PATH=${ROOT_DIR}/src/grpc/v1
readonly HEALTH_PROTO_PATH=${ROOT_DIR}/src/grpc/health/v1

mkdir -p $SERVICE_DIR/proto
cp -r $PROTO_PATH/rating.proto $SERVICE_DIR/proto/
cp -r $HEALTH_PROTO_PATH/health.proto $SERVICE_DIR/proto/
