#!/bin/bash
docker rmi -f $(docker images -f "dangling=true" -q)
docker rmi -f $(docker images --format '{{.Repository}}:{{.Tag}}' | grep 'vndg')