#!/bin/sh
set -e
mongoimport --host localhost --db catalog --collection products --drop --file /app/data/products.json
mongoimport --host localhost --db rating --collection ratings --drop --file /app/data/ratings.json
