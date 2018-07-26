#!/bin/bash
docker rmi $(docker images -f "dangling=true" -q)
docker rmi $(docker images --format '{{.Repository}}:{{.Tag}}' | grep 'vndg')