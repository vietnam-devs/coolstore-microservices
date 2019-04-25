FROM node:alpine

RUN apk update && apk add libc6-compat
RUN GRPC_HEALTH_PROBE_VERSION=v0.2.0 && \
  wget -qO/bin/grpc_health_probe https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 && \
  chmod +x /bin/grpc_health_probe

RUN npm install -g typescript

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}

WORKDIR /app
COPY ./src/grpc/v1/catalog.proto ./proto/
COPY ./src/grpc/health/v1/health.proto ./proto/
COPY ./src/services/catalog .
RUN yarn install

EXPOSE 5002
ENV NODE_ENV production

CMD ["yarn", "start"]
