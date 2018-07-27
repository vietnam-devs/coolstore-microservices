#! /bin/sh 
echo "127.0.0.1 $WEB_HOST_ALIAS" >> /etc/hosts
echo "$IDP_SERVICE_SERVICE_HOST $ID_HOST_ALIAS" >> /etc/hosts
echo "$GATEWAY_SERVICE_SERVICE_HOST $API_HOST_ALIAS" >> /etc/hosts

if [[ -z "$WEB_HOST_ALIAS" ]]; then
   
    sed -i -e 's/coolstore.local/'$WEB_HOST_ALIAS'/g' -e 's/id.coolstore.local/'$ID_HOST_ALIAS'/g' -e 's/api.coolstore.local/'$API_HOST_ALIAS'/g'  proxy.config.js
    cd dist/
    sed -i -e 's/coolstore.local/'$WEB_HOST_ALIAS'/g' -e 's/id.coolstore.local/'$ID_HOST_ALIAS'/g' -e 's/api.coolstore.local/'$API_HOST_ALIAS'/g' app.*
	cd ..
   
fi


yarn start