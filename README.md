# Cool Store Microservice Demo

CoolStore is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Kubernetes

It demonstrates how to wire up small microservices into a larger application using microservice architectural principals.

# Services

There are several individual microservices and infrastructure components that make up this app:

1.  Catalog Service - .NET Core service and MongoDB, serves products and prices for retail products
2.  Cart Service - .NET Core service which manages shopping cart for each customer
3.  Inventory Service - .NET Core service and SQL Server, serves inventory and availability data for retail products
4.  Pricing Service - .NET Core service which is a business rules application for product pricing
5.  Review Service - .NET Core service and SQL Server running for writing and displaying reviews for products
6.  Rating Service - .NET Core service running for rating products
7.  Coolstore Gateway - [ingress-nginx](https://github.com/kubernetes/ingress-nginx) service running that serving as an API gateway to the backend services
8.  IDP - Identity Provider using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) to authentication with OAuth 2.0 and OpenID Connect for the whole stack
9.  Web UI - A frontend based on [vuejs](https://vuejs.org/) and [Node.js](https://nodejs.org)

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

# Prerequisites

- Ubuntu Server
- minikube
- kubectl
- VirtualBox
- helm

# Deploy CoolStore Microservices Application

Make sure we have `docker for windows` edge running with `kubernetes` option enabled. We need to install `kubectl`, `helm` and `istioctl` on the build machine as well.

From current console, type `bash` to enter `Linux Subsystem (Ubuntu)`

Download `istio 0.8.0`, and unzip it into somewhere

```
> cd <istio 0.8.0> path
> kubectl create -f install/kubernetes/helm/helm-service-account.yaml
> helm init --service-account tiller --upgrade
> helm install install/kubernetes/helm/istio --name istio --namespace istio-system --timeout 1000
```

Then `cd` into your root of project

```
> ./cs-build.sh
> ./cs-inject-istio.sh
```

Waiting for the container provision completed

```
> curl http://localhost:3200
```

# Deploy Demo: CoolStore Microservices with CI/CD

TODO
