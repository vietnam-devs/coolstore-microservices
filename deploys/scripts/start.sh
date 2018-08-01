#!/bin/bash
DOCKER_ENV=''
DOCKER_TAG=''
DOCKER_GROUP=''

case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_ENV=production
    DOCKER_TAG=latest
    DOCKER_GROUP=vndg
    ;;
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD

echo "Build MS-SQL Database"
echo "================================================================================"
docker build -f ./deploys/dockers/mssqldb/Dockerfile -t $DOCKER_USERNAME/cs-mssqldb:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-mssqldb:$DOCKER_TAG $DOCKER_GROUP/cs-mssqldb:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-mssqldb:$DOCKER_TAG
echo "================================================================================"

echo "Build Gateway Service"
echo "================================================================================"
docker build -f ./src/services/gateway/Dockerfile -t $DOCKER_USERNAME/cs-gateway-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-gateway-service:$DOCKER_TAG $DOCKER_GROUP/cs-gateway-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-gateway-service:$DOCKER_TAG
echo "================================================================================"

echo "Build IdP Service"
echo "================================================================================"
docker build -f ./src/services/idp/Dockerfile -t $DOCKER_USERNAME/cs-idp-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-idp-service:$DOCKER_TAG $DOCKER_GROUP/cs-idp-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-idp-service:$DOCKER_TAG
echo "================================================================================"

echo "Build Inventoty Service"
echo "================================================================================"
docker build -f ./src/services/inventory/Dockerfile -t $DOCKER_USERNAME/cs-inventory-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-inventory-service:$DOCKER_TAG $DOCKER_GROUP/cs-inventory-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-inventory-service:$DOCKER_TAG
echo "================================================================================"

echo "Build Catalog Service"
echo "================================================================================"
docker build -f ./src/services/catalog/Dockerfile -t $DOCKER_USERNAME/cs-catalog-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-catalog-service:$DOCKER_TAG $DOCKER_GROUP/cs-catalog-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-catalog-service:$DOCKER_TAG
echo "================================================================================"

echo "Build Rating Service"
echo "================================================================================"
docker build -f ./src/services/rating/Dockerfile -t $DOCKER_USERNAME/cs-rating-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-rating-service:$DOCKER_TAG $DOCKER_GROUP/cs-rating-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-rating-service:$DOCKER_TAG
echo "================================================================================"

echo "Build Cart Service"
echo "================================================================================"
docker build -f ./src/services/cart/Dockerfile -t $DOCKER_USERNAME/cs-cart-service:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-cart-service:$DOCKER_TAG $DOCKER_GROUP/cs-cart-service:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-cart-service:$DOCKER_TAG
echo "================================================================================"

echo "Build Web Application"
echo "================================================================================"
docker build -f ./src/web/Dockerfile -t $DOCKER_USERNAME/cs-spa:$DOCKER_TAG .
docker tag $DOCKER_USERNAME/cs-spa:$DOCKER_TAG $DOCKER_GROUP/cs-spa:$DOCKER_TAG
docker push $DOCKER_GROUP/cs-spa:$DOCKER_TAG
echo "================================================================================"