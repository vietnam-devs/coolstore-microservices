#!/bin/bash
echo "inject environment variables to files"
envsubst < "index.html" > index.compiled.html

echo "start nginx application"
cd ..
nginx -g "daemon off;"