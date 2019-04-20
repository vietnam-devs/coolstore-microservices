# Cool Store - Kubernetes-based polyglot microservices application with Istio service mesh

![Travis (.org)](https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?branch=master)
[![Build Status](https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/vietnam-devs.coolstore-microservices)](https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=1)
[![Price](https://img.shields.io/badge/price-FREE-0098f7.svg)](https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE)
[![OpenTracing Badge](https://img.shields.io/badge/OpenTracing-enabled-blue.svg)](http://opentracing.io)

CoolStore is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Service Mesh. It demonstrates how to wire up small microservices into a larger application using microservice architectural principals. Read https://vietnam-devs.github.io for more information about how can we design and implement it.

The business domain for internal µservice is inspired from [CoolStore project](https://github.com/jbossdemocentral/coolstore-microservice) by [JBoss Demo Central](https://github.com/jbossdemocentral) and [Red Hat Demo Central](https://gitlab.com/redhatdemocentral). The readme file is inspired from [GoogleCloudPlatform Demo](https://github.com/GoogleCloudPlatform/microservices-demo).

## Public presentation

- [From Microservices to Service Mesh - DevCafe event in July 2018](https://www.slideshare.net/ThangChung/from-microservices-to-service-mesh-devcafe-event-july-2018)
- [Service Mesh for Microservices- Vietnam Mobile Day event in June 2018](https://www.slideshare.net/ThangChung/service-mesh-for-microservices-vietnam-mobile-day-june-2017)
- [Avoid SPOF in Cloud-native Apps - Vietnam Web Summit event in December 2018](https://www.slideshare.net/ThangChung/avoid-single-point-of-failure-in-cloud-native-application)

Check out my [blog](https://medium.com/@thangchung), my [chat](https://spectrum.chat/net-core) or say hi on [Twitter](https://twitter.com/thangchung)!

[Become a sponsor on Patreon](https://www.patreon.com/thangchung)

## Screenshots

<details>
  <summary>Home page</summary>

![home-page](assets/images/ui-screen-1.PNG?raw=true)

</details>

<details>
  <summary>Cart page</summary>

![cart-page](assets/images/ui-screen-2.PNG?raw=true)

</details>

# Table of contents

- [OS, SDK, library, tooling and prerequisites](https://github.com/vietnam-devs/coolstore-microservices#os-sdk-library-tooling-and-prerequisites)
- [High level software architecture](https://github.com/vietnam-devs/coolstore-microservices#high-level-software-architecture)
- [Installation](https://github.com/vietnam-devs/coolstore-microservices#installation)
- [µService development](https://github.com/vietnam-devs/coolstore-microservices#µmicroservice-development)
- [Open API](https://github.com/vietnam-devs/coolstore-microservices#open-api)
- [CI/CD](https://github.com/vietnam-devs/coolstore-microservices#ci-cd)
- [Service mesh](https://github.com/vietnam-devs/coolstore-microservices#service-mesh)
- [Contributing](https://github.com/vietnam-devs/coolstore-microservices#contributing)
- [Contributors](https://github.com/vietnam-devs/coolstore-microservices#contributors)
- [Licence](https://github.com/vietnam-devs/coolstore-microservices#licence)

## OS, SDK, library, tooling and prerequisites

- **`Windows 10`** - the OS for developing and building this demo application .
- **[`Windows subsystem Linux - Ubuntu OS`](https://docs.microsoft.com/en-us/windows/wsl/install-win10)** - the subsystem that helps to run easily the bash shell on Windows OS
- **[`Docker for desktop (Kubernetes enabled)`](https://www.docker.com/products/docker-desktop)** - the easiest tool to run Docker, Docker Swarm and Kubernetes on Mac and Windows
- **[`Kubernetes`](https://kubernetes.io) / [`AKS`](https://docs.microsoft.com/en-us/azure/aks)** - the app is designed to run on Kubernetes (both locally on "Docker for Desktop", as well as on the cloud with AKS)
- **[`istio`](https://istio.io)** - application works on Istio service mesh
- **[`helm`](https://helm.sh)** - the best package manager to find, share, and use software built for Kubernetes
- **[`envoy-proxy`](https://www.envoyproxy.io/)** - open source edge and service proxy, designed for cloud-native applications
- **[`.NET Core SDK 2.x`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- **[`nodejs 10.x`](https://nodejs.org/en/download)** - JavaScript runtime built on Chrome's V8 JavaScript engine
- **[`typescript`](https://www.typescriptlang.org)** - a typed superset of JavaScript that compiles to plain JavaScript
- **[`identityserver`](https://identityserver.io)** - the Identity and Access Control solution for .NET Core
- **[`gRPC`](https://grpc.io)** - a high-performance, open-source universal RPC framework
- **[`create-react-app`](https://facebook.github.io/create-react-app)** - a modern web app by running one command
- **[`vue-cli`](https://cli.vuejs.org/)** - standard tooling for Vue.js development
- **[`apollo-client`](https://www.apollographql.com/docs/react/)** - the best way to use GraphQL to build client applications
- **[`tanka-graphql`](https://pekkah.github.io/tanka-graphql)** - GraphQL execution library with SignalR based server and ApolloLink implementation
- **[`netcorekit`](https://github.com/cloudnative-netcore/netcore-kit)** - a crafted microservices toolkit for building cloud-native apps on the .NET platform

## High level software architecture

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

There are several individual µservices and infrastructure components that make up this app:

<table>
  <thead>
    <th>No.</th>
    <th>Service</th>
    <th>Description</th>
    <th>Source</th>
    <th>Endpoints</th>
  </thead>
  <tbody>
    <tr>
      <td align="center">1.</td>
      <td>
        IdP (.NET Core + In-memory database)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=4&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/identity?branchName=master" />
        </a>
      </td>
      <td>Uses <a href="https://github.com/IdentityServer/IdentityServer4">IdentityServer4</a> to authentication with OAuth 2.0 and OpenID Connect for the whole stack</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/idp">code</a>
      </td>
      <td>
        <a href="http://localhost:5001">dev</a> and <a href="http://id.coolstore.local">staging</a>
      </td>
    </tr>
    <tr>
      <td align="center">2.</td>
      <td>
        GraphQL server (.NET Core)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=12&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/graphql?branchName=master" />
        </a>
      </td>
      <td>The GraphQL server for backoffice application</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/graphql">code</a>
      </td>
      <td>
        <a href="http://localhost:5011">dev</a> and <a href="http://api.coolstore.local/graphql/playground">staging</a>
      </td>
     </tr>
     <tr>
      <td align="center">3.</td>
      <td>
        OpenApi (.NET Core + envoy-proxy)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=11&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/openapi?branchName=master" />
        </a>
      </td>
      <td>The OpenAPI which generated from gRPC contract files, hosted in OpenAPI format, and used envoy-proxy to proxy it</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/openapi">code</a>
      </td>
      <td>
        <a href="http://localhost:5010">dev</a> and <a href="http://api.coolstore.local/openapi/swagger">staging</a>
      </td>
     </tr>
     <tr>
      <td align="center">4.</td>
      <td>
        Web (PWA - Vuejs + Node.js)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=8&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/web?branchName=master" />
        </a>
      </td>
      <td>Frontend based on <a href="https://vuejs.org">vuejs</a> and <a href="https://nodejs.org">Node.js</a></td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/web">code</a>
      </td>
      <td>
        <a href="http://localhost:8080">dev</a> and <a href="http://web.coolstore.local">staging</a>
      </td>
     </tr>
     <tr>
      <td align="center">5.</td>
      <td>
        Backoffice (React + TypeScript + Apollo-client)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=10&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/backoffice?branchName=master" />
      </td>
      <td>The back office application for management business entities in the system</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/backoffice">code</a>
      </td>
      <td>
        <a href="http://localhost:8081">dev</a> and <a href="http://backoffice.coolstore.local">staging</a>
      </td>
    </tr>
    <tr>
      <td align="center">6.</td>
      <td>
        Catalog (Node.js + TypeScript + Mongo)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=3&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/catalog?branchName=master" />
        </a>
      </td>
      <td>Serves products and prices for retail products</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/catalog">code</a>
      </td>
      <td>
        <a href="http://localhost:5002">dev</a>
      </td>
     </tr>
     <tr>
      <td align="center">7.</td>
      <td>
        Cart (.NET Core + MySQL)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=2&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/cart?branchName=master" />
        </a>
      </td>
      <td>Manages shopping cart for each customer</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/cart">code</a>
      </td>
      <td>
        <a href="http://localhost:5003">dev</a>
      </td>
     </tr>
     <tr>
      <td align="center">8.</td>
      <td>
        Inventory (.NET Core + MySQL)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=5&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/inventory?branchName=master" />
        </a>
      </td>
      <td>Serves inventory and availability data for retail products</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/inventory">code</a>
      </td>
      <td>
        <a href="http://localhost:5004">dev</a>
      </td>
     </tr>
     <tr>
      <td align="center">9.</td>
      <td>
        Rating (Node.js + TypeScript + Mongo)<br/>
        <a href="https://dev.azure.com/vietnam-devs/coolstore-microservices/_build/latest?definitionId=6&branchName=master">
          <img src="https://dev.azure.com/vietnam-devs/coolstore-microservices/_apis/build/status/rating?branchName=master" />
        </a>
      </td>
      <td>Runs for rating products</td>
      <td>
        <a href="https://github.com/vietnam-devs/coolstore-microservices/tree/master/src/services/rating">code</a>
      </td>
      <td>
        <a href="http://localhost:5007">dev</a>
      </td>
     </tr>
  </tbody>
</table>

## Installation

### Development environment

#### Up and running manually with `Docker for desktop`

See https://vietnam-devs.github.io/docs/development/up-running-d4d-aks/#docker-for-desktop

#### Up and running with `docker compose`

```bash
$ docker-compose build
$ docker-compose up
```

### Staging and Production environments

#### Up and Running on `Azure Kubernetes Service` (`AKS`)

See https://vietnam-devs.github.io/docs/development/up-running-d4d-aks/#azure-kubernetes-service-aks

## µService development

Guidance for developing µService can be found at [Clean Domain-Driven Design in 10 minutes](https://medium.com/@thangchung/clean-domain-driven-design-in-10-minutes-6037a59c8b7b)

![µService Screenshot](assets/images/miniservice-development.PNG?raw=true 'Microservice')

## Open API

![OpenAPI Screenshot](assets/images/open-api.png?raw=true 'OpenAPI')

## CI/CD

![Lift and Shift](assets/images/lift-and-shift.PNG?raw=true 'liftandshift')

## Service mesh

[`istio`](https://istio.io) provide a wealth of benefits for the organizations that use them. There’s no denying, however, that adopting the cloud can put strains on DevOps teams. Developers must use microservices to architect for portability, meanwhile operators are managing extremely large hybrid and multi-cloud deployments. Istio lets you connect, secure, control, and observe services.

### Distributed tracing

![DAG chart](assets/images/jaeger-dag-1.PNG?raw=true 'DAG')

![Trace chart](assets/images/jaeger-trace-1.PNG?raw=true 'Trace')

### Metrics

![Metrics chart](assets/images/grafana-ui-1.PNG?raw=true 'Metrics')

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :p

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key))

<table>
  <tbody>
    <tr>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/thangchung.png?s=150">
        <br>
        <a href="https://github.com/thangchung">Thang Chung</a>
        <p>
          <a href="https://github.com/vietnam-devs/coolstore-microservices/commits?author=thangchung" title="Developer">💻</a>
          <a href="#question" title="Answering Questions">💬</a>
          <a href="#docs" title="Documentation">📖</a>
          <a href="#review" title="Reviewed Pull Requests">👀</a>
          <a href="#infra" title="Infrastructure">🚇</a>
          <a href="#maintance" title="Maintenance">🚧</a>
        </p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/tungphuong.png?s=150">
        <br>
        <a href="https://github.com/tungphuong">Phuong Le</a>
        <p>
          <a href="https://github.com/vietnam-devs/coolstore-microservices/commits?author=tungphuong" title="Developer">💻</a>
          <a href="#package" title="Packaging">📦</a>
          <a href="#infra" title="Infrastructure">🚇</a>
        </p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/trumhemcut.png?s=150">
        <br>
        <a href="https://github.com/trumhemcut">Phi Huynh</a>
        <p>
          <a href="#idea" title="Ideas & Planning">🤔</a>
          <a href="https://github.com/vietnam-devs/coolstore-microservices/commits?author=trumhemcut" title="Infrastructure">🚇</a>
        </p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/thinhnotes.png?s=150">
        <br>
        <a href="https://github.com/thinhnotes">Thinh Nguyen</a>
        <p>
          <a href="https://github.com/vietnam-devs/coolstore-microservices/commits?author=thinhnotes" title="Developer">💻</a>
          <a href="#maintance" title="Maintenance">🚧</a>
        </p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/stuartleeks.png?s=150">
        <br>
        <a href="https://github.com/stuartleeks">Stuart Leeks</a>
        <p>
          <a href="#docs" title="Documentation">📖</a>
        </p>
      </td>
     </tr>
  </tbody>
</table>

## Licence

Code released under [the MIT license](https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE).
