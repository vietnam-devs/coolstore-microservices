#!/bin/bash

kubectl create namespace coolstore

helm template deploys/charts/coolstore --namespace coolstore \
  -f deploys/charts/coolstore/values.dev.yaml \
  -f deploys/charts/coolstore/values.dev.local.yaml \
  > deploys/out/dev-all-in-one.yaml

istioctl kube-inject -f deploys/out/dev-all-in-one.yaml | kubectl apply -f -
