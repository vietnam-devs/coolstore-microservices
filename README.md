# Cool Store Application on Service Mesh

<p align="left">
  <a href="https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE"><img src="https://img.shields.io/badge/price-FREE-0098f7.svg" alt="Price"></a>
  <a href="https://codecov.io/gh/vietnam-devs/coolstore-microservices"><img src="https://codecov.io/gh/vietnam-devs/coolstore-microservices/branch/master/graph/badge.svg" /></a>
  <a href="https://travis-ci.org/vietnam-devs/coolstore-microservices"><img src="https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?label=travis-ci&branch=master&style=flat-square" alt="Build Status" data-canonical-src="https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?label=travis-ci&branch=master" style="max-width:100%;"></a>
</p>

> This project is inspired from [coolstore project](https://github.com/jbossdemocentral/coolstore-microservice) by [jbossdemocentral](https://github.com/jbossdemocentral) & [Red Hat Demo Central](https://gitlab.com/redhatdemocentral)

CoolStore is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Service Mesh.

It demonstrates how to wire up small microservices into a larger application using microservice architectural principals.

### Slides
- [From Microservices to Service Mesh - DevCafe Event - July 2018](https://www.slideshare.net/ThangChung/from-microservices-to-service-mesh-devcafe-event-july-2018)
- [Service Mesh for Microservices- Vietnam Mobile Day Event - June 2018](https://www.slideshare.net/ThangChung/service-mesh-for-microservices-vietnam-mobile-day-june-2017)

# Table of contents

* [Prerequisites](https://github.com/vietnam-devs/coolstore-microservices#prerequisites)
* [Services](https://github.com/vietnam-devs/coolstore-microservices#services)
* [Up and Running](https://github.com/vietnam-devs/coolstore-microservices#up-and-running)
* [Open API](https://github.com/vietnam-devs/coolstore-microservices#open-api)
* [CI/CD](https://github.com/vietnam-devs/coolstore-microservices#ci-cd)
* [Contributing](https://github.com/vietnam-devs/coolstore-microservices#contributing)
* [Contributors](https://github.com/vietnam-devs/coolstore-microservices#contributors)
* [Licence](https://github.com/vietnam-devs/coolstore-microservices#licence)

### Prerequisites

- Windows 10
- Windows Subsystem Linux (WSL - Ubuntu OS)
- Docker for Windows (Kubernetes enabled)
- kubectl
- helm
- istioctl

### Services

There are several individual microservices and infrastructure components that make up this app:

1. Catalog Service: NodeJS service and MongoDB, serves products and prices for retail products
  - **[http://localhost:5002](http://localhost:5002)**
  - **[http://api.coolstore.local/catalog](http://api.coolstore.local/catalog)**
2. Cart Service: .NET Core service which manages shopping cart for each customer
  - **[http://localhost:5003](http://localhost:5003)**
  - **[http://api.coolstore.local/cart](http://api.coolstore.local/cart)**
3. Inventory Service: .NET Core service and SQL Server, serves inventory and availability data for retail products
  - **[http://localhost:5004](http://localhost:5004)**
  - **[http://api.coolstore.local/inventory](http://api.coolstore.local/inventory)**
4. Pricing Service: .NET Core service which is a business rules application for product pricing
  - **[http://localhost:5005](http://localhost:5005)**
  - **[http://api.coolstore.local/pricing](http://api.coolstore.local/pricing)**
5. Review Service: .NET Core service and SQL Server running for writing and displaying reviews for products
  - **[http://localhost:5006](http://localhost:5006)**
  - **[http://api.coolstore.local/review](http://api.coolstore.local/review)**
6. Rating Service: NodeJS service running for rating products
  - **[http://localhost:5007](http://localhost:5007)**
  - **[http://api.coolstore.local/rating](http://api.coolstore.local/rating)**
7. IdP: Identity Provider using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) to authentication with OAuth 2.0 and OpenID Connect for the whole stack
  - **[http://localhost:5001](http://localhost:5001)**
  - **[http://id.coolstore.local](http://id.coolstore.local)**
8. Web UI (PWA): A frontend based on [vuejs](https://vuejs.org/) and [Node.js](https://nodejs.org)
  - **[http://localhost:8080](http://localhost:8080)**
  - **[http://coolstore.local](http://coolstore.local)**

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

### Up and Running

1. Make sure we have `docker for windows` edge running with `kubernetes` option enabled. We need to install `kubectl`, `helm` and `istioctl` on the build machine as well.

2. From current console, type `bash` to enter `Linux Subsystem (Ubuntu)`

3. Download `istio istio-1.0.0-snapshot.2`, and unzip it into somewhere

```
> cd <istio-1.0.0-snapshot.2 path>
> kubectl create -f install/kubernetes/helm/helm-service-account.yaml
> helm init --service-account tiller --upgrade
> helm install install/kubernetes/helm/istio --name istio --namespace istio-system --timeout 1000
```

4. Then `cd` into your root of project

```
> ./cs-build.sh
> ./cs-inject-istio.sh
```

5. Install `web` chart

```
> helm install -n web deploys/charts/web
```

6. Add hosts file with following content

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

7. Clean up `coolstore` application as

```
> kubectl delete -f deployment/istio/dev-all-in-one.yaml
> helm delete web --purge
> helm delete istio --purge
```

**Notes**:
> If you want to run just only `Kubernetes` + `nginx-ingress` go to `deploys/charts/coolstore/values.yaml` and add
>```
> nginx:
>    enabled: true
>```
> Then run the `helm` command as
> ```
> helm install --name cs-nginx stable/nginx-ingress
> ```

> If you run it on `Docker for Windows`, then you cannot run sidecar auto injection so that we need to export `coolstore` chart to manifest file like
> ```
> helm template deploys/charts/coolstore -f deploys/charts/coolstore/values.dev.yaml --namespace cs-system > deploys/istio/dev-all-in-one.yaml
> istioctl kube-inject -f deploys/istio/dev-all-in-one.yaml | kubectl apply -f -
>```

### Open API

![OpenAPI Screenshot](assets/images/open-api.png?raw=true 'OpenAPI')

### CI/CD

![Lift and Shift](assets/images/lift-and-shift.PNG?raw=true 'liftandshift')

### Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :p

### Contributors

| [Thang Chung](https://github.com/thangchung)                                                         | [Thinh Nguyen](https://github.com/thinhnotes)                                                          |
| ---------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| <img src="https://avatars3.githubusercontent.com/u/422341?s=460&v=4"  alt="thangchung" width="150"/> | <img src="https://avatars2.githubusercontent.com/u/4660531?s=460&v=4" alt="thinhnguyen" width="150" /> |

### Licence

Code released under [the MIT license](https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE).
