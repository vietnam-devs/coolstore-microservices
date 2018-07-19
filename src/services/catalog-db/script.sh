#! /bin/sh
set -e
mongoimport --host localhost --db catalog --collection products --file products.json 