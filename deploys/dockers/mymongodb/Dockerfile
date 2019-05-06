FROM mongo
RUN mkdir -p /app/data/
COPY ./deploys/dockers/mymongodb/products.json /app/data/
COPY ./deploys/dockers/mymongodb/ratings.json /app/data/
COPY ./deploys/dockers/mymongodb/script.sh /docker-entrypoint-initdb.d/
RUN chmod +x /docker-entrypoint-initdb.d/script.sh
