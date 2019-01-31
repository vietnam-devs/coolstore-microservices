#!/bin/bash
set -ex

mkdir -p proto
cd ./../../
echo `pwd`

cp -r ./grpc/third_party/googleapis/* ./services/catalog/proto/
cp -r ./grpc/v1/catalog.proto ./services/catalog/proto/
