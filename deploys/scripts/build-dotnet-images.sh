#!/bin/bash
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
echo "${NAMESPACE} and ${TAG}"

echo "Build IdP..."
docker build -f src/services/idp/Dockerfile -t vndg/cs-idp-service:$TAG -t vndg/cs-idp-service:latest .

echo "Build Inventory..."
docker build -f src/services/inventory/Dockerfile -t vndg/cs-inventory-service:$TAG -t vndg/cs-inventory-service:latest .

echo "Build Cart..."
docker build -f src/services/cart/Dockerfile -t vndg/cs-cart-service:$TAG -t vndg/cs-cart-service:latest .

#docker rmi $(docker images -f "dangling=true" -q)
