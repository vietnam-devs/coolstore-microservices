#!/bin/bash
kubectl apply -f deploys/helm/helm-service-account.yaml
helm init --service-account tiller --upgrade
