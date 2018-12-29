#!/bin/sh
set -ex

ROOT_DIR=`pwd`
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
SERVICE_PATH=${ROOT_DIR}/src/services/review
SERVICE_NAME=review-service

echo "Namespace is ${NAMESPACE} and tag is ${TAG}"
echo "Start to build ${SERVICE_NAME}..."

docker build -f $SERVICE_PATH/Dockerfile \
    -t $NAMESPACE/$SERVICE_NAME:$TAG \
    -t $NAMESPACE/$SERVICE_NAME:latest .
