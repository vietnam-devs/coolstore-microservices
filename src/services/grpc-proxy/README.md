# Up and running

- Install `golang` into `WSL` by following the guidance at https://medium.com/@patdhlk/how-to-install-go-1-9-1-on-ubuntu-16-04-ee64c073cd79

- The `GOROOT` and `GOPATH` environment variables should:

```
GOROOT="/usr/local/go"
GOPATH="/mnt/e/oss/github/coolstore-microservices"
```

- The run commands as below

```
> cd <root of project>
>./src/services/review/App_Build/gen_proto.sh
> go run src/services/grpc-proxy/main.go
```
