# Cool Store: Cloud-Native Microservices Application on Service Mesh

<p align="left">
  <a href="https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE"><img src="https://img.shields.io/badge/price-FREE-0098f7.svg" alt="Price"></a>
  <a href="https://travis-ci.org/vietnam-devs/coolstore-microservices"><img src="https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?label=travis-ci&branch=master&style=flat-square" alt="Build Status" data-canonical-src="https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?label=travis-ci&branch=master" style="max-width:100%;"></a>
</p>

> This project is inspired from [CoolStore project](https://github.com/jbossdemocentral/coolstore-microservice) by [JBoss Demo Central](https://github.com/jbossdemocentral) & [Red Hat Demo Central](https://gitlab.com/redhatdemocentral)

CoolStore is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Service Mesh.

It demonstrates how to wire up small microservices into a larger application using microservice architectural principals.

### Presentation
Our team uses this application to demonstrate Kubernetes, AKS, Istio and similar cloud-native technologies in events as following

- [From Microservices to Service Mesh - DevCafe Event - July 2018](https://www.slideshare.net/ThangChung/from-microservices-to-service-mesh-devcafe-event-july-2018)
- [Service Mesh for Microservices- Vietnam Mobile Day Event - June 2018](https://www.slideshare.net/ThangChung/service-mesh-for-microservices-vietnam-mobile-day-june-2017)

# Table of contents

* [Prerequisites](https://github.com/vietnam-devs/coolstore-microservices#prerequisites)
* [Installation](https://github.com/vietnam-devs/coolstore-microservices#installation)
* [µService Development](https://github.com/vietnam-devs/coolstore-microservices#µmicroservice-development)
* [Open API](https://github.com/vietnam-devs/coolstore-microservices#open-api)
* [CI/CD](https://github.com/vietnam-devs/coolstore-microservices#ci-cd)
* [Contributing](https://github.com/vietnam-devs/coolstore-microservices#contributing)
* [Contributors](https://github.com/vietnam-devs/coolstore-microservices#contributors)
* [Licence](https://github.com/vietnam-devs/coolstore-microservices#licence)

## Prerequisites

- Windows 10
- Windows Subsystem Linux (WSL - Ubuntu OS)
- Docker for Desktop (Kubernetes enabled)
- kubectl
- helm
- istioctl

## µServices

There are several individual µservices and infrastructure components that make up this app:

| No. | Service | Description | Language | Database | Endpoints |
|-----|---------|-------------|----------|----------|-----------|
| 1 | Catalog | Serves products and prices for retail products | Node.js | Mongo | [`http://localhost:5002`](http://localhost:5002) or [`http://api.coolstore.local/catalog`](http://api.coolstore.local/catalog/swagger)
| 2 | Cart | Manages shopping cart for each customer | .NET Core | MySQL | [`http://localhost:5003`](http://localhost:5003) or [`http://api.coolstore.local/cart`](http://api.coolstore.local/cart/swagger)
| 3 | Inventory | Serves inventory and availability data for retail products | .NET Core | MySQL | [`http://localhost:5004`](http://localhost:5004) or [`http://api.coolstore.local/inventory`](http://api.coolstore.local/inventory/swagger)
| 4 | Pricing | Handles a business rules application for product pricing | .NET Core | MySQL | [`http://localhost:5005`](http://localhost:5005) or [`http://api.coolstore.local/pricing`](http://api.coolstore.local/pricing/swagger)
| 5 | Review | Runs for writing and displaying reviews for products | .NET Core | MySQL | [`http://localhost:5006`](http://localhost:5006) or [`http://api.coolstore.local/review`](http://api.coolstore.local/review/swagger)
| 6 | Rating | Runs for rating products | Node.js | Mongo | [`http://localhost:5007`](http://localhost:5007) or [`http://api.coolstore.local/rating`](http://api.coolstore.local/rating/swagger)
| 7 | IdP | Uses [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) to authentication with OAuth 2.0 and OpenID Connect for the whole stack | .NET Core | In Memory | [`http://localhost:5001`](http://localhost:5001) or [`http://id.coolstore.local`](http://id.coolstore.local)
| 8 | Web UI (PWA) | Frontend based on [vuejs](https://vuejs.org/) and [Node.js](https://nodejs.org) | Vuejs + Node.js | N/A | [`http://localhost:8080`](http://localhost:8080) or [`http://coolstore.local`](http://coolstore.local)

### Architecture of µServices

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

## Features
- **[Kubernetes](https://kubernetes.io)/[AKS](https://docs.microsoft.com/en-us/azure/aks):**
  The app is designed to run on Kubernetes (both locally on "Docker for
  Desktop", as well as on the cloud with AKS).
- **[Istio](https://istio.io):** Application works on Istio service mesh.
- **[NetCoreKit](https://github.com/cloudnative-netcore/netcore-kit):** Set of Cloud Native tools and utilities for .NET Core.

## Installation

### Option 1: Up and Running locally with "Docker for Desktop", development only

1. Make sure we have **`Docker for Desktop`** running with **`Kubernetes`** option enabled. We need to install **`kubectl`**, **`helm`** and **`istioctl`** on the build machine as well.

2. From current console, type `bash` to enter `Linux Subsystem (Ubuntu)`

3. Then `cd` into your root of project

```
> ./deploys/cs-build.sh
```

It should run and package all docker images.

4. Download and install [istio-1.0.0](https://github.com/istio/istio/releases/tag/1.0.0) on the box, and unzip it into somewhere, then initialize it with following commands

```
> cd <istio-1.0.0 path>
> kubectl create -f install/kubernetes/helm/helm-service-account.yaml
> helm init --service-account tiller --upgrade
```

5. Get `istio-ingressgateway` IP address

```
> kubectl get services istio-ingressgateway -n istio-system -o=jsonpath={.spec.clusterIP}
> 10.96.34.68 <== example IP
```

6. Create `values.dev.local.yaml` file in `deploys/charts/coolstore`, and put content like

```
gateway:
  ip: 10.96.34.68
```

7. Apply `istioctl` command to `coolstore` chart

```
> helm template deploys/charts/coolstore -f deploys/charts/coolstore/values.dev.yaml -f deploys/charts/coolstore/values.dev.local.yaml > deploys/k8s/dev-all-in-one.yaml
> istioctl kube-inject -f deploys/k8s/dev-all-in-one.yaml | kubectl apply -f -
```

8. Add hosts file with following content

```
127.0.0.1   api.coolstore.local
127.0.0.1   id.coolstore.local
127.0.0.1   coolstore.local
```

Waiting for the container provision completed

```
> curl -I http://coolstore.local # website
> curl -I http://api.coolstore.local # api gateway
> curl -I http://id.coolstore.local # identity provider
```

9. Clean up `coolstore` chart as

```
> kubectl delete -f deployment/istio/dev-all-in-one.yaml
> helm delete istio --purge
```

**Notes**:

1. Global path
> Set `PATH` for `docker`, `kubectl`, `helm`, and `istioctl`.

2. Run with Nginx (not recommendation)
> If you want to run just only `Kubernetes` + `nginx-ingress` go to `deploys/charts/coolstore/values.yaml`, and modify as following
>```
> nginx:
>    enabled: true
>```
> Then run the `helm` command as
> ```
> helm install --name cs-nginx stable/nginx-ingress
> ```

### Option 2: Up and Running on Azure Kubernetes Service (AKS)

TODO

## µService Development

![µService Screenshot](assets/images/miniservice-development.PNG?raw=true 'Microservice')

## Open API

![OpenAPI Screenshot](assets/images/open-api.png?raw=true 'OpenAPI')

## CI/CD

![Lift and Shift](assets/images/lift-and-shift.PNG?raw=true 'liftandshift')

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :p

## Contributors

| [Thang Chung](https://github.com/thangchung)                                                         | [Thinh Nguyen](https://github.com/thinhnotes)                                                          |
| ---------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| <img src="https://avatars3.githubusercontent.com/u/422341?s=460&v=4"  alt="thangchung" width="150"/> | <img src="https://avatars2.githubusercontent.com/u/4660531?s=460&v=4" alt="thinhnguyen" width="150" /> |

## Licence

Code released under [the MIT license](https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE).
