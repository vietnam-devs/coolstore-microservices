#!/bin/bash

cd ../charts/
helm template cart --name dev > ../k8s/dev.cart.yaml

echo "finished!"