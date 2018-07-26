#! /bin/sh 
echo "127.0.0.1 $WEB_HOST_ALIAS" >> /etc/hosts
echo "$IDP_SERVICE_SERVICE_HOST $ID_HOST_ALIAS" >> /etc/hosts
echo "$GATEWAY_SERVICE_SERVICE_HOST $API_HOST_ALIAS" >> /etc/hosts

cd dist/
sed -i -e 's/${process.env.WEB_HOST_ALIAS}/'$WEB_HOST_ALIAS'/g' -e 's/${process.env.ID_HOST_ALIAS}/'$ID_HOST_ALIAS'/g' -e 's/${process.env.API_HOST_ALIAS}/'$API_HOST_ALIAS'/g' app.*

cd ..
yarn start