FROM mongo:latest

COPY products.json /products.json
CMD sleep 10 && mongoimport --host catalog-db --db catalog --collection products --type json --file /products.json && echo "MongoDB Import Completed"
