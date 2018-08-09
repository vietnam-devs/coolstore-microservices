#!/bin/sh

set -euo pipefail

sleep 60

if [ -n "$WEB_HOST_ALIAS" ] && [ -n "$GATEWAY_IP" ]
then
	echo "127.0.0.1 $WEB_HOST_ALIAS" >> /etc/hosts
	echo "$GATEWAY_IP $ID_HOST_ALIAS" >> /etc/hosts
	echo "$GATEWAY_IP $API_HOST_ALIAS" >> /etc/hosts
	
	sed -i -e 's/coolstore.local/'$WEB_HOST_ALIAS'/g' -e 's/id.coolstore.local/'$ID_HOST_ALIAS'/g' -e 's/api.coolstore.local/'$API_HOST_ALIAS'/g'  proxy.config.js
	cd dist/
	sed -i -e 's/coolstore.local/'$WEB_HOST_ALIAS'/g' -e 's/id.coolstore.local/'$ID_HOST_ALIAS'/g' -e 's/api.coolstore.local/'$API_HOST_ALIAS'/g' app.*
	cd ..
fi

yarn start