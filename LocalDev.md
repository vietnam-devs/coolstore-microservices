# Development with Docker on localhost

## SQL Server Setup

```bash
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" --name sqlserver -p 1401:1433 -d mcr.microsoft.com/mssql/server:2017-latest
```

## Redis Setup

```bash
$ docker run --name redis -e REDIS_PASSWORD=letmein -p 6379:6379 -d bitnami/redis:5.0.5-debian-9-r124
```

Now you can develop the application on Visual Studio or Visual Code.

# Up and running on Docker Compose

```bash
$ docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
```

# Up and running on Kubernetes and Service Mesh

## Kubernetes

```bash
$ kubectl apply -f coolstore-infrastructure.yaml
```

Waiting a minute until all pods running with UP status

```bash
$ kubectl apply -f coolstore-migration.yaml
```

```bash
$ kubectl apply -f coolstore.yaml
```

## Octant

```bash
$ octant
```

## Linkerd

```bash
$ kubectl get -n coolstore deploy -o yaml | linkerd inject - | kubectl apply -f -
$ linkerd dashboard --port 9999
```

## Clean up

```bash
$ kubectl delete -f coolstore.yaml
$ kubectl delete -f coolstore-migration.yaml
$ kubectl delete -f coolstore-infrastructure.yaml
```
