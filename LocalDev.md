# Development with Docker on localhost

## SQL Server Setup

```bash
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" --name sqlserver -p 1401:1433 -d mcr.microsoft.com/mssql/server:2017-latest
```

## Redis Setup

```bash
$ docker run --name redis -e REDIS_PASSWORD=letmein -p 6379:6379 -d bitnami/redis:5.0.5-debian-9-r124
```

## Linkerd
