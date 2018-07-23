#!/bin/bash
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
echo "${NAMESPACE} and ${TAG}"
echo "Build gateway"
docker build -f src/services/gateway/Dockerfile -t vndg/cs-gateway-service:$TAG -t vndg/cs-gateway-service:latest .
echo "Build IDP"
docker build -f src/services/idp/Dockerfile -t vndg/cs-idp-service:$TAG -t vndg/cs-idp-service:latest .
echo "Build Inventoty"
docker build -f src/services/inventory/Dockerfile -t vndg/cs-inventory-service:$TAG -t vndg/cs-inventory-service:latest .
echo "Build Mysql-db"
docker build -f deploys/dockers/mssqldb/Dockerfile -t vndg/cs-mssql-db:$TAG -t vndg/cs-mssql-db:latest .
echo "Build Catalog"
docker build -f src/services/catalog/Dockerfile -t vndg/cs-catalog-service:$TAG -t vndg/cs-catalog-service:latest .
echo "Build Catalog-db"
docker build -f deploys/dockers/catalogdb/Dockerfile -t vndg/cs-catalog-db:$TAG -t vndg/cs-catalog-db:latest .
echo "Build cart"
docker build -f src/services/cart/Dockerfile -t vndg/cs-cart-service:$TAG -t vndg/cs-cart-service:latest .
echo "Build SPA"
docker build -f src/web/Dockerfile -t vndg/cs-spa:$TAG -t vndg/cs-spa:latest .
#docker rmi $(docker images -f "dangling=true" -q)