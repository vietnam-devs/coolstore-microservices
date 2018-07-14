# Cool Store Application on Service Mesh Demo

> This project is inspired from [coolstore project](https://github.com/jbossdemocentral/coolstore-microservice) by [jbossdemocentral](https://github.com/jbossdemocentral) & [Red Hat Demo Central](https://gitlab.com/redhatdemocentral)

CoolStore is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Service Mesh.

It demonstrates how to wire up small microservices into a larger application using microservice architectural principals.

# Services

There are several individual microservices and infrastructure components that make up this app:

1. Catalog Service
  - NodeJS service and MongoDB, serves products and prices for retail products
  - **`http://localhost:5002`**
2. Cart Service
  - .NET Core service which manages shopping cart for each customer
  - **`http://localhost:5003`**
3. Inventory Service
  - .NET Core service and SQL Server, serves inventory and availability data for retail products
  - **`http://localhost:5004`**
4. Pricing Service 
  - .NET Core service which is a business rules application for product pricing
  - **`http://localhost:5005`**
5. Review Service
  - .NET Core service and SQL Server running for writing and displaying reviews for products
  - **`http://localhost:5006`**
6. Rating Service
  - NodeJS service running for rating products
  - `http://localhost:5007`
7. Coolstore Gateway
  - [ingress-nginx](https://github.com/kubernetes/ingress-nginx) service running that serving as an API gateway to the backend services
  - **`http://localhost:5000`**
8. IDP
  - Identity Provider using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) to authentication with OAuth 2.0 and OpenID Connect for the whole stack
  - **`http://localhost:5001`**
9. Web UI (PWA)
  - A frontend based on [vuejs](https://vuejs.org/) and [Node.js](https://nodejs.org)
  - **`http://localhost:8080`**

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

# Prerequisites

- Windows 10
- Windows Subsystem Linux (WSL - Ubuntu OS)
- Docker for Windows (Kubernetes enabled)
- kubectl
- helm
- istioctl

# Up and Running

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

Add hosts file with following content

```
127.0.0.1   id.coolstore.local
127.0.0.1   coolstore.local
```

Waiting for the container provision completed

```
> curl -I http://coolstore.local
> curl -I http://coolstore.local/cs
```

# Open API 

![OpenAPI Screenshot](assets/images/open-api.png?raw=true 'OpenAPI')

# Deploy Demo: CoolStore Microservices with CI/CD

TODO
