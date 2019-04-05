### Up and running on local development

- Change you working location

```bash
> cd <root of project>
> bash
```

- Build catalog service

```bash
> src/services/catalog/build/gen_proto.sh
> src/services/catalog/build/build_image.sh
```

- Build cart service

```bash
> src/services/cart/App_Build/gen_proto.sh
> src/services/cart/App_Build/build_image.sh
```

- Build inventory service

```bash
> src/services/inventory/App_Build/gen_proto.sh
> src/services/inventory/App_Build/build_image.sh
```

- Build rating service

```bash
> src/services/rating/build/build_image.sh
> src/services/rating/build/gen_proto.sh
```

- Build idp service

```bash
> src/services/idp/App_Build/build_image.sh
```

- Build open-api service

```bash
> src/services/open-api/build_image.sh
> src/services/open-api/gen_proto.sh
```

- Build envoy-proxy

```bash
> src/services/envoy-proxy/build_image.sh
> src/services/envoy-proxy/gen_proto.sh
```

- Run `docker-compose.yml`

```bash
> cd <root of project>
> docker-compose up
```

- Urls can access to

```bash
> http://localhost:8081 => for the admin of envoy
> http://localhost:8082/swagger-ui
```
