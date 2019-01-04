#!/bin/bash
set -ex

readonly ROOT_DIR=`pwd`
readonly SERVICE_DIR=${ROOT_DIR}/src/services/inventory

cd `$SERVICE_DIR/App_Build`
cat inv_request.json | grpcurl -k call localhost:5004 inventory.InventoryService.GetInventory
cd -
