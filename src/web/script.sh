#! /bin/sh 
echo "127.0.0.1 $WEB_HOST_ALIAS" >> /etc/hosts
echo "$IDP_SERVICE_SERVICE_HOST $ID_HOST_ALIAS" >> /etc/hosts
echo "$GATEWAY_SERVICE_SERVICE_HOST $API_HOST_ALIAS" >> /etc/hosts

yarn start