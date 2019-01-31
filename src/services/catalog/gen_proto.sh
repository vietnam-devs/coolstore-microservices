#!/bin/bash
set -ex

mkdir -p proto
cd ./../../
echo `pwd`

cp -r ./grpc/v1/catalog.proto ./services/catalog/proto/
