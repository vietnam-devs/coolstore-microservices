#! /bin/sh 

until nc -z mongodb 27017
do
    echo "Waiting to connect to a database..."
    sleep 1
done

echo "Connected to the database."

#sed -i -e 's/"basePath": "/"/'"basePath": "$BASE_PATH"'/g' tsoa.json

yarn start