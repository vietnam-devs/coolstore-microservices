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

```bash
$ cd deploys\linkerd
```

## Kubernetes

```bash
$ kubectl apply -f coolstore-infrastructure.yaml
```

Waiting a minute until all pods running with UP status

```bash
$ kubectl apply -f coolstore-migration.yaml
```

```bash
$ kubectl apply -f coolstore.yaml -f gateway.yaml
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

## Run with gateway

Open `hosts` file at `C:\Windows\System32\drivers\etc`, and add some lines as below

```
127.0.0.1	id.coolstore.local
127.0.0.1	api.coolstore.local
127.0.0.1	coolstore.local
```

Now, you can access the `identity` and `api` at [`http://id.coolstore.local:31888`](http://id.coolstore.local:31888) and [`http://api.coolstore.local:31666`](http://api.coolstore.local:31666)

## Clean up

```bash
$ kubectl delete -f gateway.yaml -f coolstore.yaml -f coolstore-migration.yaml -f coolstore-infrastructure.yaml
```
