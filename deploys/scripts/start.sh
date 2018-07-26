#!/bin/bash
DOCKER_ENV=''
DOCKER_TAG=''

case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD

echo "Build mssqldb"
docker build -f ./deploys/dockers/mssqldb/Dockerfile -t $DOCKER_USERNAME/cs-mssqldb:$DOCKER_TAG .

echo "Build Gateway service"
docker build -f ./src/services/gateway/Dockerfile -t $DOCKER_USERNAME/cs-gateway-service:$DOCKER_TAG .

echo "Build IdP service"
docker build -f ./src/services/idp/Dockerfile -t $DOCKER_USERNAME/cs-idp-service:$DOCKER_TAG .

echo "Build Inventoty service"
docker build -f ./src/services/inventory/Dockerfile -t $DOCKER_USERNAME/cs-inventory-service:$DOCKER_TAG .

echo "Build Catalog service"
docker build -f ./src/services/catalog/Dockerfile -t $DOCKER_USERNAME/cs-catalog-service:$DOCKER_TAG .

echo "Build Cart service"
docker build -f ./src/services/cart/Dockerfile -t $DOCKER_USERNAME/cs-cart-service:$DOCKER_TAG .

echo "Build Web Application"
docker build -f ./src/web/Dockerfile -t $DOCKER_USERNAME/cs-spa:$DOCKER_TAG .

docker push $DOCKER_USERNAME/cs-mssqldb:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-gateway-service:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-idp-service:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-inventory-service:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-catalog-service:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-cart-service:$DOCKER_TAG
docker push $DOCKER_USERNAME/cs-spa:$DOCKER_TAG