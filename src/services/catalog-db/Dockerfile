FROM bitnami/mongodb:latest

WORKDIR .

COPY ./src/services/catalog-db/products.json /products.json
COPY ./src/services/catalog-db/script.sh /docker-entrypoint-initdb.d/

RUN chmod +x /docker-entrypoint-initdb.d/script.sh
