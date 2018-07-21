#!/bin/bash

helm template deploys/charts/coolstore \
  -f deploys/charts/coolstore/values.dev.yaml \
  > deploys/manifests/dev-all-in-one.yaml

istioctl kube-inject \
  -f deploys/manifests/dev-all-in-one.yaml | kubectl apply -f -
