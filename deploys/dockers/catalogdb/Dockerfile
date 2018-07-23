FROM bitnami/mongodb:latest

WORKDIR .

COPY ./deploys/dockers/catalogdb/products.json /products.json
COPY ./deploys/dockers/catalogdb/script.sh /docker-entrypoint-initdb.d/

EXPOSE 27017

RUN chmod +x /docker-entrypoint-initdb.d/script.sh
