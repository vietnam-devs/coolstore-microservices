#! /bin/sh 
until nc -z mongodb 27017
do
    echo "Waiting to connect db"
    sleep 1
done

echo "Can be to connect db"
yarn start