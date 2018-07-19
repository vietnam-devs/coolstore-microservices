#! /bin/sh 
echo "127.0.0.1 coolstore.local" >> /etc/hosts
echo "$IDP_SERVICE_SERVICE_HOST id.coolstore.local" >> /etc/hosts
echo "$GATEWAY_SERVICE_SERVICE_HOST api.coolstore.local" >> /etc/hosts
yarn start