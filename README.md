# CoolStore Web Application - :ferris_wheel: Modern Application on Dapr and Tye :sailboat:

![Travis (.org)](https://travis-ci.org/vietnam-devs/coolstore-microservices.svg?branch=master)
[![Price](https://img.shields.io/badge/price-FREE-0098f7.svg)](https://github.com/vietnam-devs/coolstore-microservices/blob/master/LICENSE)

CoolStore Website is a containerised microservices application consisting of services based on .NET Core running on Dapr. It demonstrates how to wire up small microservices into a larger application using microservice architectural principals.

Read [documentation](https://vietnam-devs.github.io/coolstore-microservices) for more information.

The business domain is inspired from [CoolStore project](https://github.com/jbossdemocentral/coolstore-microservice) by [JBoss Demo Central](https://github.com/jbossdemocentral) and [Red Hat Demo Central](https://gitlab.com/redhatdemocentral).

Check out my [medium](https://medium.com/@thangchung), or my [dev.to](https://dev.to/thangchung) or say hi on [Twitter](https://twitter.com/thangchung)!

## Public presentation

- [Service Mesh on AKS, the future is now - Microsoft Build event in May 2019](https://mybuild.techcommunity.microsoft.com/sessions/77172?source=TechCommunity)
- [From Microservices to Service Mesh - DevCafe event in July 2018](https://www.slideshare.net/ThangChung/from-microservices-to-service-mesh-devcafe-event-july-2018)
- [Service Mesh for Microservices- Vietnam Mobile Day event in June 2018](https://www.slideshare.net/ThangChung/service-mesh-for-microservices-vietnam-mobile-day-june-2017)
- [Avoid SPOF in Cloud-native Apps - Vietnam Web Summit event in December 2018](https://www.slideshare.net/ThangChung/avoid-single-point-of-failure-in-cloud-native-application)

# Table of contents

- [Try it!](https://github.com/vietnam-devs/coolstore-microservices#try-it)
- [Dapr Building Blocks](https://github.com/vietnam-devs/coolstore-microservices#dapr-building-blocks)
- [Screenshots](https://github.com/vietnam-devs/coolstore-microservices#screenshots)
- [OS, SDK, library, tooling and prerequisites](https://github.com/vietnam-devs/coolstore-microservices#os-sdk-library-tooling-and-prerequisites)
- [High level software architecture](https://github.com/vietnam-devs/coolstore-microservices#high-level-software-architecture)
- [µService development](https://github.com/vietnam-devs/coolstore-microservices#µmicroservice-development)
- [Open API](https://github.com/vietnam-devs/coolstore-microservices#open-api)
- [CI/CD](https://github.com/vietnam-devs/coolstore-microservices#ci-cd)
- [Service mesh](https://github.com/vietnam-devs/coolstore-microservices#service-mesh)
- [Contributing](https://github.com/vietnam-devs/coolstore-microservices#contributing)
- [Contributors](https://github.com/vietnam-devs/coolstore-microservices#contributors)
- [Licence](https://github.com/vietnam-devs/coolstore-microservices#licence)

## Try it

Make sure you have [`dapr`](https://docs.dapr.io/getting-started/install-dapr/) and [`tye`](https://github.com/dotnet/tye/blob/master/docs/getting_started.md) installed on your machine!

### Only wanna see wth is it?

```
$ tye run
```

Go to [`http://localhost:8000`](http://localhost:8000), and you're able to access to several endpoints whenevever it's ready as below:

- Web App: [`http://localhost:3000`](http://localhost:3000)
- Web Api Gateway: [`http://localhost:5000`](http://localhost:5000)
- Identity Server: [`http://localhost:5001`](http://localhost:5001)

### Wanna go deeply to see how can we built it!

1. Start core components

```
$ tye run tye.slim.yaml
```

2. Start dapr apps locally via dapr cli

```
$ dapr run --app-port 5001 --app-id identityapp dotnet run -- -p src\Services\Identity\IdentityService\IdentityService.csproj
```

```
$ dapr run --app-port 5002 --app-id inventoryapp dotnet run -- -p src\Services\Inventory\InventoryService.Api\InventoryService.Api.csproj
```

```
$ dapr run --app-port 5003 --app-id productcatalogapp dotnet run -- -p src\Services\ProductCatalog\ProductCatalogService.Api\ProductCatalogService.Api.csproj
```

```
$ dapr run --app-port 5004 --app-id shoppingcartapp dotnet run -- -p src\Services\ShoppingCart\ShoppingCartService.Api\ShoppingCartService.Api.csproj
```

Now, you can start to develop, debug or explore more about `dapr` with `tye` via Coolstore Apps.

> Enable `vm.max_map_count` for ElasticSearch via run `sysctl -w vm.max_map_count=262144`

## Dapr Building Blocks

<table>
  <thead>
    <th>Name</th>
    <th>Usecase</th>
    <th>Apps Participants</th>
  </thead>
  <tbody>
    <tr>
      <td><b>Service-to-service invocation</b></td>
      <td>User clicks to the detail product</td>
      <td>productcatalogapp, inventoryapp</td>
    </tr>
    <tr>
      <td><b>State management</b></td>
      <td>Items in the shopping cart</td>
      <td>shoppingcartapp</td>
    </tr>
    <tr>
      <td><b>Publish and subscribe</b></td>
      <td>User clicks checkout button, and the checkout process happens. It triggers the pub/sub flow in the system</td>
      <td>shoppingcartapp, saleapp, identityapp</td>
    </tr>
    <tr>
      <td><b>Resource bindings</b></td>
      <td>Every 30 seconds and 1 minutes the validation process happens. It will change the status of order from Received to Process and Complete via Cron binding</td>
      <td>productcatalogapp, inventoryapp</td>
    </tr>
    <tr>
      <td><b>Actors</b></td>
      <td>N/A</td>
      <td>N/A</td>
    </tr>
    <tr>
      <td><b>Observability</b></td>
      <td>All apps in the application are injected by daprd so that it's tracked and observed by dapr </td>
      <td>identityapp, webapigatewayapp, inventoryapp, productcatalogapp, shoppingcartapp, saleapp, web</td>
    </tr>
  </tbody>
</table>

## Screenshots

### Home page

![home-page](assets/images/ui-screen-1.PNG?raw=true)

### Shopping Cart page

![cart-page](assets/images/ui-screen-2.PNG?raw=true)

## OS, SDK, library, tooling and prerequisites

### Infrastructure

- **`Windows 10`** - the OS for developing and building this demo application.
- **[`Windows subsystem Linux - Ubuntu OS`](https://docs.microsoft.com/en-us/windows/wsl/install-win10)** - the subsystem that helps to run easily the bash shell on Windows OS.
- **[`Docker for desktop (Kubernetes enabled)`](https://www.docker.com/products/docker-desktop)** - the easiest tool to run Docker, Docker Swarm and Kubernetes on Mac and Windows.
- **[`Kubernetes`](https://kubernetes.io) / [`AKS`](https://docs.microsoft.com/en-us/azure/aks)** - the app is designed to run on Kubernetes (both locally on "Docker for Desktop", as well as on the cloud with AKS).
- **[`helm`](https://helm.sh)** - the best package manager to find, share, and use software built for Kubernetes.
- **[`dapr`](https://dapr.io/)** - an event-driven, portable runtime for building microservices on cloud and edge.

### Back-end

- **[`.NET Core 5.x`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core.
- **[`IdentityServer4`](https://identityserver.io)** - the Identity and Access Control solution for .NET Core.
- **[`gRPC`](https://grpc.io)** - a high-performance, open-source universal RPC framework.
- **[`Redis`](https://github.com/StackExchange/StackExchange.Redis)** - General purpose redis client.
- **[`NEST`](https://github.com/elastic/elasticsearch-net)** - Elasticsearch.Net & NEST.

### Front-end

- **[`nodejs 10.x`](https://nodejs.org/en/download)** - JavaScript runtime built on Chrome's V8 JavaScript engine.
- **[`typescript`](https://www.typescriptlang.org)** - a typed superset of JavaScript that compiles to plain JavaScript.
- **[`create-react-app`](https://facebook.github.io/create-react-app)** - a modern web app by running one command.

## High level software architecture

![Architecture Screenshot](assets/images/arch-diagram.png?raw=true 'Architecture Diagram')

## µService development

Guidance for developing µService can be found at [Clean Domain-Driven Design in 10 minutes](https://medium.com/@thangchung/clean-domain-driven-design-in-10-minutes-6037a59c8b7b)

![µService Screenshot](assets/images/miniservice-development.PNG?raw=true 'Microservice')

## Open API

![OpenAPI Screenshot](assets/images/open-api.png?raw=true 'OpenAPI')

https://documenter.getpostman.com/view/4847807/SVmvUeZv?version=latest#9f5ed7e4-e855-46e5-a42d-64edb31bc1cb

## CI/CD

![Lift and Shift](assets/images/lift-and-shift.PNG?raw=true 'liftandshift')

## Dapr components

TODO

## Debug and Tracing Apps

- Setup Kibana with `TraceId`, `HandlerName`, `RequestPath`, `level` and filter with `HandlerName`

![](assets/dapr/tracing_kibana_logs.png)

Then, you can find the exception happend in code via Kibana dashboard with settings above. Grab the `TraceId`, then paste it to `Zipkin` dashboard, then you can see the tracing of this request as the following picture

![](assets/dapr/tracing_zipkin.png)

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
