#! /bin/sh 
set -e
mongoimport --host localhost --db catalog --collection Product --drop --file /products.json