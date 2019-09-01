# Introduction

CoolStore Microservices is a containerised polyglot microservices application consisting of services based on .NET Core, NodeJS and more running on Service Mesh. It demonstrates how to wire up small microservices and build up a larger application using microservice architectural principals.

It's mainly building for .NET ecosystem with a lot of popular libraries and toolkits which have used by .NET community for a long time. Additionaly, it uses and experiements new components and libraries to build the modern application with cloud-native apps approach.

We have the [Vietnam Microservices Community Group](https://www.facebook.com/groups/645391349250568) to write, discuss and sharing tips and tricks of how can we build and maintain microservices architectural projects.

## Technical stack

### Technology

- **[.NET Core 2.2 SDK](https://github.com/dotnet/core):** .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- **[nodejs](https://github.com/nodejs/node):** Node.js JavaScript runtime
- **[typescript](https://github.com/Microsoft/TypeScript):** Superset of JavaScript that compiles to clean JavaScript output

### Tool and library

- **Windows 10:** The OS for developing and building this demo application
- **[Windows Subsystem Linux - Ubuntu OS](https://docs.microsoft.com/en-us/windows/wsl/install-win10):** The subsystem that helps to run easily the bash shell on Windows OS
- **[Docker for Desktop (Kubernetes enabled)](https://www.docker.com/products/docker-engine#/windows):** The easiest tool to run Docker, Docker Swarm and Kubernetes on Mac and Windows
- **[Docker Compose](https://github.com/docker/compose):** Define and run multi-container applications with Docker
- **[Kubernetes](https://github.com/kubernetes/kubernetes) / [AKS](https://docs.microsoft.com/en-us/azure/aks):** The app is designed to run on Kubernetes (both locally on "Docker for Desktop", as well as on the cloud with AKS)
- **[Istio](https://github.com/istio/istio):** Application works on Istio service mesh
- **[helm](https://github.com/helm/helm):** The best package manager to find, share, and use software built for Kubernetes
- **[Envoy Proxy](https://github.com/envoyproxy/envoy):** Cloud-native high-performance edge/middle/service proxy
- **[NetCoreKit](https://github.com/cloudnative-netcore/netcore-kit):** Set of Cloud-native tools and utilities for .NET Core

### Microservice Design Patterns

![](https://azurecomcdn.azureedge.net/mediahandler/acomblog/media/Default/blog/ba865aef-4e7f-4b97-a94b-517aacf8d29a.png)

Reference more at https://azure.microsoft.com/en-us/blog/design-patterns-for-microservices