#!/bin/bash
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
echo "${NAMESPACE}\cs-gateway-service:${TAG}"
#docker build -f src/services/gateway/Dockerfile -t vndg/cs-gateway-service:$TAG -t vndg/cs-gateway-service:latest .
#docker build -f src/services/idp/Dockerfile -t vndg/cs-idp-service:$TAG -t vndg/cs-idp-service:latest .
#docker build -f src/services/inventory/Dockerfile -t vndg/cs-inventory-service:$TAG -t vndg/cs-inventory-service:latest .
docker build -f src/services/catalog/Dockerfile -t vndg/cs-catalog-service:$TAG -t vndg/cs-catalog-service:latest .
#docker build -f src/web/Dockerfile -t vndg/cs-spa:$TAG -t vndg/cs-spa:latest .
#docker rmi $(docker images -f "dangling=true" -q)
