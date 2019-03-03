#!/bin/bash
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
echo "${NAMESPACE} and ${TAG}"

echo "Build MySQL..."
docker build -f deploys/dockers/mysqldb/Dockerfile -t vndg/cs-mysqldb:$TAG -t vndg/cs-mysqldb:latest .

echo "Build IdP..."
./src/services/idp/cmd_build_image.sh

echo "Build Inventory..."
./src/services/inventory/cmd_build_image.sh

echo "Build Catalog..."
./src/services/catalog/cmd_build_image.sh

echo "Build Rating..."
./src/services/rating/cmd_build_image.sh

echo "Build Cart..."
./src/services/cart/cmd_build_image.sh

echo "Build Review..."
./src/services/review/cmd_build_image.sh

echo "Build SPA..."
./src/web/cmd_build_image.sh

#docker rmi $(docker images -f "dangling=true" -q)
