TAG=$(git rev-parse --short HEAD)
NAMESPACE=VNDG

docker build -f src/services/gateway/Dockerfile \
  -t $NAMESPACE/cs-gateway-service:$TAG \
  -t $NAMESPACE/cs-gateway-service:latest .

docker build -f src/services/idp/Dockerfile \
  -t $NAMESPACE/cs-idp-service:$TAG \
  -t $NAMESPACE/cs-idp-service:latest .

docker build -f src/services/inventory/Dockerfile \
  -t $NAMESPACE/cs-inventory-service:$TAG \
  -t $NAMESPACE/cs-inventory-service:latest .

docker build -f src/services/catalog/Dockerfile \
  -t $NAMESPACE/cs-catalog-service:$TAG \
  -t $NAMESPACE/cs-catalog-service:latest .

docker build -f src/web/Dockerfile \
  -t $NAMESPACE/cs-spa:$TAG \
  -t $NAMESPACE/cs-spa:latest .

docker rmi $(docker images -f "dangling=true" -q)
