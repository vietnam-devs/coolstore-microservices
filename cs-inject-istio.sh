#!/bin/bash

helm template deployment/charts/coolstore \
  -f deployment/charts/coolstore/values.dev.yaml \
  > deployment/manifests/dev-all-in-one.yaml

istioctl kube-inject \
  -f deployment/manifests/dev-all-in-one.yaml | kubectl apply -f -
